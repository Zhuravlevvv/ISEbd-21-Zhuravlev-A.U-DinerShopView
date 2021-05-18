using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using DinerBusinessLogic.Attributes;

namespace DinerBusinessLogic.ViewModels
{
    //Продукт, требуемый для изготовления закуски
    public class FoodViewModel
    {
        [Column(title: "Номер", width: 100, visible: false)]
        public int Id { get; set; }
        [Column(title: "Название компонента", gridViewAutoSize: GridViewAutoSize.Fill)]
        public string FoodName { get; set; }
    }
}
