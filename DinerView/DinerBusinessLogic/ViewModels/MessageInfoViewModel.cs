using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using DinerBusinessLogic.Attributes;

namespace DinerBusinessLogic.ViewModels
{
    [DataContract]
    public class MessageInfoViewModel
    {
        [DataMember]
        [Column(title: "Номер", width: 100, visible: false)]
        public string MessageId { get; set; }

        [Column(title: "Отправитель", width: 150)]
        [DataMember]
        public string SenderName { get; set; }

        [Column(title: "Дата письма", width: 100)]
        [DataMember]
        public DateTime DateDelivery { get; set; }

        [Column(title: "Заголовок", width: 100)]
        [DataMember]
        public string Subject { get; set; }

        [Column(title: "Текст", gridViewAutoSize: GridViewAutoSize.AllCells)]
        [DataMember]
        public string Body { get; set; }
    }
}
