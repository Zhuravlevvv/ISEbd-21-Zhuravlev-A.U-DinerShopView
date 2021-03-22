using System;
using System.Collections.Generic;
using System.Text;

namespace DinerBusinessLogic.ViewModels
{
    public class ReportFoodSnackViewModel
    {
        public string SnackName { get; set; }

        public int TotalCount { get; set; }

        public List<Tuple<string, int>> Foods { get; set; } // FoodName/Count
    }
}
