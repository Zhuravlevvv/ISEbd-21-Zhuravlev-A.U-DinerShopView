using System;
using System.Collections.Generic;
using DinerBusinessLogic.BindingModels;
using DinerBusinessLogic.Interfaces;
using DinerBusinessLogic.ViewModels;

namespace DinerBusinessLogic.BusinessLogics
{
    public class FoodLogic
    {
        private readonly IFoodStorage _foodStorage;
        public FoodLogic(IFoodStorage foodStorage)
        {
            _foodStorage = foodStorage;
        }
        public List<FoodViewModel> Read(FoodBindingModel model)
        {
            if (model == null)
            {
                return _foodStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<FoodViewModel> { _foodStorage.GetElement(model) };
            }
            return _foodStorage.GetFilteredList(model);
        }
        public void CreateOrUpdate(FoodBindingModel model)
        {
            var element = _foodStorage.GetElement(new FoodBindingModel { 
            FoodName = model.FoodName });
            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Уже есть продукт с таким названием!");
            }
            if (model.Id.HasValue)
            {
                _foodStorage.Update(model);
            }
            else
            {
                _foodStorage.Insert(model);
            }
        }
        public void Delete(FoodBindingModel model)
        {
            var element = _foodStorage.GetElement(new FoodBindingModel { Id =
            model.Id });
            if (element == null)
            {
                throw new Exception("Продукт не найден!");
            }
            _foodStorage.Delete(model);
        }
    }
}