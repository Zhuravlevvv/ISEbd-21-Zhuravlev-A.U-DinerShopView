using DinerBusinessLogic.BindingModels;
using DinerBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace DinerBusinessLogic.Interfaces
{
    public interface IMessageInfoStorage
    {
        List<MessageInfoViewModel> GetFullList();

        List<MessageInfoViewModel> GetFilteredList(MessageInfoBindingModel model);

        void Insert(MessageInfoBindingModel model);      
    }
}
