using Prism.Events;
using SimpleBusinessApp.Data;
using SimpleBusinessApp.Event;
using SimpleBusinessApp.Model;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace SimpleBusinessApp.ViewModel
{
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        private IClientLookupDataService _clientLookupDataService;
        private IEventAggregator _eventAggregator;

        public ObservableCollection<NavigationItemViewModel> Clients { get; }
        private NavigationItemViewModel _selectedClient;

        public NavigationItemViewModel SelectedClient
        {
            get { return _selectedClient; }
            set
            {
                _selectedClient = value;
                OnPropertyChanged();
                if (_selectedClient != null)
                {
                    _eventAggregator.GetEvent<OpenClientDetailViewEvent>()
                        .Publish(_selectedClient.Id);
                }
            }
        }


        public NavigationViewModel(IClientLookupDataService clientLookupDataService, IEventAggregator eventAggregator)
        {
            _clientLookupDataService = clientLookupDataService;
            _eventAggregator = eventAggregator;
            Clients = new ObservableCollection<NavigationItemViewModel>();
            _eventAggregator.GetEvent<AfterClientSaveEvent>().Subscribe(AfterClientSaved);
        }

        private void AfterClientSaved(AfterClientSaveEventArgs obj)
        {
            var lookupItem = Clients.Single(l=>l.Id==obj.Id);
            lookupItem.DisplayMember = obj.DisplayMember;
        }

        public async Task LoadAsync()
        {
            var lookup = await _clientLookupDataService.GetClientLookupAsync();
            Clients.Clear();
            foreach (var item in lookup)
            {
                Clients.Add(new NavigationItemViewModel(item.Id, item.DisplayMember));
            }
        }
    }
}
