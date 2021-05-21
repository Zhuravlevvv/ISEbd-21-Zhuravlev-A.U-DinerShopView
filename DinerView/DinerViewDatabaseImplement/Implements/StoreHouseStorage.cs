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
        public List<StoreHouseViewModel> GetFullList()
        {
            using (var context = new DinerViewDatabase())
            {
                return context.StoreHouses
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
                .ToDictionary(recPC => recPC.FoodId, recPC =>
                (recPC.Food?.FoodName, recPC.Count))
                })
                .ToList();
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
                .ToDictionary(recPC => recPC.FoodId, recPC =>
                (recPC.Food?.FoodName, recPC.Count))
                })
                .ToList();
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
                var storehouse = context.StoreHouses
                .Include(rec => rec.StoreHouseFoods)
                .ThenInclude(rec => rec.Food)
                .FirstOrDefault(rec => rec.StoreHouseName.Equals(model.StoreHouseName) || rec.Id
                == model.Id);
                return storehouse != null ?
                new StoreHouseViewModel
                {
                    Id = storehouse.Id,
                    StoreHouseName = storehouse.StoreHouseName,
                    ResponsiblePersonFCS = storehouse.ResponsiblePersonFCS,
                    DateCreate = storehouse.DateCreate,
                    StoreHouseFoods = storehouse.StoreHouseFoods
                .ToDictionary(recPC => recPC.FoodId, recPC =>
                (recPC.Food?.FoodName, recPC.Count))
                } : null;
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
                        StoreHouse storehouse = CreateModel(model, new StoreHouse());
                        context.StoreHouses.Add(storehouse);
                        context.SaveChanges();
                        CreateModel(model, storehouse, context);

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
                        var element = context.StoreHouses.FirstOrDefault(rec => rec.Id == model.Id);
                        if (element == null)
                        {
                            throw new Exception("Элемент не найден");
                        }
                        CreateModel(model, element, context);
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

        public void Delete(StoreHouseBindingModel model)
        {
            using (var context = new DinerViewDatabase())
            {
                StoreHouse element = context.StoreHouses.FirstOrDefault(rec => rec.Id == model.Id);
                if (element != null)
                {
                    context.StoreHouses.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }

        private StoreHouse CreateModel(StoreHouseBindingModel model, StoreHouse storehouse)
        {
            storehouse.StoreHouseName = model.StoreHouseName;
            storehouse.ResponsiblePersonFCS = model.ResponsiblePersonFCS;
            storehouse.DateCreate = model.DateCreate;
            return storehouse;
        }

        private StoreHouse CreateModel(StoreHouseBindingModel model, StoreHouse storeHouse,
       DinerViewDatabase context)
        {
            storeHouse.StoreHouseName = model.StoreHouseName;
            storeHouse.ResponsiblePersonFCS = model.ResponsiblePersonFCS;
            storeHouse.DateCreate = model.DateCreate;
            if (model.Id.HasValue)
            {
                var productComponents = context.StoreHouseFoods.Where(rec =>
                rec.StoreHouseId == model.Id.Value).ToList();
                // удалили те, которых нет в модели
                context.StoreHouseFoods.RemoveRange(productComponents.Where(rec =>
                !model.StoreHouseFoods.ContainsKey(rec.FoodId)).ToList());
                context.SaveChanges();
                // обновили количество у существующих записей
                foreach (var updateComponent in productComponents)
                {
                    updateComponent.Count =
                    model.StoreHouseFoods[updateComponent.FoodId].Item2;
                    model.StoreHouseFoods.Remove(updateComponent.FoodId);
                }
                context.SaveChanges();
            }
            // добавили новые
            foreach (var pc in model.StoreHouseFoods)
            {
                context.StoreHouseFoods.Add(new StoreHouseFood
                {
                    StoreHouseId = storeHouse.Id,
                    FoodId = pc.Key,
                    Count = pc.Value.Item2,
                });
                try
                {
                    context.SaveChanges();
                }
                catch
                {
                    throw;
                }
            }
            return storeHouse;
        }

        public bool CheckAndTake(int PackageId, int Count)
        {
            using (var context = new DinerViewDatabase())
            {
                var list = GetFullList();
                var DCount = context.SnackFoods.Where(rec => rec.SnackId == PackageId)
                    .ToDictionary(rec => rec.FoodId, rec => rec.Count * Count);

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var key in DCount.Keys.ToArray())
                        {
                            foreach (var storeHouseFood in context.StoreHouseFoods.Where(rec => rec.FoodId == key))
                            {
                                if (storeHouseFood.Count > DCount[key])
                                {
                                    storeHouseFood.Count -= DCount[key];
                                    DCount[key] = 0;
                                    break;
                                }
                                else
                                {
                                    DCount[key] -= storeHouseFood.Count;
                                    storeHouseFood.Count = 0;
                                }
                            }
                            if (DCount[key] > 0)
                            {
                                transaction.Rollback();
                                return false;
                            }
                        }
                        context.SaveChanges();
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
    }
}
