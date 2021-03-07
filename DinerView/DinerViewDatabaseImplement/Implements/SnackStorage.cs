using System;
using System.Collections.Generic;
using System.Linq;
using DinerBusinessLogic.BindingModels;
using DinerBusinessLogic.Interfaces;
using DinerBusinessLogic.ViewModels;
using DinerViewDatabaseImplement.Models;
using Microsoft.EntityFrameworkCore;

namespace DinerViewDatabaseImplement.Implements
{
    public class SnackStorage : ISnackStorage
    {
        public List<SnackViewModel> GetFullList()
        {
            using (var context = new DinerViewDatabase())
            {
                return context.Snacks.Include(rec => rec.SnackFoods).ThenInclude(rec => rec.Food)
                .ToList().Select(rec => new SnackViewModel
                {
                    Id = rec.Id,
                    SnackName = rec.SnackName,
                    Price = rec.Price,
                    SnackFoods = rec.SnackFoods.ToDictionary(recPC => recPC.FoodId, recPC => (recPC.Food?.FoodName, recPC.Count))
                }).ToList();
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
                return context.Snacks.Include(rec => rec.SnackFoods).ThenInclude(rec => rec.Food)
                .Where(rec => rec.SnackName.Contains(model.SnackName)).ToList().Select(rec => new SnackViewModel
                {
                    Id = rec.Id,
                    SnackName = rec.SnackName,
                    Price = rec.Price,
                }).ToList();
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
                var snack = context.Snacks.Include(rec => rec.SnackFoods).ThenInclude(rec => rec.Food)
                .FirstOrDefault(rec => rec.SnackName == model.SnackName || rec.Id == model.Id);
                return snack != null ?
                new SnackViewModel
                {
                    Id = snack.Id,
                    SnackName = snack.SnackName,
                    Price = snack.Price,
                    SnackFoods = snack.SnackFoods.ToDictionary(recPC => recPC.FoodId, recPC => (recPC.Food?.FoodName, recPC.Count))
                } : null;
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
                        Snack canned = CreateModel(model, new Snack());
                        context.Snacks.Add(canned);
                        context.SaveChanges();
                        CreateModel(model, canned, context);
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
                        var element = context.Snacks.FirstOrDefault(rec => rec.Id == model.Id);
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
        public void Delete(SnackBindingModel model)
        {
            using (var context = new DinerViewDatabase())
            {
                Snack element = context.Snacks.FirstOrDefault(rec => rec.Id ==
                model.Id);
                if (element != null)
                {
                    context.Snacks.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }
        private Snack CreateModel(SnackBindingModel model, Snack snack)
        {
            snack.SnackName = model.SnackName;
            snack.Price = model.Price;
            return snack;
        }
        private Snack CreateModel(SnackBindingModel model, Snack snack, DinerViewDatabase context)
        {
            snack.SnackName = model.SnackName;
            snack.Price = model.Price;
            if (model.Id.HasValue)
            {
                var snackFoods = context.SnackFoods.Where(rec => rec.SnackId == model.Id.Value).ToList();
                // удалили те, которых нет в модели
                context.SnackFoods.RemoveRange(snackFoods.Where(rec => !model.SnackFoods.ContainsKey(rec.FoodId)).ToList());
                context.SaveChanges();
                // обновили количество у существующих записей
                foreach (var updateFood in snackFoods)
                {
                    updateFood.Count = model.SnackFoods[updateFood.FoodId].Item2;
                    model.SnackFoods.Remove(updateFood.FoodId);
                }
                context.SaveChanges();
            }
            // добавили новые
            foreach (var pc in model.SnackFoods)
            {
                context.SnackFoods.Add(new SnackFood
                {
                    SnackId = snack.Id,
                    FoodId = pc.Key,
                    Count = pc.Value.Item2
                });
                context.SaveChanges();
            }
            return snack;
        }
    }
}
