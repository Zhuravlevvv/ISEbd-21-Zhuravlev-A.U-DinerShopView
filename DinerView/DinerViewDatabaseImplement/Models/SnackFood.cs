using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DinerViewDatabaseImplement.Models
{
    public class SnackFood
    {
        public int Id { get; set; }
        public int SnackId { get; set; }
        public int FoodId { get; set; }
        [Required]
        public int Count { get; set; }
        public virtual Food Food { get; set; }
        public virtual Snack Snack { get; set; }
    }
}
