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
        private IMeetingLookupDataService _meetingLookupDataService;
        private IEventAggregator _eventAggregator;

        public ObservableCollection<NavigationItemViewModel> Clients { get; }
        public ObservableCollection<NavigationItemViewModel> Meetings { get; } // usually it is ok to create separate navigations for every type


        public NavigationViewModel(IClientLookupDataService clientLookupDataService,
            IMeetingLookupDataService meetingLookupDataService,
            IEventAggregator eventAggregator)
        {
            _clientLookupDataService = clientLookupDataService;
            _meetingLookupDataService = meetingLookupDataService;
            _eventAggregator = eventAggregator;
            Clients = new ObservableCollection<NavigationItemViewModel>();
            Meetings = new ObservableCollection<NavigationItemViewModel>();
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


            lookup = await _meetingLookupDataService.GetMeetingLookupAsync();
            Meetings.Clear();
            foreach (var item in lookup)
            {
                Meetings.Add(new NavigationItemViewModel(item.Id, item.DisplayMember,
                    nameof(MeetingDetailViewModel), _eventAggregator));
            }

        }

        private void AfterDetailSaved(AfterDetailSavedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(ClientDetailViewModel):
                    AfterDetailSaved(Clients, args);
                    break;

                case nameof(MeetingDetailViewModel):
                    AfterDetailSaved(Meetings, args);
                    break;
            }
        }

        private void AfterDetailSaved(ObservableCollection<NavigationItemViewModel> items,
            AfterDetailSavedEventArgs args)
        {
            var lookupItem = items.SingleOrDefault(l => l.Id == args.Id);
            if (lookupItem == null)
            {
                items.Add(new NavigationItemViewModel(args.Id, args.DisplayMember,
                    args.ViewModelName,
                    _eventAggregator));
            }
            else
            {
                lookupItem.DisplayMember = args.DisplayMember;
            }
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {

            switch (args.ViewModelName)
            {
                case nameof(ClientDetailViewModel):
                    AfterDetailDeleted(Clients, args);
                    break;
                case nameof(MeetingDetailViewModel):
                    AfterDetailDeleted(Meetings, args);
                    break;
            }
        }

        private void AfterDetailDeleted(ObservableCollection<NavigationItemViewModel> items, 
            AfterDetailDeletedEventArgs args)
        {
            var item = items.SingleOrDefault(c => c.Id == args.Id);
            if (item != null)
            {
                items.Remove(item);
            }
        }
    }
}
