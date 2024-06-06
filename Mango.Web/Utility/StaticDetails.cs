namespace Mango.Web.Utility
{
    public class StaticDetails
    {
        public static string CouponAPIUrl { get; set; }
        public static string AuthAPIUrl { get; set; }
        public static string ProductAPIUrl { get; set; }
        public static string OrderAPIUrl { get; set; }
        public static string ShoppingCartAPIUrl { get; set; }
        public const string RoleAdmin = "ADMINISTRATOR";
        public const string RoleCustomer = "CUSTOMER";
        public const string TokenCookie = "JwtToken";
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }

        public const string Status_Pending = "Pending";
        public const string Status_Approved = "Approved";
        public const string Status_ReadyForPickup = "ReadyForPickup";
        public const string Status_Completed = "Completed";
        public const string Status_Refunded = "Refunded";
        public const string Status_Cancelled = "Cancelled";
    }
}
