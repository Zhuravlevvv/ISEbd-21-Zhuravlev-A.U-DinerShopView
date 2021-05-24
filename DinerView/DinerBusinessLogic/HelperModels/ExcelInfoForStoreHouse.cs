using System;
using System.Collections.Generic;
using System.Text;
using DinerBusinessLogic.ViewModels;

namespace DinerBusinessLogic.HelperModels
{
    public class ExcelInfoForStoreHouse
    {
        public string FileName { get; set; }

        public string Title { get; set; }

        public List<ReportStoreHouseFoodsViewModel> StoreHouseFoods { get; set; }
    }
}
