using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace new_backend.Models
{
    public class Food
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual long Id { get; set; }

        public virtual long UserId { get; set; }

        [Required]
        public virtual string Title { get; set; }

        [Required]
        public virtual double Quantity { get; set; }

        [Required]
        public virtual string Unit { get; set; }

        [Required]
        public virtual int Calories { get; set; }

        [Required]
        public virtual int Fats { get; set; }

        [Required]
        public virtual int Carbs { get; set; }

        [Required]
        public virtual int Proteins { get; set; }

        public Food()
        {
        }
    }
}