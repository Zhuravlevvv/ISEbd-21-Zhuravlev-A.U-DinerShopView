using System;
using System.Collections.Generic;
using System.Linq;
using DinerViewFileImplement.Models;
using DinerBusinessLogic.BindingModels;
using DinerBusinessLogic.Interfaces;
using DinerBusinessLogic.ViewModels;

namespace DinerViewFileImplement.Implements
{
    public class FoodStorage : IFoodStorage
    {
        private readonly FileDataListSingleton sourse;

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
        public FoodStorage()
        {
            sourse = FileDataListSingleton.GetInstance();
        }
        public List<FoodViewModel> GetFullList()
        {
            return sourse.Foods.Select(CreateModel).ToList();
        }
        public List<FoodViewModel> GetFilteredList(FoodBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            return sourse.Foods.Where(fd => fd.FoodName.Contains(model.FoodName)).Select(CreateModel).ToList();
        }
        public FoodViewModel GetElement(FoodBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            var food = sourse.Foods.FirstOrDefault(fd => fd.FoodName == model.FoodName || fd.Id == model.Id);
            return food != null ? CreateModel(food) : null;
        }
        public void Insert(FoodBindingModel model)
        {
            int maxId = sourse.Foods.Count > 0 ? sourse.Foods.Max(fd => fd.Id) : 0;
            var food = new Food { Id = maxId + 1 };
            sourse.Foods.Add(CreateModel(model, food));
        }
        public void Update(FoodBindingModel model)
        {
            var food = sourse.Foods.FirstOrDefault(fd => fd.Id == fd.Id);
            if (food == null)
            {
                throw new Exception("Продукт не найден");
            }
            CreateModel(model, food);
        }
        public void Delete(FoodBindingModel model)
        {
            var food = sourse.Foods.FirstOrDefault(comp => comp.Id == model.Id);
            if (food != null)
            {
                sourse.Foods.Remove(food);
            }
            else
            {
                throw new Exception("Продукт не найден");
            }
        }
    }
}
