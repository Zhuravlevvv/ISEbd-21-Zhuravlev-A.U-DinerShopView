using DinerBusinessLogic.BindingModels;
using DinerBusinessLogic.Interfaces;
using DinerBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DinerBusinessLogic.BusinessLogics
{
    public class StoreHouseLogic
    {
        private readonly IStoreHouseStorage _storeHouseStorage;

        private readonly IFoodStorage _foodStorage;

        public StoreHouseLogic(IStoreHouseStorage storeHouseStorage, IFoodStorage foodStorage)
        {
            _storeHouseStorage = storeHouseStorage;
            _foodStorage = foodStorage;
        }

        public List<StoreHouseViewModel> Read(StoreHouseBindingModel model)
        {
            if (model == null)
            {
                return _storeHouseStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<StoreHouseViewModel> { _storeHouseStorage.GetElement(model) };
            }
            return _storeHouseStorage.GetFilteredList(model);
        }

        public void CreateOrUpdate(StoreHouseBindingModel model)
        {
            var element = _storeHouseStorage.GetElement(new StoreHouseBindingModel
            {
                StoreHouseName = model.StoreHouseName
            });
            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Уже есть склад с таким названием");
            }
            if (model.Id.HasValue)
            {
                _storeHouseStorage.Update(model);
            }
            else
            {
                _storeHouseStorage.Insert(model);
            }
        }

        public void Delete(StoreHouseBindingModel model)
        {
            var element = _storeHouseStorage.GetElement(new StoreHouseBindingModel
            {
                Id = model.Id
            });
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            _storeHouseStorage.Delete(model);
        }

        public void Replenishment(StoreHouseReplenishmentBindingModel model)
        {
            var storeHouse = _storeHouseStorage.GetElement(new StoreHouseBindingModel
            {
                Id = model.StoreHouseId
            });

            var food = _foodStorage.GetElement(new FoodBindingModel
            {
                Id = model.FoodId
            });
            if (storeHouse == null)
            {
                throw new Exception("Не найден склад");
            }
            if (food == null)
            {
                throw new Exception("Не найден компонент");
            }
            if (storeHouse.StoreHouseFoods.ContainsKey(model.FoodId))
            {
                storeHouse.StoreHouseFoods[model.FoodId] =
                    (food.FoodName, storeHouse.StoreHouseFoods[model.FoodId].Item2 + model.Count);
            }
            else
            {
                storeHouse.StoreHouseFoods.Add(food.Id, (food.FoodName, model.Count));
            }
            _storeHouseStorage.Update(new StoreHouseBindingModel
            {
                Id = storeHouse.Id,
                StoreHouseName = storeHouse.StoreHouseName,
                ResponsiblePersonFCS = storeHouse.ResponsiblePersonFCS,
                DateCreate = storeHouse.DateCreate,
                StoreHouseFoods = storeHouse.StoreHouseFoods
            });
        }
    }
}
