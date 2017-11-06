using SimpleBusinessApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBusinessApp.Wrapper
{
    public class ClientPhoneNumberWrapper : ModelWrapper<ClientPhoneNumber>
    {
        public ClientPhoneNumberWrapper(ClientPhoneNumber model) : base( model)
        {
        }

        public string Number
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
    }
}
