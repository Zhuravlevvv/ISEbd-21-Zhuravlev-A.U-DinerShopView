using System;
using System.Collections.Generic;
using System.Text;
using DinerBusinessLogic.ViewModels;
using DinerBusinessLogic.BindingModels;

namespace DinerBusinessLogic.Interfaces
{
    public interface IFoodStorage
    {
        List<FoodViewModel> GetFullList();
        List<FoodViewModel> GetFilteredList(FoodBindingModel model);
        FoodViewModel GetElement(FoodBindingModel model);
        void Insert(FoodBindingModel model);
        void Update(FoodBindingModel model);
        void Delete(FoodBindingModel model);
    }
}