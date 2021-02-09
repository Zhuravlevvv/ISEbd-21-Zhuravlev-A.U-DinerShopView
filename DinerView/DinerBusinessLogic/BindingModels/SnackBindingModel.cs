using System;
using System.Collections.Generic;
using System.Text;

namespace DinerBusinessLogic.BindingModels
{
    //закуска
    public class SnackBindingModel
    {
        public int? Id { get; set; }
        public string SnackName { get; set; }
        public decimal Price { get; set; }
        public Dictionary<int, (string, int)> SnackFoods { get; set; }
    }
}
