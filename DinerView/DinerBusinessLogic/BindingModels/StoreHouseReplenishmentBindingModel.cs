using System;
using System.Collections.Generic;
using System.Text;

namespace DinerBusinessLogic.BindingModels
{
    public class StoreHouseReplenishmentBindingModel
    {
        public int FoodId { get; set; }
        public int StoreHouseId { get; set; }
        public int Count { get; set; }
    }
}
