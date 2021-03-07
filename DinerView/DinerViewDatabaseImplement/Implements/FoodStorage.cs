using System;
using System.Collections.Generic;
using System.Linq;
using DinerBusinessLogic.BindingModels;
using DinerBusinessLogic.Interfaces;
using DinerBusinessLogic.ViewModels;
using DinerViewDatabaseImplement.Models;

namespace DinerViewDatabaseImplement.Implements
{
    public class FoodStorage : IFoodStorage
    {
        public List<FoodViewModel> GetFullList()
        {
            using (var context = new DinerViewDatabase())
            {
                return context.Foods.Select(rec => new FoodViewModel { Id = rec.Id, FoodName = rec.FoodName }).ToList();
            }
        }
        public List<FoodViewModel> GetFilteredList(FoodBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new DinerViewDatabase())
            {
                return context.Foods.Where(rec => rec.FoodName.Contains(model.FoodName)).Select(rec => new FoodViewModel
                { Id = rec.Id, FoodName = rec.FoodName }).ToList();
            }
        }
        public FoodViewModel GetElement(FoodBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new DinerViewDatabase())
            {
                var food = context.Foods.FirstOrDefault(rec => rec.FoodName == model.FoodName || rec.Id == model.Id);

                return food != null ? new FoodViewModel
                {
                    Id = food.Id,
                    FoodName = food.FoodName
                } : null;
            }
        }
        public void Insert(FoodBindingModel model)
        {
            using (var context = new DinerViewDatabase())
            {
                context.Foods.Add(CreateModel(model, new Food()));
                context.SaveChanges();
            }
        }
        public void Update(FoodBindingModel model)
        {
            using (var context = new DinerViewDatabase())
            {
                var element = context.Foods.FirstOrDefault(rec => rec.Id ==
               model.Id);
                if (element == null)
                {
                    throw new Exception("Элемент не найден");
                }
                CreateModel(model, element);
                context.SaveChanges();
            }
        }
        public void Delete(FoodBindingModel model)
        {
            using (var context = new DinerViewDatabase())
            {
                Food element = context.Foods.FirstOrDefault(rec => rec.Id ==
               model.Id);
                if (element != null)
                {
                    context.Foods.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }
        private Food CreateModel(FoodBindingModel model, Food food)
        {
            food.FoodName = model.FoodName;
            return food;
        }
    }
}
