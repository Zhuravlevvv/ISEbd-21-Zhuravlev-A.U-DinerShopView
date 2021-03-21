using System;
using System.Collections.Generic;
using System.Linq;
using DinerBusinessLogic.BindingModels;
using DinerBusinessLogic.Interfaces;
using DinerBusinessLogic.ViewModels;
using DinerViewFileImplement.Models;

namespace DinerViewFileImplement.Implements
{
    public class StoreHouseStorage : IStoreHouseStorage
    {
        private readonly FileDataListSingleton source;

        public StoreHouseStorage()
        {
            source = FileDataListSingleton.GetInstance();
        }

        public List<StoreHouseViewModel> GetFullList()
        {
            return source.StoreHouses
                .Select(CreateModel)
                .ToList();
        }

        public List<StoreHouseViewModel> GetFilteredList(StoreHouseBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            return source.StoreHouses
                .Where(rec => rec.StoreHouseName.Contains(model.StoreHouseName))
                .Select(CreateModel)
                .ToList();
        }

        public StoreHouseViewModel GetElement(StoreHouseBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            var storeHouse = source.StoreHouses.FirstOrDefault(rec => rec.StoreHouseName == model.StoreHouseName ||
            rec.Id == model.Id);
            return storeHouse != null ? CreateModel(storeHouse) : null;
        }

        public void Insert(StoreHouseBindingModel model)
        {
            int maxId = source.StoreHouses.Count > 0 ? source.StoreHouses.Max(rec => rec.Id) : 0;
            var storeHouse = new StoreHouse
            {
                Id = maxId + 1,
                StoreHouseFoods = new Dictionary<int, int>(),
                DateCreate = DateTime.Now
            };
            source.StoreHouses.Add(CreateModel(model, storeHouse));
        }

        public void Update(StoreHouseBindingModel model)
        {
            var storeHouse = source.StoreHouses.FirstOrDefault(rec => rec.Id == model.Id);

            if (storeHouse == null)
            {
                throw new Exception("Элемент не найден");
            }
            CreateModel(model, storeHouse);
        }

        public void Delete(StoreHouseBindingModel model)
        {
            var storeHouse = source.StoreHouses.FirstOrDefault(rec => rec.Id == model.Id);

            if (storeHouse == null)
            {
                throw new Exception("Элемент не найден");
            }
            source.StoreHouses.Remove(storeHouse);
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
            foreach (var component in model.StoreHouseFoods)
            {
                if (storeHouse.StoreHouseFoods.ContainsKey(component.Key))
                {
                    storeHouse.StoreHouseFoods[component.Key] =
                    model.StoreHouseFoods[component.Key].Item2;
                }
                else
                {
                    storeHouse.StoreHouseFoods.Add(component.Key,
                    model.StoreHouseFoods[component.Key].Item2);
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
                foreach (var component in source.Foods)
                {
                    if (storeHouseFood.Key == component.Id)
                    {
                        foodName = component.FoodName;
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

        public bool CheckAndTake(int count, Dictionary<int, (string, int)> components)
        {
            foreach (var component in components)
            {
                int requiredCount = component.Value.Item2 * count;
                int availableCount = source.StoreHouses
                    .Where(rec => rec.StoreHouseFoods.ContainsKey(component.Key))
                    .Sum(rec => rec.StoreHouseFoods[component.Key]);
                if (availableCount < requiredCount)
                {
                    return false;
                }
            }
            foreach (var component in components)
            {
                int requiredCount = component.Value.Item2 * count;
                List<StoreHouse> availableStoreHouses = source.StoreHouses
                    .Where(rec => rec.StoreHouseFoods.ContainsKey(component.Key))
                    .ToList();
                foreach (var storeHouse in availableStoreHouses)
                {
                    int availableCount = storeHouse.StoreHouseFoods[component.Key];
                    if (availableCount <= requiredCount)
                    {
                        requiredCount = requiredCount - availableCount;
                        storeHouse.StoreHouseFoods.Remove(component.Key);
                    }
                    else
                    {
                        storeHouse.StoreHouseFoods[component.Key] -= requiredCount;
                        requiredCount = 0;
                    }
                    if (requiredCount == 0)
                    {
                        break;
                    }
                }
            }
            return true;
        }
    }
}
