using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace SimpleBusinessApp.Model
{
    /// <summary>
    /// We use DataAnnotations which is used by 
    /// Code First EF migrations to create DB
    /// </summary>
    public class Client
    {
        public Client()
        {
            PhoneNumbers = new Collection<ClientPhoneNumber>();
            Meetings = new Collection<Meeting>();
        }

        //[Key] EF has a convention for Id property it will set is as Key automatically so no need attribute
        public int Id { get; set; }

        [Required]
        [MaxLength (50)] // this attribute is used for string or arrays, if I only want for strings there is StringLength attribute
        public string FirstName { get; set; }

        [StringLength (50)]
        public string LastName { get; set; }

        [StringLength (50)]
        [EmailAddress] // this annotations allow us to check if the property contains a valid email address
        public string Email { get; set; }

        public int? CompanyId { get; set; }

        public Company CompanyIsWorkingFor { get; set; }

        public ICollection<ClientPhoneNumber> PhoneNumbers { get; set; }

        public ICollection<Meeting> Meetings { get; set; }

    }
}
