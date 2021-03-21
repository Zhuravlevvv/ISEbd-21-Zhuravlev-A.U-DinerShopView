using System;
using System.Collections.Generic;
using System.Linq;
using DinerViewFileImplement.Models;
using DinerBusinessLogic.BindingModels;
using DinerBusinessLogic.Interfaces;
using DinerBusinessLogic.ViewModels;

namespace DinerViewFileImplement.Implements
{
    public class OrderStorage : IOrderStorage
    {
        private readonly FileDataListSingleton sourse;
        private Order CreateModel(OrderBindingModel model, Order order)
        {
            order.SnackId = model.SnackId;
            order.Count = model.Count;
            order.Sum = model.Sum;
            order.Status = model.Status;
            order.DateCreate = model.DateCreate;
            order.DateImplement = model.DateImplement;

            return order;
        }
        private OrderViewModel CreateModel(Order order)
        {
            return new OrderViewModel
            {
                Id = order.Id,
                SnackName = sourse.Snacks.FirstOrDefault(snack => snack.Id == order.SnackId)?.SnackName,
                SnackId = order.SnackId,
                Count = order.Count,
                Sum = order.Sum,
                Status = order.Status,
                DateCreate = order.DateCreate,
                DateImplement = order.DateImplement
            };
        }
        public OrderStorage()
        {
            sourse = FileDataListSingleton.GetInstance();
        }
        public List<OrderViewModel> GetFullList()
        {
            return sourse.Orders.Select(CreateModel).ToList();
        }
        public List<OrderViewModel> GetFilteredList(OrderBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            return sourse.Orders.Where(recOrder => recOrder.Id == model.Id).Select(CreateModel).ToList();
        }
        public OrderViewModel GetElement(OrderBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            var order = sourse.Orders.FirstOrDefault(recOder => recOder.Id == model.Id);
            return order != null ? CreateModel(order) : null;
        }
        public void Insert(OrderBindingModel model)
        {
            int maxId = sourse.Orders.Count > 0 ? sourse.Orders.Max(recOder => recOder.Id) : 0;
            var order = new Order { Id = maxId + 1 };
            sourse.Orders.Add(CreateModel(model, order));
        }
        public void Update(OrderBindingModel model)
        {
            var order = sourse.Orders.FirstOrDefault(recOder => recOder.Id == model.Id);
            if (order == null)
            {
                throw new Exception("Заказ не найден");
            }
            CreateModel(model, order);
        }
        public void Delete(OrderBindingModel model)
        {
            var order = sourse.Orders.FirstOrDefault(recOrder => recOrder.Id == model.Id);

            if (order != null)
            {
                sourse.Orders.Remove(order);
            }
            else
            {
                throw new Exception("Заказ не найден");
            }
        }
    }
}
