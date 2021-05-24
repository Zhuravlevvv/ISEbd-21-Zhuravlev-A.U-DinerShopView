using System;
using System.Collections.Generic;
using DinerBusinessLogic.BindingModels;
using DinerBusinessLogic.Enums;
using DinerBusinessLogic.Interfaces;
using DinerBusinessLogic.ViewModels;
using DinerBusinessLogic.HelperModels;

namespace DinerBusinessLogic.BusinessLogics
{
    public class OrderLogic
    {
        private readonly IOrderStorage _orderStorage;
        private readonly IClientStorage _clientStorage;
        private readonly object locker = new object();
        private readonly IStoreHouseStorage _storehouseStorage;
        public OrderLogic(IOrderStorage orderStorage, IClientStorage clientStorage, IStoreHouseStorage storeHouseStorage)
        {
            _orderStorage = orderStorage;
            _clientStorage = clientStorage;
            _storehouseStorage = storeHouseStorage;
        }
        public List<OrderViewModel> Read(OrderBindingModel model)
        {
            if (model == null)
            {
                return _orderStorage.GetFullList();
            }

            if (model.Id.HasValue)
            {
                return new List<OrderViewModel> { _orderStorage.GetElement(model) };
            }
            return _orderStorage.GetFilteredList(model);
        }
        public void CreateOrder(CreateOrderBindingModel model)
        {
            _orderStorage.Insert(new OrderBindingModel
            {
                SnackId = model.SnackId,
                Count = model.Count,
                Sum = model.Sum,
                DateCreate = DateTime.Now,
                Status = OrderStatus.Принят,
                ClientId = model.ClientId
            });

            MailLogic.MailSendAsync(new MailSendInfo
            {
                MailAddress = _clientStorage.GetElement(new ClientBindingModel
                {
                    Id = model.ClientId
                })?.Email,
                Subject = $"Новый заказ",
                Text = $"Заказ от {DateTime.Now} на сумму {model.Sum:N2} принят."
            });
        }
        public void TakeOrderInWork(ChangeStatusBindingModel model)
        {
            lock (locker)
            {
                var order = _orderStorage.GetElement(new OrderBindingModel
                {
                    Id = model.OrderId
                });

                if (order == null)
                {
                    throw new Exception("Не найден заказ!");
                }

                if (order.Status != OrderStatus.Принят)
                {
                    throw new Exception("Заказ не в статусе \"Принят\"");
                }
                if (order.ImplementerId.HasValue)
                {
                    throw new Exception("У заказа уже есть исполнитель");
                }
                OrderBindingModel orderModel = new OrderBindingModel
                {
                    Id = order.Id,
                    SnackId = order.SnackId,
                    ImplementerId = model.ImplementerId,
                    Count = order.Count,
                    Sum = order.Sum,
                    DateCreate = order.DateCreate,
                    DateImplement = DateTime.Now,
                    Status = OrderStatus.Выполняется,
                    ClientId = order.ClientId
                };
                if (!_storehouseStorage.CheckAndTake(order.SnackId, order.Count))
                {
                    orderModel.Status = OrderStatus.ТребуютсяMатериалы;
                    orderModel.ImplementerId = null;
                }
                _orderStorage.Update(orderModel);

                MailLogic.MailSendAsync(new MailSendInfo
                {
                    MailAddress = _clientStorage.GetElement(new ClientBindingModel
                    {
                        Id = order.ClientId
                    })?.Email,
                    Subject = $"Заказ №{order.Id}",
                    Text = $"Заказ №{order.Id} на сумму {order.Sum} передан в работу."
                });
            }            
        }
        public void FinishOrder(ChangeStatusBindingModel model)
        {
            var order = _orderStorage.GetElement(new OrderBindingModel { 
            Id = model.OrderId });

            if (order == null)
            {
                throw new Exception("Не найден заказ!");
            }
            if (order.Status != OrderStatus.Выполняется)
            {
                throw new Exception("Заказ не в статусе \"Выполняется\"");
            }
            _orderStorage.Update(new OrderBindingModel
            {
                Id = order.Id,
                SnackId = order.SnackId,
                Count = order.Count,
                Sum = order.Sum,
                DateCreate = order.DateCreate,
                DateImplement = order.DateImplement,
                Status = OrderStatus.Готов,
                ClientId = order.ClientId,
                ImplementerId = model.ImplementerId
            });

            MailLogic.MailSendAsync(new MailSendInfo
            {
                MailAddress = _clientStorage.GetElement(new ClientBindingModel
                {
                    Id = order.ClientId
                })?.Email,
                Subject = $"Заказ №{order.Id}",
                Text = $"Заказ №{order.Id} на сумму {order.Sum} выполнен."
            });
        }
        public void PayOrder(ChangeStatusBindingModel model)
        {
            var order = _orderStorage.GetElement(new OrderBindingModel { 
            Id = model.OrderId });
            if (order == null)
            {
                throw new Exception("Не найден заказ!");
            }
            if (order.Status != OrderStatus.Готов)
            {
                throw new Exception("Заказ не в статусе \"Готов\"");
            }
            _orderStorage.Update(new OrderBindingModel
            {
                Id = order.Id,
                SnackId = order.SnackId,
                Count = order.Count,
                Sum = order.Sum,
                DateCreate = order.DateCreate,
                DateImplement = order.DateImplement,
                Status = OrderStatus.Оплачен,
                ClientId = order.ClientId
            });

            MailLogic.MailSendAsync(new MailSendInfo
            {
                MailAddress = _clientStorage.GetElement(new ClientBindingModel
                {
                    Id = order.ClientId
                })?.Email,
                Subject = $"Заказ №{order.Id}",
                Text = $"Заказ №{order.Id} оплачен."
            });
        }
    }
}
