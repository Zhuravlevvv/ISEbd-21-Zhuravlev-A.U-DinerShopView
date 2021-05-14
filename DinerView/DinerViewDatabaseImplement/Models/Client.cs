using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace DinerViewDatabaseImplement.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required]
        public string ClientFIO { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [ForeignKey("ClientId")]
        public List<Order> Order { get; set; }
    }
}
