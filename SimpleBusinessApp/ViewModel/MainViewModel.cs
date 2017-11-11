using Prism.Commands;
using Prism.Events;
using SimpleBusinessApp.Event;
using SimpleBusinessApp.View.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleBusinessApp.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private IEventAggregator _eventAggregator;
        private Func<IClientDetailViewModel> _clientDetailViewModelCreator;
        private Func<IMeetingDetailViewModel> _meetingDetailViewModelCreator;
        private IMessageDialogService _messageDialogService;
        private IDetailViewModel _detailViewModel;

        public ICommand CreateNewDetailCommand { get; }
        public INavigationViewModel NavigationViewModel { get; }
        public IDetailViewModel DetailViewModel
        {
            get { return _detailViewModel; }
            set
            {
                _detailViewModel = value;
                OnPropertyChanged();
            }            
        }

        public MainViewModel(INavigationViewModel navigationViewModel, 
            Func<IClientDetailViewModel> clientDetailViewModelCreator, 
            Func<IMeetingDetailViewModel> meetingDetailViewModelCreator,
            IEventAggregator eventAggregator, IMessageDialogService messageDialogService) 
        {
            _eventAggregator = eventAggregator;
            _clientDetailViewModelCreator = clientDetailViewModelCreator;
            _meetingDetailViewModelCreator = meetingDetailViewModelCreator;
            _messageDialogService = messageDialogService;

            _eventAggregator.GetEvent<OpenDetailViewEvent>()
              .Subscribe(OnOpenDetailView); // this event is published by NavigationViewModel when an item is clicked
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);

            CreateNewDetailCommand = new DelegateCommand<Type>(OnCreateNewDetailExecute);

            NavigationViewModel = navigationViewModel;        
        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();           
        }

        private async void OnOpenDetailView(OpenDetailViewEventArgs args)
        {
            //it is not a good idea to use MessageBox directly in our viewmodel as this would block unit test on this method
            if (DetailViewModel != null && DetailViewModel.HasChanges)
            {
                var result = _messageDialogService.ShowOkCancelDialog("You`ve made changes. Do you wish to leave?", "Question");
                if (result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }
            switch (args.ViewModelName)
            {
                case nameof(ClientDetailViewModel):
                    DetailViewModel = _clientDetailViewModelCreator();
                    break;

                case nameof(MeetingDetailViewModel):
                    DetailViewModel = _meetingDetailViewModelCreator();
                    break;
                default:
                    throw new Exception($"ViewModel {args.ViewModelName} not mapped");
                    // break; is no need as we throw an exception
            }

            await DetailViewModel.LoadAsync(args.Id);
        }

        private void OnCreateNewDetailExecute(Type viewModelType)
        {
            OnOpenDetailView(
                new OpenDetailViewEventArgs { ViewModelName = viewModelType.Name});
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            DetailViewModel = null;
        }
    }
}

