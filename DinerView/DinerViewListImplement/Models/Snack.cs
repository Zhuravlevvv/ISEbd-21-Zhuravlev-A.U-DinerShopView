using System;
using System.Collections.Generic;
using System.Text;

namespace DinerViewListImplement.Models
{
    public class Snack
    {
        public int Id { get; set; }
        public string SnackName { get; set; }
        public decimal Price { get; set; }
        public Dictionary<int, int> SnackFoods { get; set; }
    }
}