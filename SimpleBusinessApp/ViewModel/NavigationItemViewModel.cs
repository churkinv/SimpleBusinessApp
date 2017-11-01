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

        public int Id { get; }
        public ICommand OpenClientDetailViewCommand { get; }
        public string DisplayMember
        {
            get { return _displayMember; }
            set
            {
                _displayMember = value;
                OnPropertyChanged();
            }
        }

        public NavigationItemViewModel(int id, string displayMember,
            IEventAggregator eventAggregator)
        {
            Id = id;
            DisplayMember = displayMember;
            OpenClientDetailViewCommand = new DelegateCommand(OnOpenClientView);
            _eventAggregator = eventAggregator;
        }

        private void OnOpenClientView()
        {
            _eventAggregator.GetEvent<OpenClientDetailViewEvent>()
                        .Publish(Id);
        }

    }
}
