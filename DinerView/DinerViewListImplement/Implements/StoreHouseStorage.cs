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
            foreach (var storehouse in source.StoreHouses)
            {
                result.Add(CreateModel(storehouse));
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
            foreach (var storehouse in source.StoreHouses)
            {
                if (storehouse.StoreHouseName.Contains(model.StoreHouseName))
                {
                    result.Add(CreateModel(storehouse));
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
            foreach (var storehouse in source.StoreHouses)
            {
                if (storehouse.Id == model.Id || storehouse.StoreHouseName.Equals(model.StoreHouseName))
                {
                    return CreateModel(storehouse);
                }
            }
            return null;
        }

        public void Insert(StoreHouseBindingModel model)
        {
            StoreHouse tempStorehouse = new StoreHouse
            {
                Id = 1,
                StoreHouseFoods = new Dictionary<int, int>()
            };
            foreach (var storehouse in source.StoreHouses)
            {
                if (storehouse.Id >= tempStorehouse.Id)
                {
                    tempStorehouse.Id = storehouse.Id + 1;
                }
            }
            source.StoreHouses.Add(CreateModel(model, tempStorehouse));
        }

        public void Update(StoreHouseBindingModel model)
        {
            StoreHouse tempStorehouse = null;
            foreach (var storehouse in source.StoreHouses)
            {
                if (storehouse.Id == model.Id)
                {
                    tempStorehouse = storehouse;
                }
            }
            if (tempStorehouse == null)
            {
                throw new Exception("Элемент не найден");
            }
            CreateModel(model, tempStorehouse);
        }

        public void Delete(StoreHouseBindingModel model)
        {
            for (int i = 0; i < source.StoreHouses.Count; ++i)
            {
                if (source.StoreHouses[i].Id == model.Id)
                {
                    source.StoreHouses.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }

        private StoreHouse CreateModel(StoreHouseBindingModel model, StoreHouse storeHouse)
        {
            storeHouse.StoreHouseName = model.StoreHouseName;
            storeHouse.ResponsiblePersonFCS = model.ResponsiblePersonFCS;
            storeHouse.DateCreate = model.DateCreate;
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
            Dictionary<int, (string, int)> storehouseFoods = new
            Dictionary<int, (string, int)>();
            foreach (var pc in storeHouse.StoreHouseFoods)
            {
                string componentName = string.Empty;
                foreach (var component in source.Foods)
                {
                    if (pc.Key == component.Id)
                    {
                        componentName = component.FoodName;
                        break;
                    }
                }
                storehouseFoods.Add(pc.Key, (componentName, pc.Value));
            }
            return new StoreHouseViewModel
            {
                Id = storeHouse.Id,
                StoreHouseName = storeHouse.StoreHouseName,
                ResponsiblePersonFCS = storeHouse.ResponsiblePersonFCS,
                DateCreate = storeHouse.DateCreate,
                StoreHouseFoods = storehouseFoods
            };
        }

        public bool CheckAndTake(int SnackId, int Count)
        {
            throw new NotImplementedException();
        }
    }
}
