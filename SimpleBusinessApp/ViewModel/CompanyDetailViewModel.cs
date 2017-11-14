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

namespace SimpleBusinessApp.ViewModel
{
    class CompanyDetailViewModel : DetailViewModelBase
    {
        private ICompanyRepository _companyRepository;
        public ObservableCollection<CompanyWrapper> Companies { get; }


        public CompanyDetailViewModel(IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            ICompanyRepository companyRepository) 
            : base(eventAggregator, messageDialogService)
        {
            _companyRepository = companyRepository;
            Title = "Company details";
            Companies = new ObservableCollection<CompanyWrapper>();
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
            await _companyRepository.SaveAsync();
            HasChanges = _companyRepository.HasChanges();
            RaiseCollectionSavedEvent();
        }
    }
}
