using AltairCA.Blazor.WebAssembly.Cookie.Models;

namespace AltairCA.Blazor.WebAssembly.Cookie
{
    public interface IAltairCABlazorCookieUtil
    {
        /// <summary>
        /// Set a object in the cookie
        /// </summary>
        /// <param name="key">The key for the cookie (name)</param>
        /// <param name="value">Cookie value</param>
        /// <param name="span">TimeSpan that will be set to the 'expires' attribute value</param>
        /// <param name="path">Path in the request url which must exist for the cookie to be sent in requests </param>
        /// <param name="domain">The host to which the cookie will be sent</param>
        /// <param name="secure">Specifies that the cookie will be sent only over secure protocols</param>
        /// <param name="isSession">Flags the cookie as a session cookie (temporal) by setting the 'expires' attribute value to ''</param>
        /// <param name="partitioned">Requires that the browser has activated partitioned cookies</param>
        /// <param name="maxAgeInSeconds">Maximum age in seconds</param> 
        /// <returns></returns>
        Task SetValueAsync(string key, object value, TimeSpan? span = null, string? path = null,
            string? domain = null, bool? secure = null, SameSite? sameSite = null, bool? partitioned = null, bool? isSession = null,
            int? maxAgeInSeconds = null);

        /// <summary>
        /// Set a string in the cookie
        /// </summary>
        /// <param name="key">The key for the cookie (name)</param>
        /// <param name="value">Cookie value</param>
        /// <param name="span">TimeSpan that will be set to the 'expires' attribute value</param>
        /// <param name="path">Path in the request url which must exist for the cookie to be sent in requests </param>
        /// <param name="domain">The host to which the cookie will be sent</param>
        /// <param name="secure">Specifies that the cookie will be sent only over secure protocols</param>
        /// <param name="isSession">Flags the cookie as a session cookie (temporal) by setting the 'expires' attribute value to ''</param>
        /// <param name="partitioned">Requires that the browser has activated partitioned cookies</param>
        /// <param name="maxAgeInSeconds">Maximum age in seconds</param> 
        /// <returns></returns>
        Task SetValueAsync(string key, string value, TimeSpan? span = null, string? path=null, string? domain=null, 
            bool? secure = null, SameSite? sameSite = null, bool? partitioned = null, bool? isSession = null,
            int? maxAgeInSeconds = null);
       
        Task<string> GetValueAsync(string key);
       
        Task<T> GetValueAsync<T>(string key) where T : class;
        
        Task RemoveAsync(string key, string? path = null);

    }
}