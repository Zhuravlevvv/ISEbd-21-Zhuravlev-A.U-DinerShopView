using System;
using System.Collections.Generic;
using System.Text;

namespace DinerBusinessLogic.ViewModels
{
    public class ReportOrderByDateViewModel
    {
        public DateTime Date { get; set; }

        public int Count { get; set; }

        public decimal Sum { get; set; }
    }
}
