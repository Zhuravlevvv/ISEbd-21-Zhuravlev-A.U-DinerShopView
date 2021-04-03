using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DinerBusinessLogic.BindingModels;
using DinerBusinessLogic.Interfaces;
using DinerBusinessLogic.ViewModels;
using DinerViewDatabaseImplement.Models;

namespace DinerViewDatabaseImplement.Implements
{
    public class StoreHouseStorage : IStoreHouseStorage
    {
        private StoreHouse CreateModel(StoreHouseBindingModel model, StoreHouse storeHouse, DinerViewDatabase context)
        {
            storeHouse.StoreHouseName = model.StoreHouseName;
            storeHouse.ResponsiblePersonFCS = model.ResponsiblePersonFCS;
            if (storeHouse.Id == 0)
            {
                storeHouse.DateCreate = DateTime.Now;
                context.StoreHouses.Add(storeHouse);
                context.SaveChanges();
            }

            if (model.Id.HasValue)
            {
                var storeHouseFoods = context.StoreHouseFoods
                    .Where(rec => rec.StoreHouseId == model.Id.Value)
                    .ToList();

                context.StoreHouseFoods.RemoveRange(storeHouseFoods
                    .Where(rec => !model.StoreHouseFoods.ContainsKey(rec.FoodId))
                    .ToList());
                context.SaveChanges();

                foreach (var updateFood in storeHouseFoods)
                {
                    updateFood.Count = model.StoreHouseFoods[updateFood.FoodId].Item2;
                    model.StoreHouseFoods.Remove(updateFood.FoodId);
                }
                context.SaveChanges();
            }

            foreach (var storeHousFood in model.StoreHouseFoods)
            {
                context.StoreHouseFoods.Add(new StoreHouseFood
                {
                    StoreHouseId = storeHouse.Id,
                    FoodId = storeHousFood.Key,
                    Count = storeHousFood.Value.Item2
                });
                context.SaveChanges();
            }

            return storeHouse;
        }

        public bool CheckAndTake(int count, Dictionary<int, (string, int)> foods)
        {
            using (var context = new DinerViewDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var storeHouseFood in foods)
                        {
                            int requiredCount = storeHouseFood.Value.Item2 * count;
                            int countInStoreHouses = context.StoreHouseFoods
                                .Where(rec => rec.FoodId == storeHouseFood.Key)
                                .Sum(rec => rec.Count);
                            if (requiredCount > countInStoreHouses)
                            {
                                throw new Exception("На складе недостаточно продуктов!");
                            }

                            IEnumerable<StoreHouseFood> storeHouseFoods = context.StoreHouseFoods
                                .Where(rec => rec.FoodId == storeHouseFood.Key);
                            foreach (var food in storeHouseFoods)
                            {
                                if (food.Count <= requiredCount)
                                {
                                    requiredCount -= food.Count;
                                    context.StoreHouseFoods.Remove(food);
                                    context.SaveChanges();
                                }
                                else
                                {
                                    food.Count -= requiredCount;
                                    context.SaveChanges();
                                    requiredCount = 0;
                                }
                                if (requiredCount == 0)
                                {
                                    break;
                                }
                            }
                        }
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Delete(StoreHouseBindingModel model)
        {
            using (var context = new DinerViewDatabase())
            {
                var storeHouse = context.StoreHouses.FirstOrDefault(rec => rec.Id == model.Id);

                if (storeHouse == null)
                {
                    throw new Exception("Склад не найден");
                }

                context.StoreHouses.Remove(storeHouse);
                context.SaveChanges();
            }
        }

        public StoreHouseViewModel GetElement(StoreHouseBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new DinerViewDatabase())
            {
                var storeHouse = context.StoreHouses
                    .Include(rec => rec.StoreHouseFoods)
                    .ThenInclude(rec => rec.Food)
                    .FirstOrDefault(rec => rec.StoreHouseName == model.StoreHouseName ||
                    rec.Id == model.Id);

                return storeHouse != null ?
                    new StoreHouseViewModel
                    {
                        Id = storeHouse.Id,
                        StoreHouseName = storeHouse.StoreHouseName,
                        ResponsiblePersonFCS = storeHouse.ResponsiblePersonFCS,
                        DateCreate = storeHouse.DateCreate,
                        StoreHouseFoods = storeHouse.StoreHouseFoods
                            .ToDictionary(recStoreHouseFood => recStoreHouseFood.FoodId,
                            recStoreHouseFood => (recStoreHouseFood.Food?.FoodName,
                            recStoreHouseFood.Count))
                    } :
                    null;
            }
        }

        public List<StoreHouseViewModel> GetFilteredList(StoreHouseBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new DinerViewDatabase())
            {
                return context.StoreHouses
                    .Include(rec => rec.StoreHouseFoods)
                    .ThenInclude(rec => rec.Food)
                    .Where(rec => rec.StoreHouseName.Contains(model.StoreHouseName))
                    .ToList()
                    .Select(rec => new StoreHouseViewModel
                    {
                        Id = rec.Id,
                        StoreHouseName = rec.StoreHouseName,
                        ResponsiblePersonFCS = rec.ResponsiblePersonFCS,
                        DateCreate = rec.DateCreate,
                        StoreHouseFoods = rec.StoreHouseFoods
                            .ToDictionary(recStoreHouseFood => recStoreHouseFood.FoodId,
                            recStoreHouseFood => (recStoreHouseFood.Food?.FoodName,
                            recStoreHouseFood.Count))
                    })
                    .ToList();
            }
        }

        public List<StoreHouseViewModel> GetFullList()
        {
            using (var context = new DinerViewDatabase())
            {
                return context.StoreHouses.Count() == 0 ? new List<StoreHouseViewModel>() :
                    context.StoreHouses
                    .Include(rec => rec.StoreHouseFoods)
                    .ThenInclude(rec => rec.Food)
                    .ToList()
                    .Select(rec => new StoreHouseViewModel
                    {
                        Id = rec.Id,
                        StoreHouseName = rec.StoreHouseName,
                        ResponsiblePersonFCS = rec.ResponsiblePersonFCS,
                        DateCreate = rec.DateCreate,
                        StoreHouseFoods = rec.StoreHouseFoods
                            .ToDictionary(recStoreHouseFoods => recStoreHouseFoods.FoodId,
                            recStoreHouseFoods => (recStoreHouseFoods.Food?.FoodName,
                            recStoreHouseFoods.Count))
                    })
                    .ToList();
            }
        }

        public void Insert(StoreHouseBindingModel model)
        {
            using (var context = new DinerViewDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        CreateModel(model, new StoreHouse(), context);
                        context.SaveChanges();

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Update(StoreHouseBindingModel model)
        {
            using (var context = new DinerViewDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var storeHouse = context.StoreHouses.FirstOrDefault(rec => rec.Id == model.Id);

                        if (storeHouse == null)
                        {
                            throw new Exception("Склад не найден");
                        }

                        CreateModel(model, storeHouse, context);
                        context.SaveChanges();

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
