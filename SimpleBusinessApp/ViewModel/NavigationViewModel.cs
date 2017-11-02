using Prism.Events;
using SimpleBusinessApp.Event;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using SimpleBusinessApp.Data.Lookups;
using System;

namespace SimpleBusinessApp.ViewModel
{
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        private IClientLookupDataService _clientLookupDataService;
        private IEventAggregator _eventAggregator;

        public ObservableCollection<NavigationItemViewModel> Clients { get; }

        public NavigationViewModel(IClientLookupDataService clientLookupDataService, IEventAggregator eventAggregator)
        {
            _clientLookupDataService = clientLookupDataService;
            _eventAggregator = eventAggregator;
            Clients = new ObservableCollection<NavigationItemViewModel>();
            _eventAggregator.GetEvent<AfterClientSaveEvent>().Subscribe(AfterClientSaved);
            _eventAggregator.GetEvent<AfterClientDeletedEvent>().Subscribe(AfterClientDeleted);
        }

        private void AfterClientDeleted(int clientId)
        {
            var client = Clients.SingleOrDefault(c => c.Id == clientId);
            if (client != null)
            {
                Clients.Remove(client);
            }
        }

        private void AfterClientSaved(AfterClientSaveEventArgs obj)
        {
            var lookupItem = Clients.SingleOrDefault(l => l.Id == obj.Id);
            if (lookupItem == null)
            {
                Clients.Add(new NavigationItemViewModel(obj.Id, obj.DisplayMember, _eventAggregator));
            }
            else
            {
                lookupItem.DisplayMember = obj.DisplayMember;
            }
        }

        public async Task LoadAsync()
        {
            var lookup = await _clientLookupDataService.GetClientLookupAsync();
            Clients.Clear();
            foreach (var item in lookup)
            {
                Clients.Add(new NavigationItemViewModel(item.Id, item.DisplayMember, _eventAggregator));
            }
        }
    }
}
