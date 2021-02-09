using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace DinerBusinessLogic.ViewModels
{
    //Продукт, требуемый для изготовления закуски
    public class FoodViewModel
    {
        public int Id { get; set; }
        [DisplayName("Название продукта")]
        public string FoodName { get; set; }
    }
}