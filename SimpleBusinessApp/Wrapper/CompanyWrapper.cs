using SimpleBusinessApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBusinessApp.Wrapper
{
    public class CompanyWrapper : ModelWrapper<Company>
    {
        public int Id { get { return Model.Id; } }
        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string OwnershipType
        {
            get { return Model.OwnershipType; }
            set { SetValue(value); }
        }

        public string CountryOfRegistration
        {
            get { return Model.CountryOfRegistration; }
            set { SetValue(value); }
        }

        public string HeadQuarter
        {
            get { return Model.HeadQuarter; }
            set { SetValue(value); }
        }

        public CompanyWrapper(Company model) : base(model)
        {
        }
    }
}
