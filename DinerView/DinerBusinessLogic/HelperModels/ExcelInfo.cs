using System;
using System.Collections.Generic;
using System.Text;
using DinerBusinessLogic.ViewModels;

namespace DinerBusinessLogic.HelperModels
{
    class ExcelInfo
    {
        public string FileName { get; set; }

        public string Title { get; set; }

        public List<ReportFoodSnackViewModel> FoodSnacks { get; set; }
    }
}
