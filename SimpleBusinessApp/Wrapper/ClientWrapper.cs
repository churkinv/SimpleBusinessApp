using SimpleBusinessApp.Model;
using SimpleBusinessApp.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Collections;

namespace SimpleBusinessApp.Wrapper
{

    /// <summary>
    /// This class is a wrapper of Client class and is used in ClientDetailViewModel. We created it to use INotifyDataErrorInfo for input validation.
    /// </summary>
    public class ClientWrapper : ViewModelBase, INotifyDataErrorInfo
    {
        public ClientWrapper(Client model)
        {
            Model = model;
        }

        public Client Model { get; }

        public int Id { get { return Model.Id; } }

        public string FirstName
        {
            get { return Model.FirstName; }
            set
            {
                Model.FirstName = value;
                OnPropertyChanged();
                ValidateProperty(nameof(FirstName));
            }
        }

        private void ValidateProperty(string propertyName)
        {
            ClearErrors(propertyName);
            switch (propertyName)
            {
                case nameof(FirstName):
                    if (string.Equals(FirstName, "Batman", StringComparison.OrdinalIgnoreCase)) // just example, enter Batman as FirstName to txtbox, and now when we change the name to Batamn, out txtbox will have red frame
                    {
                        AddError(propertyName, "Batman is not valid"); // 
                    }
                    break;

            }
        }

        public string LastName
        {
            get { return Model.LastName; }
            set
            {
                Model.LastName = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get { return Model.Email; }
            set
            {
                Model.Email = value;
                OnPropertyChanged();
            }
        }

        private Dictionary<string, List<string>> _errorsByPropertyName
            = new Dictionary<string, List<string>>();

        public bool HasErrors => _errorsByPropertyName.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorsByPropertyName.ContainsKey(propertyName)
                ? _errorsByPropertyName[propertyName]
                : null;
        }

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        private void AddError(string propertyName, string error)  // this method is not a part of INotifyDataErrorInfo, and created to simplify
        {
            if (!_errorsByPropertyName.ContainsKey(propertyName))
            {
                _errorsByPropertyName[propertyName] = new List<string>();
            }
            if (!_errorsByPropertyName[propertyName].Contains(error))
            {
                _errorsByPropertyName[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        private void ClearErrors(string propertyName)  // this method is not a part of INotifyDataErrorInfo, and created to simplify
        {
            if (_errorsByPropertyName.ContainsKey(propertyName)) 
            {
                _errorsByPropertyName.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }
    }
}
