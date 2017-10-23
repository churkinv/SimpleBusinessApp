using SimpleBusinessApp.Data;
using SimpleBusinessApp.Model;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SimpleBusinessApp.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public INavigationViewModel NavigationViewModel { get; }
        public IClientDetailViewModel ClientDetailViewModel { get; }

        #region before restructuring
        //private Client _selectedClient;
        //private IClientDataService _clientDataService;
        //public ObservableCollection<Client> Clients { get; set; } // then bind it to listView
        #endregion

        public MainViewModel(/*IClientDataService clientDataService*/ INavigationViewModel navigationViewModel, IClientDetailViewModel clientDetailViewModel) //replaced
        {
            NavigationViewModel = navigationViewModel;
            ClientDetailViewModel = clientDetailViewModel;
            #region before restructuring
            //Clients = new ObservableCollection<Client> (); commented after refactoring
            //_clientDataService = clientDataService;
            #endregion

        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();

            #region before restructuring
            //var clients = await _clientDataService.GetAllAsync();
            //Clients.Clear(); // to be sure that load method can be called many times
            //foreach (var client in clients)
            //{
            //    Clients.Add(client);
            //}
            #endregion
        }

        #region before restructuring
        //public Client SelectedClient
        //{
        //    get { return _selectedClient ; }
        //    set
        //    {
        //        _selectedClient = value;
        //        OnPropertyChanged(/*nameof(SelectedClient)*/); // property will be passed automatically by the compiler
        //    }
        //}
        #endregion

    }

}

