using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AnimalChip.Models
{
    public class Animal
    {
        
        public int Id { get; set; }
        [StringLength(60, MinimumLength = 3)]
        [RegularExpression("^[A-Z]+[a-zA-ZąćęłńóśźżĄĘŁŃÓŚŹŻ]{3,60}", ErrorMessage = "Name cannot contain special characters and must start with a capital letter"), Required]
        public string Name { get; set; }
        public string Kind { get; set; }
        [MaxLength(60)]
        [MinLength(3)]
        [RegularExpression("[ a-zA-ZąćęłńóśźżĄĘŁŃÓŚŹŻ]{3,60}", ErrorMessage = "Breed cannot contain special characters"), Required]
        public string Breed { get; set; }

        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid Phone Number")]
        [MinLength(6, ErrorMessage = "The chip must be 6 to 8 digits long")]
        [MaxLength(8, ErrorMessage = "The chip must be 6 to 8 digits long")]
        [RegularExpression("[0-9]{6,8}", ErrorMessage = "Chip must be a number between 6 and 8 digits long")]
        public string Chip { get; set; }

        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0: dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Modified Date")]
        [DisplayFormat(DataFormatString = "{0: dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ModifiedTime { get; set; }

        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid Phone Number")]
        [RegularExpression("[0-9]{9}", ErrorMessage = "The number must consist of 9 digits")]
        [StringLength(9)]
        [Phone]
        public string Contact { get; set; }
        public string Email { get; set; }


        public override string ToString() => JsonSerializer.Serialize<Animal>(this);
        public Animal()
        {
        }

    }
}
