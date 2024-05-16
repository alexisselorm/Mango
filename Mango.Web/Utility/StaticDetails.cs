namespace Mango.Web.Utility
{
    public class StaticDetails
    {
        public static string CouponAPIUrl { get; set; }
        public static string AuthAPIUrl { get; set; }
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
