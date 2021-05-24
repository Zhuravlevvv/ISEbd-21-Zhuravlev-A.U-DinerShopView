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

        public bool CheckAndTake(int SnackId, int Count)
        {
            var list = GetFullList();
            var DCount = source.Snacks.FirstOrDefault(rec => rec.Id == SnackId).SnackFoods;
            DCount = DCount.ToDictionary(rec => rec.Key, rec => rec.Value * Count);
            Dictionary<int, int> Have = new Dictionary<int, int>();

            // считаем сколько у нас всего нужных компонентов
            foreach (var view in list)
            {
                foreach (var d in view.StoreHouseFoods)
                {
                    int key = d.Key;
                    if (DCount.ContainsKey(key))
                    {
                        if (Have.ContainsKey(key))
                        {
                            Have[key] += d.Value.Item2;
                        }
                        else
                        {
                            Have.Add(key, d.Value.Item2);
                        }
                    }
                }
            }

            //проверяем хватает ли компонентов
            foreach (var key in Have.Keys)
            {
                if (DCount[key] > Have[key])
                {
                    return false;
                }
            }

            // вычитаем со складов компоненты
            foreach (var view in list)
            {
                var storehouseFoods = view.StoreHouseFoods;
                foreach (var key in view.StoreHouseFoods.Keys.ToArray())
                {
                    var value = view.StoreHouseFoods[key];
                    if (DCount.ContainsKey(key))
                    {
                        if (value.Item2 > DCount[key])
                        {
                            storehouseFoods[key] = (value.Item1, value.Item2 - DCount[key]);
                            DCount[key] = 0;
                        }
                        else
                        {
                            storehouseFoods[key] = (value.Item1, 0);
                            DCount[key] -= value.Item2;
                        }
                        Update(new StoreHouseBindingModel
                        {
                            Id = view.Id,
                            DateCreate = view.DateCreate,
                            ResponsiblePersonFCS = view.ResponsiblePersonFCS,
                            StoreHouseName = view.StoreHouseName,
                            StoreHouseFoods = storehouseFoods
                        });
                    }
                }
            }
            return true;
        }
    }
}
