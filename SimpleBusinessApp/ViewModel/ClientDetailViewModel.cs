using Prism.Commands;
using Prism.Events;
using SimpleBusinessApp.Data.Repositories;
using SimpleBusinessApp.Event;
using SimpleBusinessApp.Wrapper;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleBusinessApp.ViewModel
{
    /// <summary>
    /// Loads a single Client
    /// </summary>
    public class ClientDetailViewModel : ViewModelBase, IClientDetailViewModel
    {
        private IClientRepository _clientRepository;
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
        
        public ClientDetailViewModel(IClientRepository clientRepository, IEventAggregator eventAggregator)
        {
            _clientRepository = clientRepository;
            _eventAggregator = eventAggregator;
          
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
        }

        public async Task LoadAsync(int clientId)
        {
            var client = await _clientRepository.GetByIdAsync(clientId);
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
            await _clientRepository.SaveAsync();
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

       
    }
}
