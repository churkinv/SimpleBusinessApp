using System;
using System.Threading.Tasks;
using Prism.Events;
using SimpleBusinessApp.View.Services;
using SimpleBusinessApp.Data.Repository;
using System.Collections.ObjectModel;
using SimpleBusinessApp.Model;
using SimpleBusinessApp.Wrapper;
using System.ComponentModel;
using Prism.Commands;
using System.Linq;
using System.Windows.Input;

namespace SimpleBusinessApp.ViewModel
{
    class CompanyDetailViewModel : DetailViewModelBase
    {
        private ICompanyRepository _companyRepository;
        private CompanyWrapper _selectedCompany;
        public CompanyWrapper SelectedCompany
        {
            get { return _selectedCompany; }
            set
            {
                _selectedCompany = value;
                OnPropertyChanged();
                ((DelegateCommand)RemoveCommand).RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<CompanyWrapper> Companies { get; }
        public ICommand AddCommand { get; }
        public ICommand RemoveCommand { get; }

        public CompanyDetailViewModel(IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            ICompanyRepository companyRepository)
            : base(eventAggregator, messageDialogService)
        {
            _companyRepository = companyRepository;
            Title = "Company details";
            Companies = new ObservableCollection<CompanyWrapper>();

            AddCommand = new DelegateCommand(OnAddExecute);
            RemoveCommand = new DelegateCommand(OnRemoveExecuteAsync, OnRemoveCanExecute);
        }

        private void OnAddExecute()
        {
            var wrapper = new CompanyWrapper(new Company());
            wrapper.PropertyChanged += Wrapper_PropertyChanged;
            _companyRepository.Add(wrapper.Model);
            Companies.Add(wrapper);

            //trigger the validation
            wrapper.Name = "";
        }

        private async void OnRemoveExecuteAsync()
        {
            var isReferenced =
                await _companyRepository.IsReferenceByClientAsync(
                    SelectedCompany.Id);
            if (isReferenced)
            {
                await MessageDialogService.ShowInfoDialogAsync($"The company {SelectedCompany.Name} as it referenced by at least one Client ");
                return;
            }

            SelectedCompany.PropertyChanged -= Wrapper_PropertyChanged;
            _companyRepository.Remove(SelectedCompany.Model);
            Companies.Remove(SelectedCompany);
            SelectedCompany = null;
            HasChanges = _companyRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private bool OnRemoveCanExecute()
        {
            return SelectedCompany != null;
        }

        public override async Task LoadAsync(int id)
        {
            Id = id;
            foreach (var wrapper in Companies)
            {
                wrapper.PropertyChanged -= Wrapper_PropertyChanged;
            }

            Companies.Clear();

            var companies = await _companyRepository.GetAllAsync();

            foreach (var model in companies)
            {
                var wrapper = new CompanyWrapper(model);
                wrapper.PropertyChanged += Wrapper_PropertyChanged;
                Companies.Add(wrapper);
            }
        }

        private void Wrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _companyRepository.HasChanges();
            }

            if (e.PropertyName == nameof(CompanyWrapper.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        protected override void OnDeleteExecute()
        {
            throw new NotImplementedException();
        }

        protected override bool OnSaveCanExecute()
        {
            return HasChanges && Companies.All(c => !c.HasErrors);
        }

        protected async override void OnSaveExecute()
        {
            try
            {
                await _companyRepository.SaveAsync();
                HasChanges = _companyRepository.HasChanges();
                RaiseCollectionSavedEvent();
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                await MessageDialogService.ShowInfoDialogAsync("Error while saving the entities, " +
                    "the data be reloaded. Details: " + ex.Message);
                await LoadAsync(Id);
            }
           
        }
    }
}
