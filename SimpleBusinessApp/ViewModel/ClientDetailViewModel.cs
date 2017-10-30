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
        private bool _hasChanges;

        public ClientWrapper Client
        {
            get { return _client; }
            private set
            {
                _client = value;
                OnPropertyChanged();
            }
        }
        public bool HasChanges
        {
            get { return _hasChanges; }
            set
            {
                if (_hasChanges != value) // to check if changes really took place
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
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
                  if (!HasChanges)
                  {
                      HasChanges = _clientRepository.HasChanges(); // to be sure that we don`t call has changes method everytime, we only want to call if HasChanges is not true yet
                  }
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
            HasChanges = _clientRepository.HasChanges();
            _eventAggregator.GetEvent<AfterClientSaveEvent>().Publish(
                new AfterClientSaveEventArgs
                {
                    Id = Client.Id,
                    DisplayMember = $"{Client.FirstName} {Client.LastName}"
                });
        }

        private bool OnSaveCanExecute()
        {
            return Client != null && !Client.HasErrors && HasChanges;
        }


    }
}
