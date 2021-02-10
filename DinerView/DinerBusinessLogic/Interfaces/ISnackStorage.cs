using System;
using System.Collections.Generic;
using System.Text;
using DinerBusinessLogic.BindingModels;
using DinerBusinessLogic.ViewModels;

namespace DinerBusinessLogic.Interfaces
{
    public interface ISnackStorage
    {
        List<SnackViewModel> GetFullList();
        List<SnackViewModel> GetFilteredList(SnackBindingModel model);
        SnackViewModel GetElement(SnackBindingModel model);
        void Insert(SnackBindingModel model);
        void Update(SnackBindingModel model);
        void Delete(SnackBindingModel model);
    }
}
