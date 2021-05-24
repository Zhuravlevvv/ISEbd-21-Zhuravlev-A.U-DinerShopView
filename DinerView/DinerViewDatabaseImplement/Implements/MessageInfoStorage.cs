using System;
using System.Collections.Generic;
using DinerBusinessLogic.BindingModels;
using DinerBusinessLogic.Interfaces;
using DinerBusinessLogic.ViewModels;
using DinerViewDatabaseImplement.Models;
using System.Linq;

namespace DinerViewDatabaseImplement.Implements
{
    public class MessageInfoStorage : IMessageInfoStorage
    {
        public List<MessageInfoViewModel> GetFullList()
        {
            using (var context = new DinerViewDatabase())
            {
                return context.MessageInfoes
                    .Select(rec => new MessageInfoViewModel
                    {
                        MessageId = rec.MessageId,
                        SenderName = rec.SenderName,
                        DateDelivery = rec.DateDelivery,
                        Subject = rec.Subject,
                        Body = rec.Body
                    })
                    .ToList();
            }
        }

        public List<MessageInfoViewModel> GetFilteredList(MessageInfoBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new DinerViewDatabase())
            {
                return context.MessageInfoes
                    .Where(rec => (model.ClientId.HasValue && rec.ClientId == model.ClientId) ||
                    (!model.ClientId.HasValue && rec.DateDelivery.Date == model.DateDelivery.Date))
                    .Select(rec => new MessageInfoViewModel
                    {
                        MessageId = rec.MessageId,
                        SenderName = rec.SenderName,
                        DateDelivery = rec.DateDelivery,
                        Subject = rec.Subject,
                        Body = rec.Body
                    })
                    .ToList();
            }
        }

        public void Insert(MessageInfoBindingModel model)
        {
            using (var context = new DinerViewDatabase())
            {
                MessageInfo element = context.MessageInfoes.FirstOrDefault(rec => rec.MessageId == model.MessageId);
                if (element != null)
                {
                    return;
                }

                context.MessageInfoes.Add(new MessageInfo
                {
                    MessageId = model.MessageId,
                    ClientId = model.ClientId,
                    SenderName = model.FromMailAddress,
                    DateDelivery = model.DateDelivery,
                    Subject = model.Subject,
                    Body = model.Body
                });
                context.SaveChanges();
            }
        }
        public int Count()
        {
            using (var context = new DinerViewDatabase())
            {
                return context.MessageInfoes.Count();
            }
        }

        public List<MessageInfoViewModel> GetMessagesForPage(MessageInfoBindingModel model)
        {
            using (var context = new DinerViewDatabase())
            {
                return context.MessageInfoes.Where(rec => (model.ClientId.HasValue &&
                model.ClientId.Value == rec.ClientId) || !model.ClientId.HasValue)
                    .Skip((model.Page.Value - 1) * model.PageSize.Value).Take(model.PageSize.Value)
                    .ToList().Select(rec => new MessageInfoViewModel
                    {
                        MessageId = rec.MessageId,
                        SenderName = rec.SenderName,
                        DateDelivery = rec.DateDelivery,
                        Subject = rec.Subject,
                        Body = rec.Body
                    }).ToList();
            }
        }
    }
}
