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
            _eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
        }

        public async Task LoadAsync()
        {
            var lookup = await _clientLookupDataService.GetClientLookupAsync();
            Clients.Clear();
            foreach (var item in lookup)
            {
                Clients.Add(new NavigationItemViewModel(item.Id, item.DisplayMember, nameof(ClientDetailViewModel), _eventAggregator));
            }
        }

        private void AfterDetailSaved(AfterDetailSavedEventArgs obj)
        {
            switch (obj.ViewModelName)
            {
                case nameof(ClientDetailViewModel):

                    var lookupItem = Clients.SingleOrDefault(l => l.Id == obj.Id);
                    if (lookupItem == null)
                    {
                        Clients.Add(new NavigationItemViewModel(obj.Id, obj.DisplayMember, nameof(ClientDetailViewModel), _eventAggregator));
                    }
                    else
                    {
                        lookupItem.DisplayMember = obj.DisplayMember;
                    }
                    break;
            }
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {

            switch (args.ViewModelName)
            {
                case nameof(ClientDetailViewModel):
                    var client = Clients.SingleOrDefault(c => c.Id == args.Id);
                    if (client != null)
                    {
                        Clients.Remove(client);
                    }
                    break;
                    // for another entity just create new case
            }
        }
    }
}
