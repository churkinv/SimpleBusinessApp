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

namespace SimpleBusinessApp.ViewModel
{
    public class MeetingDetailViewModel : DetailViewModelBase, IMeetingDetailViewModel
    {

        private IMeetingRepository _meetingRepository;
        private MeetingWrapper _meeting;
        private IMessageDialogService _messageDialogService;

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
            IMeetingRepository meetingRepository) : base (eventAggregator)
        {
            _meetingRepository = meetingRepository;
            _messageDialogService = messageDialogService;

            AddedClients = new ObservableCollection<Client>();
            AvailableClients = new ObservableCollection<Client>();
            AddClientCommand = new DelegateCommand(OnAddClientExecute, OnAddClientCanExecute);
            RemoveClientCommand = new DelegateCommand(OnRemoveClientExecute, OnRemoveClientCanExecute);
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

        public override async Task LoadAsync(int? meetingId)
        {
            var meeting = meetingId.HasValue
                ? await _meetingRepository.GetByIdAsync(meetingId.Value)
                : CreateNewMeeting();

            InitializeMeeting(meeting);
            //TODO: Load the clients for the picklist
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
             };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            if (Meeting.Id == 0)
            {
                Meeting.Title = "";
            }
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
            RaiseDetailSavedEvent(Meeting.Id, Meeting.Title);
        }

        protected override void OnDeleteExecute()
        {
            var result = _messageDialogService.ShowOkCancelDialog($"Do you really want to delete the meeting {Meeting.Title}?", "Question");
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

        }

        private bool OnRemoveClientCanExecute()
        {
            return SelectedAddedClient != null;
        }

        private void OnRemoveClientExecute()
        {

        }

    }
}
