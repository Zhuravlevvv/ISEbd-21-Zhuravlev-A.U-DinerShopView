using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DinerBusinessLogic.Enums;
using System;


namespace DinerViewDatabaseImplement.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int SnackId { get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        public decimal Sum { get; set; }
        [Required]
        public OrderStatus Status { get; set; }
        [Required]
        public DateTime DateCreate { get; set; }
        public DateTime? DateImplement { get; set; }
        public virtual Snack Snack { get; set; }
    }
}
