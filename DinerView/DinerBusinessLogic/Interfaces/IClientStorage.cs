using DinerBusinessLogic.BindingModels;
using DinerBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace DinerBusinessLogic.Interfaces
{
    public interface IClientStorage
    {
        List<ClientViewModel> GetFullList();
        List<ClientViewModel> GetFilteredList(ClientBindingModel model);
        ClientViewModel GetElement(ClientBindingModel model);

        void Insert(ClientBindingModel model);
        void Update(ClientBindingModel model);
        void Delete(ClientBindingModel model);
    }
}
