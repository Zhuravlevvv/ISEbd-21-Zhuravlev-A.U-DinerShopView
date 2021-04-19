using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace DinerBusinessLogic.ViewModels
{
    public class ReportStoreHouseViewModel
    {
        public int Id { get; set; }

        [DisplayName("Название закуски")]
        public string SnackName { get; set; }

        [DisplayName("Цена")]
        public decimal Price { get; set; }

        public Dictionary<int, (string, int)> FoodSnacks { get; set; }
    }
}
