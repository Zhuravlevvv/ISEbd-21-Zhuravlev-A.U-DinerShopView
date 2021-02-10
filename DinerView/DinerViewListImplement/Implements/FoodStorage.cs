using System;
using System.Collections.Generic;
using DinerBusinessLogic.BindingModels;
using DinerBusinessLogic.Interfaces;
using DinerBusinessLogic.ViewModels;
using DinerViewListImplement.Models;

namespace DinerViewListImplement.Implements
{
    public class FoodStorage : IFoodStorage
    {
        private readonly DataListSingleton source;
        public FoodStorage()
        {
            source = DataListSingleton.GetInstance();
        }
        public List<FoodViewModel> GetFullList()
        {
            List<FoodViewModel> result = new List<FoodViewModel>();
            foreach (var food in source.Foods)
            {
                result.Add(CreateModel(food));
            }
            return result;
        }
        public List<FoodViewModel> GetFilteredList(FoodBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            List<FoodViewModel> result = new List<FoodViewModel>();
            foreach (var food in source.Foods)
            {
                if (food.FoodName.Contains(model.FoodName))
                {
                    result.Add(CreateModel(food));
                }
            }
            return result;
        }
        public FoodViewModel GetElement(FoodBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            foreach (var food in source.Foods)
            {
                if (food.Id == model.Id || food.FoodName ==
               model.FoodName)
                {
                    return CreateModel(food);
                }
            }
            return null;
        }
        public void Insert(FoodBindingModel model)
        {
            Food tempFood = new Food { Id = 1 };
            foreach (var food in source.Foods)
            {
                if (food.Id >= tempFood.Id)
                {
                    tempFood.Id = food.Id + 1;
                }
            }
            source.Foods.Add(CreateModel(model, tempFood));
        }
        public void Update(FoodBindingModel model)
        {
            Food tempFood = null;
            foreach (var food in source.Foods)
            {
                if (food.Id == model.Id)
                {
                    tempFood = food;
                }
            }
            if (tempFood == null)
            {
                throw new Exception("Элемент не найден");
            }
            CreateModel(model, tempFood);
        }
        public void Delete(FoodBindingModel model)
        {
            for (int i = 0; i < source.Foods.Count; ++i)
            {
                if (source.Foods[i].Id == model.Id.Value)
                {
                    source.Foods.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
        private Food CreateModel(FoodBindingModel model, Food food)
        {
            food.FoodName = model.FoodName;
            return food;
        }
        private FoodViewModel CreateModel(Food food)
        {
            return new FoodViewModel
            {
                Id = food.Id,
                FoodName = food.FoodName
            };
        }
    }
}
