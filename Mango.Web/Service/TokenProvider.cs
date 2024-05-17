using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public TokenProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public void clearToken()
        {
            _contextAccessor.HttpContext?.Response.Cookies.Delete(StaticDetails.TokenCookie);
        }

        public string? getToken()
        {
            string? token = null;
            bool? hasToken = _contextAccessor.HttpContext?.Request.Cookies.TryGetValue(StaticDetails.TokenCookie, out token);
            return hasToken is true ? token : null;
        }

        public void setToken(string token)
        {
            _contextAccessor.HttpContext?.Response.Cookies.Append(StaticDetails.TokenCookie, token);
        }
    }
}