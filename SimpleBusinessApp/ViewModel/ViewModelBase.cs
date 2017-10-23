using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SimpleBusinessApp.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null) // with these two parametrs null and CallerMember.. i can call event without paremeter. Compiler will automatically passed the property name it is callsed from
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
