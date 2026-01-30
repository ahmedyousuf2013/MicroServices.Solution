using System.ComponentModel.DataAnnotations.Schema;
//using Producer.Common.QueueProvider;

namespace Stock.Service.Domain.Entities
{
    [Table("ORDERS")]
    public class Order
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
