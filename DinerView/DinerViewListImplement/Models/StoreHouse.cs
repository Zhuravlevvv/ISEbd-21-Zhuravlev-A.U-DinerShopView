using System;
using System.Collections.Generic;
using System.Text;

namespace DinerViewListImplement.Models
{
    public class StoreHouse
    {
        public int Id { get; set; }

        public string StoreHouseName { get; set; }

        public string ResponsiblePersonFCS { get; set; }

        public DateTime DateCreate { get; set; }

        public Dictionary<int, int> StoreHouseFoods { get; set; }
    }
}
