using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DinerBusinessLogic.ViewModels
{
    public class StoreHouseViewModel
    {
        public int Id { get; set; }
        [DisplayName("Название")]
        public string StoreHouseName { get; set; }
        [DisplayName("ФИО ответственного")]
        public string ResponsiblePersonFCS { get; set; }
        [DisplayName("Дата создания")]
        public DateTime DateCreate { get; set; }
        public Dictionary<int, (string, int)> StoreHouseFoods;
    }
}
