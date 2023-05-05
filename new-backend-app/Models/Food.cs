using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace new_backend.Models
{
    public class Food
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual long Id { get; set; }

        public virtual long UserId { get; set; }

        public virtual string Title { get; set; }

        public virtual double Quantity { get; set; }

        public virtual string Unit { get; set; }

        public virtual int Calories { get; set; }

        public virtual int Fats { get; set; }

        public virtual int Carbs { get; set; }

        public virtual int Proteins { get; set; }

        public Food()
        {
        }
    }
}