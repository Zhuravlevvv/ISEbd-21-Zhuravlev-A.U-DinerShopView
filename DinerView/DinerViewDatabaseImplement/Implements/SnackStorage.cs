using System;
using DinerBusinessLogic.Interfaces;
using DinerBusinessLogic.ViewModels;
using DinerBusinessLogic.BindingModels;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using DinerViewDatabaseImplement.Models;

namespace DinerViewDatabaseImplement.Implements
{
    public class SnackStorage : ISnackStorage
    {
        private Snack CreateModel(SnackBindingModel model, Snack snack, DinerViewDatabase context)
        {
            snack.SnackName = model.SnackName;
            snack.Price = model.Price;
            if (snack.Id == 0)
            {
                context.Snacks.Add(snack);
                context.SaveChanges();
            }

            if (model.Id.HasValue)
            {
                var SnackFoods = context.SnackFoods
                    .Where(rec => rec.SnackId == model.Id.Value)
                    .ToList();

                context.SnackFoods.RemoveRange(SnackFoods
                    .Where(rec => !model.SnackFoods.ContainsKey(rec.FoodId))
                    .ToList());
                context.SaveChanges();

                foreach (var updateFood in SnackFoods)
                {
                    updateFood.Count = model.SnackFoods[updateFood.FoodId].Item2;
                    model.SnackFoods.Remove(updateFood.FoodId);
                }
                context.SaveChanges();
            }


            foreach (var SnackFood in model.SnackFoods)
            {
                context.SnackFoods.Add(new SnackFood
                {
                    SnackId = snack.Id,
                    FoodId = SnackFood.Key,
                    Count = SnackFood.Value.Item2
                });
                context.SaveChanges();
            }

            return snack;
        }

        public List<SnackViewModel> GetFullList()
        {
            using (var context = new DinerViewDatabase())
            {
                return context.Snacks
                    .Include(rec => rec.SnackFoods)
                    .ThenInclude(rec => rec.Food)
                    .ToList()
                    .Select(rec => new SnackViewModel
                    {
                        Id = rec.Id,
                        SnackName = rec.SnackName,
                        Price = rec.Price,
                        SnackFoods = rec.SnackFoods
                            .ToDictionary(recSnackFoods => recSnackFoods.FoodId,
                            recSnackFoods => (recSnackFoods.Food?.FoodName,
                            recSnackFoods.Count))
                    })
                    .ToList();
            }
        }

        public List<SnackViewModel> GetFilteredList(SnackBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new DinerViewDatabase())
            {
                return context.Snacks
                    .Include(rec => rec.SnackFoods)
                    .ThenInclude(rec => rec.Food)
                    .Where(rec => rec.SnackName.Contains(model.SnackName))
                    .ToList()
                    .Select(rec => new SnackViewModel
                    {
                        Id = rec.Id,
                        SnackName = rec.SnackName,
                        Price = rec.Price,
                        SnackFoods = rec.SnackFoods
                            .ToDictionary(recSnackFood => recSnackFood.FoodId,
                            recSnackFood => (recSnackFood.Food?.FoodName,
                            recSnackFood.Count))
                    })
                    .ToList();
            }
        }

        public SnackViewModel GetElement(SnackBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new DinerViewDatabase())
            {
                var Snack = context.Snacks
                    .Include(rec => rec.SnackFoods)
                    .ThenInclude(rec => rec.Food)
                    .FirstOrDefault(rec => rec.SnackName == model.SnackName ||
                    rec.Id == model.Id);

                return Snack != null ?
                    new SnackViewModel
                    {
                        Id = Snack.Id,
                        SnackName = Snack.SnackName,
                        Price = Snack.Price,
                        SnackFoods = Snack.SnackFoods
                            .ToDictionary(recSnackFood => recSnackFood.FoodId,
                            recSnackFood => (recSnackFood.Food?.FoodName,
                            recSnackFood.Count))
                    } :
                    null;
            }
        }

        public void Insert(SnackBindingModel model)
        {
            using (var context = new DinerViewDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        CreateModel(model, new Snack(), context);
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

        public void Update(SnackBindingModel model)
        {
            using (var context = new DinerViewDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var secure = context.Snacks.FirstOrDefault(rec => rec.Id == model.Id);

                        if (secure == null)
                        {
                            throw new Exception("Продукт не найден");
                        }

                        context.Snacks.Add(CreateModel(model, new Snack(), context));
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

        public void Delete(SnackBindingModel model)
        {
            using (var context = new DinerViewDatabase())
            {
                var Snack = context.Snacks.FirstOrDefault(rec => rec.Id == model.Id);

                if (Snack == null)
                {
                    throw new Exception("Компонент не найден");
                }

                context.Snacks.Remove(Snack);
                context.SaveChanges();
            }
        }
    }
}
