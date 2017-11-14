using System.ComponentModel.DataAnnotations;

namespace SimpleBusinessApp.Model
{
    public class Company
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
                
        [StringLength(50)]
        public string OwnershipType { get; set; } //TODO: replace string in the class with the enum or class?
                
        [StringLength(50)]
        public string CountryOfRegistration { get; set; }

        [StringLength(50)]
        public string HeadQuarter { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
