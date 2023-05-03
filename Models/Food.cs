using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace new_backend.Models
{
    public class Food
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long UserId { get; set; }

        public string Title { get; set; }

        public double Quantity { get; set; }

        public string Unit { get; set; }

        public int Calories { get; set; }

        public int Fats { get; set; }

        public int Carbs { get; set; }

        public int Proteins { get; set; }
    }
}