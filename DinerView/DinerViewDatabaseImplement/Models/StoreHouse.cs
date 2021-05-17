using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DinerViewDatabaseImplement.Models
{
    public class StoreHouse
    {
        public int Id { get; set; }
        [Required]
        public string StoreHouseName { get; set; }
        [Required]
        public string ResponsiblePersonFCS { get; set; }
        [Required]
        public DateTime DateCreate { get; set; }
        [ForeignKey("StoreHouseId")]
        public virtual List<StoreHouseFood> StoreHouseFoods { get; set; }
    }
}
