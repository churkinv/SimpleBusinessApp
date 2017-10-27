using SimpleBusinessApp.Model;
using System;

namespace SimpleBusinessApp.Wrapper
{

    /// <summary>
    /// This class is a wrapper of Client class and is used in ClientDetailViewModel. 
    ///  We created it to use INotifyDataErrorInfo for input validation (inherited from ModelWrapper).
    /// </summary>
    public class ClientWrapper : ModelWrapper<Client>
    {
        public ClientWrapper(Client model) : base(model)
        {
        }

        public int Id { get { return Model.Id; } } 

        public string FirstName
        {
            get { return GetValue<string>(); } 
            set
            {
                SetValue(value);
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
            get { return  GetValue<string>(); }
            set { SetValue(value); }
        }

        public string Email
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }        
    }
}
