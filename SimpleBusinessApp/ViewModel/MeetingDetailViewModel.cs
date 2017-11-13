using Prism.Events;
using SimpleBusinessApp.Data.Repository;
using SimpleBusinessApp.View.Services;
using SimpleBusinessApp.Wrapper;
using System;
using System.Threading.Tasks;
using SimpleBusinessApp.Model;
using Prism.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using SimpleBusinessApp.Event;

namespace SimpleBusinessApp.ViewModel
{
    public class MeetingDetailViewModel : DetailViewModelBase, IMeetingDetailViewModel
    {

        private IMeetingRepository _meetingRepository;
        private MeetingWrapper _meeting;
        private Client _selectedAvailableClient;
        public Client SelectedAvailableClient
        {
            get { return _selectedAvailableClient; }
            set
            {
                _selectedAvailableClient = value;
                OnPropertyChanged();
                ((DelegateCommand)AddClientCommand).RaiseCanExecuteChanged();
            }
        }
        private Client _selectedAddedClient;
        private List<Client> _allClients;

        public Client SelectedAddedClient
        {
            get { return _selectedAddedClient; }
            set
            {
                _selectedAddedClient = value;
                OnPropertyChanged();
                ((DelegateCommand)RemoveClientCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand AddClientCommand { get; }
        public ICommand RemoveClientCommand { get; }

        public ObservableCollection<Client> AddedClients { get; }
        public ObservableCollection<Client> AvailableClients { get; }

        public MeetingDetailViewModel(IEventAggregator eventAggregator, IMessageDialogService messageDialogService,
            IMeetingRepository meetingRepository) : base(eventAggregator, messageDialogService)
        {
            _meetingRepository = meetingRepository;
            eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);

            AddedClients = new ObservableCollection<Client>();
            AvailableClients = new ObservableCollection<Client>();
            AddClientCommand = new DelegateCommand(OnAddClientExecute, OnAddClientCanExecute);
            RemoveClientCommand = new DelegateCommand(OnRemoveClientExecute, OnRemoveClientCanExecute);
        }

        private async void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            if (args.ViewModelName == nameof(ClientDetailViewModel))
            {
                _allClients = await _meetingRepository.GetAllClientsAsync();
                SetupPicklist();
            }
        }

        private async void AfterDetailSaved(AfterDetailSavedEventArgs args)
        {
            if (args.ViewModelName == nameof(ClientDetailViewModel))
            {
                await _meetingRepository.ReloadClientAsync(args.Id);
                _allClients = await _meetingRepository.GetAllClientsAsync();
                SetupPicklist();
            }
        }

        public MeetingWrapper Meeting
        {
            get { return _meeting; }
            private set
            {
                _meeting = value;
                OnPropertyChanged();
            }
        }

        public override async Task LoadAsync(int meetingId)
        {
            var meeting = meetingId > 0
                ? await _meetingRepository.GetByIdAsync(meetingId)
                : CreateNewMeeting();

            Id = meetingId;

            InitializeMeeting(meeting);

            _allClients = await _meetingRepository.GetAllClientsAsync();

            SetupPicklist();
        }

        private void SetupPicklist()
        {
            var meetingClientIds = Meeting.Model.Clients.Select(c => c.Id).ToList();
            var addedClients = _allClients.Where(c => meetingClientIds.Contains(c.Id)).OrderBy(c => c.FirstName);
            var availibleClients = _allClients.Except(AddedClients).OrderBy(c => c.FirstName);

            AddedClients.Clear();
            AvailableClients.Clear();
            foreach (var addedClient in addedClients)
            {
                AddedClients.Add(addedClient);
            }

            foreach (var availibleClient in availibleClients)
            {
                AvailableClients.Add(availibleClient);
            }
        }

        private void InitializeMeeting(Meeting meeting)
        {
            Meeting = new MeetingWrapper(meeting);
            Meeting.PropertyChanged += (s, e) =>
             {
                 if (!HasChanges)
                 {
                     HasChanges = _meetingRepository.HasChanges();
                 }

                 if (e.PropertyName == nameof(Meeting.HasErrors))
                 {
                     ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                 }
                 if (e.PropertyName == nameof(Meeting.Title))
                 {
                     SetTitle();
                 }
             };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            if (Meeting.Id == 0)
            {
                Meeting.Title = "";
            }

            SetTitle();
        }

        private void SetTitle()
        {
            Title = Meeting.Title;
        }

        private Meeting CreateNewMeeting()
        {
            var meeting = new Meeting
            {
                DateFrom = DateTime.Now.Date,
                DateTo = DateTime.Now.Date
            };

            _meetingRepository.Add(meeting);
            return meeting;
        }

        protected override bool OnSaveCanExecute()
        {
            return Meeting != null && !Meeting.HasErrors && HasChanges;
        }

        protected override async void OnSaveExecute()
        {
            await _meetingRepository.SaveAsync();
            HasChanges = _meetingRepository.HasChanges();
            Id = Meeting.Id;
            RaiseDetailSavedEvent(Meeting.Id, Meeting.Title);
        }

        protected override void OnDeleteExecute()
        {
            var result = MessageDialogService.ShowOkCancelDialog($"Do you really want to delete the meeting {Meeting.Title}?", "Question");
            if (result == MessageDialogResult.Ok)
            {
                _meetingRepository.Remove(Meeting.Model);
                _meetingRepository.SaveAsync();
                RaiseDetailDeletedEvent(Meeting.Id);
            }
        }

        private bool OnAddClientCanExecute()
        {
            return SelectedAvailableClient != null;
        }

        private void OnAddClientExecute()
        {
            var clientToAdd = SelectedAvailableClient;
            Meeting.Model.Clients.Add(clientToAdd);
            AddedClients.Add(clientToAdd);
            AvailableClients.Remove(clientToAdd);
            HasChanges = _meetingRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private bool OnRemoveClientCanExecute()
        {
            return SelectedAddedClient != null;
        }

        private void OnRemoveClientExecute()
        {
            var clientToRemove = SelectedAddedClient;
            Meeting.Model.Clients.Remove(clientToRemove);
            AddedClients.Remove(clientToRemove);
            AvailableClients.Add(clientToRemove);
            HasChanges = _meetingRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

    }
}
