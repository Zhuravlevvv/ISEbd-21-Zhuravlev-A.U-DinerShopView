using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using DinerBusinessLogic.Attributes;

namespace DinerBusinessLogic.ViewModels
{
    [DataContract]
    public class SnackViewModel
    {
        [DataMember]
        [Column(title: "Номер", width: 100, visible: false)]
        public int Id { get; set; }
        [DataMember]
        [Column(title: "Название закуски", gridViewAutoSize: GridViewAutoSize.Fill)]
        public string SnackName { get; set; }
        [DataMember]
        [Column(title: "Цена", width: 100)]
        public decimal Price { get; set; }
        [DataMember]
        public Dictionary<int, (string, int)> SnackFoods { get; set; }
        
    }
}
