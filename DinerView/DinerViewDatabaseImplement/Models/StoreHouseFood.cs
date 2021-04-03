using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace DinerViewDatabaseImplement.Models
{
    public class StoreHouseFood
    {
        public int Id { get; set; }

        public int StoreHouseId { get; set; }

        public int FoodId { get; set; }

        [Required]
        public int Count { get; set; }

        public virtual Food Food { get; set; }

        public virtual StoreHouse StoreHouse { get; set; }
    }
}
