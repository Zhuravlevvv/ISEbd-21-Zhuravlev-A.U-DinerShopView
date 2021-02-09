using System;
using System.Collections.Generic;
using DinerBusinessLogic.BindingModels;
using DinerBusinessLogic.Interfaces;
using DinerBusinessLogic.ViewModels;
using DinerViewListImplement.Models;
using System.Linq;

namespace DinerViewListImplement.Implements
{
    public class SnackStorage : ISnackStorage
    {
        private readonly DataListSingleton source;
        public SnackStorage()
        {
            source = DataListSingleton.GetInstance();
        }
        public List<SnackViewModel> GetFullList()
        {
            List<SnackViewModel> result = new List<SnackViewModel>();
            foreach (var snack in source.Snacks)
            {
                result.Add(CreateModel(snack));
            }
            return result;
        }
        public List<SnackViewModel> GetFilteredList(SnackBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            List<SnackViewModel> result = new List<SnackViewModel>();
            foreach (var snack in source.Snacks)
            {
                if (snack.SnackName.Contains(model.SnackName))
                {
                    result.Add(CreateModel(snack));
                }
            }
            return result;
        }
        public SnackViewModel GetElement(SnackBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            foreach (var snack in source.Snacks)
            {
                if (snack.Id == model.Id || snack.SnackName ==
                model.SnackName)
                {
                    return CreateModel(snack);
                }
            }
            return null;
        }
        public void Insert(SnackBindingModel model)
        {
            Snack tempSnack = new Snack
            {
                Id = 1,
                SnackFoods = new Dictionary<int, int>()
            };
            foreach (var snack in source.Snacks)
            {
                if (snack.Id >= tempSnack.Id)
                {
                    tempSnack.Id = snack.Id + 1;
                }
            }
            source.Snacks.Add(CreateModel(model, tempSnack));
        }
        public void Update(SnackBindingModel model)
        {
            Snack tempSnack = null;
            foreach (var snack in source.Snacks)
            {
                if (snack.Id == model.Id)
                {
                    tempSnack = snack;
                }
            }
            if (tempSnack == null)
            {
                throw new Exception("Элемент не найден");
            }
            CreateModel(model, tempSnack);
        }
        public void Delete(SnackBindingModel model)
        {
            for (int i = 0; i < source.Snacks.Count; ++i)
            {
                if (source.Snacks[i].Id == model.Id)
                {
                    source.Snacks.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
        private Snack CreateModel(SnackBindingModel model, Snack snack)
        {
            snack.SnackName = model.SnackName;
            snack.Price = model.Price;
            // удаляем убранные
            foreach (var key in snack.SnackFoods.Keys.ToList())
            {
                if (!model.SnackFoods.ContainsKey(key))
                {
                    snack.SnackFoods.Remove(key);
                }
            }
            // обновляем существуюущие и добавляем новые
            foreach (var snacks in model.SnackFoods)
            {
                if (snack.SnackFoods.ContainsKey(snacks.Key))
                {
                    snack.SnackFoods[snacks.Key] =
                    model.SnackFoods[snacks.Key].Item2;
                }
                else
                {
                    snack.SnackFoods.Add(snacks.Key,
                    model.SnackFoods[snacks.Key].Item2);
                }
            }
            return snack;
        }
        private SnackViewModel CreateModel(Snack snack)
        {
            // требуется дополнительно получить список компонентов для изделия названиями и их количество
            Dictionary<int, (string, int)> snackFoods = new
            Dictionary<int, (string, int)>();
            foreach (var sf in snack.SnackFoods)
            {
                string foodName = string.Empty;
                foreach (var snacks in source.Foods)
                {
                    if (sf.Key == snack.Id)
                    {
                        foodName = snacks.FoodName;
                        break;
                    }
                }
                snackFoods.Add(sf.Key, (foodName, sf.Value));
            }
            return new SnackViewModel
            {
                Id = snack.Id,
                SnackName = snack.SnackName,
                Price = snack.Price,
                SnackFoods = snackFoods
            };
        }
    }
}