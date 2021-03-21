using System.Collections.Generic;

namespace DinerViewFileImplement.Models
{
    public class Snack
    {
        public int Id { get; set; }
        public string SnackName { get; set; }
        public decimal Price { get; set; }
        public Dictionary<int, int> SnackFoods { get; set; }
    }
}
