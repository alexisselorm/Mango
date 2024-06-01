namespace Mango.Web.Models
{
    public class CartDTO
    {
        public CartHeader? CartHeader { get; set; }
        public IEnumerable<CartDetailsDTO?> CartDetails { get; set; }
    }
}
