using System.ComponentModel.DataAnnotations;

namespace Mango.Services.ProductAPI.Models
{
    public class ProductReview
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public string ReviewMessage { get; set; }
    }
}
