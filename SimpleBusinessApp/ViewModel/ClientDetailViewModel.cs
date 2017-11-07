using Prism.Commands;
using Prism.Events;
using SimpleBusinessApp.Data.Repositories;
using SimpleBusinessApp.Event;
using SimpleBusinessApp.Wrapper;
using System.Threading.Tasks;
using System.Windows.Input;
using System;
using SimpleBusinessApp.Model;
using SimpleBusinessApp.View.Services;
using SimpleBusinessApp.Data.Lookups;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SimpleBusinessApp.ViewModel
{
    /// <summary>
    /// Loads a single Client
    /// </summary>
    public class ClientDetailViewModel : DetailViewModelBase, IClientDetailViewModel
    {
        private IClientRepository _clientRepository;

        private IMessageDialogService _messageDialogService;
        private ICompanyLookupDataService _companyLookupDataService;
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

        private ClientPhoneNumberWrapper _selectedPhoneNumber;

        public ClientPhoneNumberWrapper SelectedPhoneNumber
        {
            get { return _selectedPhoneNumber; }
            set
            {
                _selectedPhoneNumber = value;
                OnPropertyChanged();
                ((DelegateCommand)RemovePhoneNumberCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand AddPhoneNumberCommand { get; }

        public ICommand RemovePhoneNumberCommand { get; }

        public ObservableCollection<LookupItem> Companies { get; }
        public ObservableCollection<ClientPhoneNumberWrapper> PhoneNumbers { get; }

        public ClientDetailViewModel(IClientRepository clientRepository,
            IEventAggregator eventAggregator, IMessageDialogService messageDialogService,
            ICompanyLookupDataService companyLookupDataService) : base(eventAggregator)
        {
            _clientRepository = clientRepository;
            _messageDialogService = messageDialogService;
            _companyLookupDataService = companyLookupDataService;

            AddPhoneNumberCommand = new DelegateCommand(OnAddPhoneNumberExecute);
            RemovePhoneNumberCommand = new DelegateCommand(OnRemovePhoneNumberExecute, OnRemovePhoneNumberCanExecute);

            Companies = new ObservableCollection<LookupItem>();
            PhoneNumbers = new ObservableCollection<ClientPhoneNumberWrapper>();
        }


        public override async Task LoadAsync(int? clientId)
        {
            var client = clientId.HasValue
                ? await _clientRepository.GetByIdAsync(clientId.Value)
                : CreateNewClient();

            InitializeClient(client);

            InitializeClientPhoneNumbers(client.PhoneNumbers);

            await LoadCompaniesAsync();

        }

        private void InitializeClientPhoneNumbers(ICollection<ClientPhoneNumber> phoneNumbers)
        {
            foreach (var wrapper in PhoneNumbers)
            {
                wrapper.PropertyChanged -= ClientPhoneNumberWrapper_PropertyChanged;
            }
            PhoneNumbers.Clear();

            foreach (var clientPhoneNumber in phoneNumbers)
            {
                var wrapper = new ClientPhoneNumberWrapper(clientPhoneNumber);
                PhoneNumbers.Add(wrapper);
                wrapper.PropertyChanged += ClientPhoneNumberWrapper_PropertyChanged;
            }

        }

        private void ClientPhoneNumberWrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _clientRepository.HasChanges();
            }
            if (e.PropertyName == nameof(ClientPhoneNumberWrapper.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private void InitializeClient(Client client)
        {
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

            if (Client.Id == 0)
            {
                // manipulation to trigger the validation
                Client.FirstName = "";
            }
        }

        private async Task LoadCompaniesAsync()
        {
            Companies.Clear();
            Companies.Add(new NullLookupItem { DisplayMember = " - " }); // to have possibility to display Null (or specific sign) in our combobox
            var lookup = await _companyLookupDataService.GetCompanyLookupAsync();

            foreach (var lookupItem in lookup)
            {
                Companies.Add(lookupItem);
            }
        }

        private Client CreateNewClient()
        {
            var client = new Client();
            _clientRepository.Add(client);

            return client;
        }

        protected override async void OnSaveExecute()
        {
            await _clientRepository.SaveAsync();
            HasChanges = _clientRepository.HasChanges();
            RaiseDetailSavedEvent(Client.Id, $"{Client.FirstName} {Client.LastName}");
        }

        protected override bool OnSaveCanExecute()
        {
            return Client != null
                && !Client.HasErrors
                && PhoneNumbers.All(pn => !pn.HasErrors)
                && HasChanges;
        }

        protected override async void OnDeleteExecute()
        {
            var result = _messageDialogService.ShowOkCancelDialog("Do you really want to delete the Client?",
                "Question");
            if (result == MessageDialogResult.Ok)
            {
                _clientRepository.Remove(Client.Model);
                await _clientRepository.SaveAsync();
                RaiseDetailDeletedEvent(Client.Id);
            }
        }

        private void OnAddPhoneNumberExecute()
        {
            var newNumber = new ClientPhoneNumberWrapper(new ClientPhoneNumber());
            newNumber.PropertyChanged += ClientPhoneNumberWrapper_PropertyChanged;
            PhoneNumbers.Add(newNumber);
            Client.Model.PhoneNumbers.Add(newNumber.Model);
            newNumber.Number = ""; //Trigger validation;
        }

        private void OnRemovePhoneNumberExecute()
        {
            SelectedPhoneNumber.PropertyChanged -= ClientPhoneNumberWrapper_PropertyChanged;
            _clientRepository.RemovePhoneNumber(SelectedPhoneNumber.Model);
            Client.Model.PhoneNumbers.Remove(SelectedPhoneNumber.Model);
            PhoneNumbers.Remove(SelectedPhoneNumber);
            SelectedPhoneNumber = null;
            HasChanges = _clientRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private bool OnRemovePhoneNumberCanExecute()
        {
            return SelectedPhoneNumber != null;
        }
    }
}
