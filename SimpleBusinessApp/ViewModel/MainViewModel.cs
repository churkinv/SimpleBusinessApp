using Prism.Events;
using SimpleBusinessApp.Event;
using SimpleBusinessApp.View.Services;
using System;
using System.Threading.Tasks;

namespace SimpleBusinessApp.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private IEventAggregator _eventAggregator;
        private Func<IClientDetailViewModel> _clientDetailViewModelCreator;
        private IMessageDialogService _messageDialogService;

        public INavigationViewModel NavigationViewModel { get; }
        private IClientDetailViewModel _clientDetailViewModel;

        public IClientDetailViewModel ClientDetailViewModel
        {
            get { return _clientDetailViewModel; }
            set
            {
                _clientDetailViewModel = value;
                OnPropertyChanged();
            }            
        }

        public MainViewModel(INavigationViewModel navigationViewModel, 
            Func<IClientDetailViewModel> clientDetailViewModelCreator, 
            IEventAggregator eventAggregator, IMessageDialogService messageDialogService) 
        {
            _eventAggregator = eventAggregator;
            _clientDetailViewModelCreator = clientDetailViewModelCreator;
            _messageDialogService = messageDialogService;

            _eventAggregator.GetEvent<OpenClientDetailViewEvent>()
              .Subscribe(OnOpenClientDetailView);

            NavigationViewModel = navigationViewModel;        
        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();           
        }

        private async void OnOpenClientDetailView(int clientId)
        {
            //it is not a good idea to use MessageBox directly in our viewmodel as this would block unit test on this method
            if (ClientDetailViewModel != null && ClientDetailViewModel.HasChanges)
            {
                var result = _messageDialogService.ShowOkCancelDialog("You`ve made changes. Do you wish to leave?", "Question");
                if (result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }
            ClientDetailViewModel = _clientDetailViewModelCreator();
            await ClientDetailViewModel.LoadAsync(clientId);
        }       
    }

}

