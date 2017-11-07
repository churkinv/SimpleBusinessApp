using Prism.Commands;
using System.Windows.Input;
using System;
using SimpleBusinessApp.Event;
using Prism.Events;

namespace SimpleBusinessApp.ViewModel
{
    public class NavigationItemViewModel : ViewModelBase
    {
        private string _displayMember;
        private IEventAggregator _eventAggregator;
        private string _detailViewModelName;

        public int Id { get; }
        public ICommand OpenDetailViewCommand { get; }
        public string DisplayMember
        {
            get { return _displayMember; }
            set
            {
                _displayMember = value;
                OnPropertyChanged();
            }
        }       

        public NavigationItemViewModel(int id, string displayMember, string detailViewModelName,
            IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            Id = id;
            DisplayMember = displayMember;
            _detailViewModelName = detailViewModelName;
            OpenDetailViewCommand = new DelegateCommand(OnOpenDetailViewExecute);
           
        }

        private void OnOpenDetailViewExecute()
        {
            _eventAggregator.GetEvent<OpenDetailViewEvent>()
                        .Publish(
                
                new OpenDetailViewEventArgs
                {
                    Id=Id,
                    ViewModelName = _detailViewModelName
                });
        }

    }
}
