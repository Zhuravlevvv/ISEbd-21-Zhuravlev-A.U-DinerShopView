using System;
using System.Collections.Generic;
using System.Linq;
using DinerViewFileImplement.Models;
using DinerBusinessLogic.BindingModels;
using DinerBusinessLogic.Interfaces;
using DinerBusinessLogic.ViewModels;

namespace DinerViewFileImplement.Implements
{
    public class SnackStorage : ISnackStorage
    {
        private readonly FileDataListSingleton source;
        private Snack CreateModel(SnackBindingModel model, Snack snack)
        {
            snack.SnackName = model.SnackName;
            snack.Price = model.Price;
            foreach (var key in snack.SnackFoods.Keys.ToList())
            {
                if (!model.SnackFoods.ContainsKey(key))
                {
                    snack.SnackFoods.Remove(key);
                }
            }
            foreach (var food in model.SnackFoods)
            {
                if (snack.SnackFoods.ContainsKey(food.Key))
                {
                    snack.SnackFoods[food.Key] = model.SnackFoods[food.Key].Item2;
                }
                else
                {
                    snack.SnackFoods.Add(food.Key, model.SnackFoods[food.Key].Item2);
                }
            }
            return snack;
        }
        private SnackViewModel CreateModel(Snack snack)
        {
            return new SnackViewModel
            {
                Id = snack.Id,
                SnackName = snack.SnackName,
                Price = snack.Price,
                SnackFoods = snack.SnackFoods.ToDictionary(snackFood => snackFood.Key, snackFood =>
                (source.Foods.FirstOrDefault(food => food.Id == snackFood.Key)?.FoodName, snackFood.Value))
            };
        }
        public SnackStorage()
        {
            source = FileDataListSingleton.GetInstance();
        }
        public List<SnackViewModel> GetFullList()
        {
            return source.Snacks.Select(CreateModel).ToList();
        }
        public List<SnackViewModel> GetFilteredList(SnackBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            return source.Snacks.Where(recPizza => recPizza.SnackName.Contains(model.SnackName)).Select(CreateModel).ToList();
        }
        public SnackViewModel GetElement(SnackBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            var snack = source.Snacks.FirstOrDefault(recSnack => recSnack.SnackName == model.SnackName || recSnack.Id == model.Id);
            return snack != null ? CreateModel(snack) : null;
        }
        public void Insert(SnackBindingModel model)
        {
            int maxId = source.Snacks.Count > 0 ? source.Snacks.Max(recSnack => recSnack.Id) : 0;
            var snack = new Snack { Id = maxId + 1, SnackFoods = new Dictionary<int, int>() };
            source.Snacks.Add(CreateModel(model, snack));
        }
        public void Update(SnackBindingModel model)
        {
            var snack = source.Snacks.FirstOrDefault(recSnack => recSnack.Id == model.Id);

            if (snack == null)
            {
                throw new Exception("Пицца не найден");
            }

            CreateModel(model, snack);
        }
        public void Delete(SnackBindingModel model)
        {
            var snack = source.Snacks.FirstOrDefault(recSnack => recSnack.Id == model.Id);

            if (snack != null)
            {
                source.Snacks.Remove(snack);
            }
            else
            {
                throw new Exception("Пицца не найден");
            }
        }
    }
}
