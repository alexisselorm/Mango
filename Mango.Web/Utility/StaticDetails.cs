namespace Mango.Web.Utility
{
    public class StaticDetails
    {
        public static string APIBaseUrl { get; set; }
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
