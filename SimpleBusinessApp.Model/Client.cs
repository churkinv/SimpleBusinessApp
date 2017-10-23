using System.ComponentModel.DataAnnotations;

namespace SimpleBusinessApp.Model
{
    public class Client
    {
        //[Key] EF has a convention for Id property it will set is as Key automatically so no need attribute
        public int Id { get; set; }

        [Required]
        [MaxLength (50)] // this attribute is used for string or arrays, if I only want for strings there is StringLength attribute
        public string FirstName { get; set; }

        [StringLength (50)]
        public string LastName { get; set; }

        [StringLength (50)]
        public string Email { get; set; }
    }
}
