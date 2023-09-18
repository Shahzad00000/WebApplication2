using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{

    public class Laptop
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string FatherName { get; set; }
        [Required]
        public string Path { get; set; }
        [NotMapped]
        [Display(Name = "Choose Name")]
        public IFormFile FormFile { get; set; }
        public bool IsActive { get; set; }

    }
}
