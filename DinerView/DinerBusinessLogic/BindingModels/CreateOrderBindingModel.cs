using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace DinerBusinessLogic.BindingModels
{
    //Данные от клиента, для создания заказа
    [DataContract]
    public class CreateOrderBindingModel
    {
        [DataMember]
        public int ClientId { get; set; }
        [DataMember]
        public int SnackId { get; set; }
        [DataMember]
        public int Count { get; set; }
        [DataMember]
        public decimal Sum { get; set; }
    }
}
