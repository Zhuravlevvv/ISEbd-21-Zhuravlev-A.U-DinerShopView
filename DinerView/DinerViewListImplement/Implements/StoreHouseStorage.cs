using DinerBusinessLogic.BindingModels;
using DinerBusinessLogic.Interfaces;
using DinerBusinessLogic.ViewModels;
using DinerViewListImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DinerViewListImplement.Implements
{
    public class StoreHouseStorage : IStoreHouseStorage
    {
        private readonly DataListSingleton source;
        public StoreHouseStorage()
        {
            source = DataListSingleton.GetInstance();
        }
        public List<StoreHouseViewModel> GetFullList()
        {
            List<StoreHouseViewModel> result = new List<StoreHouseViewModel>();
            foreach (var storeHouse in source.Storehouses)
            {
                result.Add(CreateModel(storeHouse));
            }
            return result;
        }
        public List<StoreHouseViewModel> GetFilteredList(StoreHouseBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            List<StoreHouseViewModel> result = new List<StoreHouseViewModel>();
            foreach (var storeHouse in source.Storehouses)
            {
                if (storeHouse.StoreHouseName.Contains(model.StoreHouseName))
                {
                    result.Add(CreateModel(storeHouse));
                }
            }
            return result;
        }
        public StoreHouseViewModel GetElement(StoreHouseBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            foreach (var storeHouse in source.Storehouses)
            {
                if (storeHouse.Id == model.Id || storeHouse.StoreHouseName ==
                model.StoreHouseName)
                {
                    return CreateModel(storeHouse);
                }
            }
            return null;
        }
        public void Insert(StoreHouseBindingModel model)
        {
            StoreHouse tempStoreHouse = new StoreHouse
            {
                Id = 1,
                StoreHouseFoods = new Dictionary<int, int>(),
                DateCreate = DateTime.Now
            };
            foreach (var storeHouse in source.Storehouses)
            {
                if (storeHouse.Id >= tempStoreHouse.Id)
                {
                    tempStoreHouse.Id = storeHouse.Id + 1;
                }
            }
            source.Storehouses.Add(CreateModel(model, tempStoreHouse));
        }
        public void Update(StoreHouseBindingModel model)
        {
            StoreHouse tempStoreHouse = null;
            foreach (var storeHouse in source.Storehouses)
            {
                if (storeHouse.Id == model.Id)
                {
                    tempStoreHouse = storeHouse;
                }
            }
            if (tempStoreHouse == null)
            {
                throw new Exception("Элемент не найден");
            }
            CreateModel(model, tempStoreHouse);
        }
        public void Delete(StoreHouseBindingModel model)
        {
            for (int i = 0; i < source.Storehouses.Count; ++i)
            {
                if (source.Storehouses[i].Id == model.Id)
                {
                    source.Storehouses.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
        private StoreHouse CreateModel(StoreHouseBindingModel model, StoreHouse storeHouse)
        {
            storeHouse.StoreHouseName = model.StoreHouseName;
            storeHouse.ResponsiblePersonFCS = model.ResponsiblePersonFCS;
            // удаляем убранные
            foreach (var key in storeHouse.StoreHouseFoods.Keys.ToList())
            {
                if (!model.StoreHouseFoods.ContainsKey(key))
                {
                    storeHouse.StoreHouseFoods.Remove(key);
                }
            }
            // обновляем существуюущие и добавляем новые
            foreach (var food in model.StoreHouseFoods)
            {
                if (storeHouse.StoreHouseFoods.ContainsKey(food.Key))
                {
                    storeHouse.StoreHouseFoods[food.Key] =
                    model.StoreHouseFoods[food.Key].Item2;
                }
                else
                {
                    storeHouse.StoreHouseFoods.Add(food.Key,
                    model.StoreHouseFoods[food.Key].Item2);
                }
            }
            return storeHouse;
        }
        private StoreHouseViewModel CreateModel(StoreHouse storeHouse)
        {
            // требуется дополнительно получить список компонентов для изделия с названиями и их количество
            Dictionary<int, (string, int)> storeHouseFoods = new Dictionary<int, (string, int)>();
            foreach (var storeHouseFood in storeHouse.StoreHouseFoods)
            {
                string foodName = string.Empty;
                foreach (var food in source.Foods)
                {
                    if (storeHouseFood.Key == food.Id)
                    {
                        foodName = food.FoodName;
                        break;
                    }
                }
                storeHouseFoods.Add(storeHouseFood.Key, (foodName, storeHouseFood.Value));
            }
            return new StoreHouseViewModel
            {
                Id = storeHouse.Id,
                StoreHouseName = storeHouse.StoreHouseName,
                ResponsiblePersonFCS = storeHouse.ResponsiblePersonFCS,
                DateCreate = storeHouse.DateCreate,
                StoreHouseFoods = storeHouseFoods
            };
        }
        public void Print()
        {
            foreach (StoreHouse storehouse in source.Storehouses)
            {
                Console.WriteLine(storehouse.StoreHouseName + " " + storehouse.ResponsiblePersonFCS + " " + storehouse.DateCreate);
                foreach (KeyValuePair<int, int> keyValue in storehouse.StoreHouseFoods)
                {
                    string foodName = source.Foods.FirstOrDefault(component => component.Id == keyValue.Key).FoodName;
                    Console.WriteLine(foodName + " " + keyValue.Value);
                }
            }
        }
        public bool CheckAndTake(int count, Dictionary<int, (string, int)> foods)
        {
            throw new NotImplementedException();
        }
    }
}
