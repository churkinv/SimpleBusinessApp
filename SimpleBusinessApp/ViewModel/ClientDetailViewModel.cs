using Prism.Commands;
using Prism.Events;
using SimpleBusinessApp.Data;
using SimpleBusinessApp.Event;
using SimpleBusinessApp.Model;
using SimpleBusinessApp.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleBusinessApp.ViewModel
{
    /// <summary>
    /// Loads a single Client
    /// </summary>
    public class ClientDetailViewModel : ViewModelBase, IClientDetailViewModel
    {
        private IClientDataService _clientDataService;
        private IEventAggregator _eventAggregator;
        private ClientWrapper _client;
        public ClientWrapper Client
        {
            get { return _client; }
            private set
            {
                _client = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; }
        
        public ClientDetailViewModel(IClientDataService dataService, IEventAggregator eventAggregator)
        {
            _clientDataService = dataService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<OpenClientDetailViewEvent>()
                .Subscribe(OnOpenClientDetailView);
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
        }

        public async Task LoadAsync(int clientId)
        {
            var client = await _clientDataService.GetByIdAsync(clientId);
            Client = new ClientWrapper(client);

            Client.PropertyChanged += (s, e) =>
              {
                  if (e.PropertyName == nameof(Client.HasErrors))
                  {
                      ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                  }  
              };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private async void OnSaveExecute()
        {
            await _clientDataService.SaveAsync(Client.Model);
            _eventAggregator.GetEvent<AfterClientSaveEvent>().Publish(
                new AfterClientSaveEventArgs
                {
                    Id = Client.Id,
                    DisplayMember=$"{Client.FirstName} {Client.LastName}"            
                });
        }

        private bool OnSaveCanExecute()
        {
            //TODO check in additions if Client has changes
            return Client != null && !Client.HasErrors;
        }

        private async void OnOpenClientDetailView(int clientId)
        {
            await LoadAsync(clientId);
        }

       
    }
}
