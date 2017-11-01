using Prism.Events;
using SimpleBusinessApp.Data;
using SimpleBusinessApp.Event;
using SimpleBusinessApp.Model;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace SimpleBusinessApp.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private IEventAggregator _eventAggregator;
        private Func<IClientDetailViewModel> _clientDetailViewModelCreator;

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
            IEventAggregator eventAggregator ) 
        {
            _eventAggregator = eventAggregator;
           
            _clientDetailViewModelCreator = clientDetailViewModelCreator;

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
            if (ClientDetailViewModel != null && ClientDetailViewModel.HasChanges)
            {
                var result = MessageBox.Show("You`ve made changes. Do you wish to leave?", "Question",
                    MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
            }
            ClientDetailViewModel = _clientDetailViewModelCreator();
            await ClientDetailViewModel.LoadAsync(clientId);
        }       
    }

}

