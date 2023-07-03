using AltairCA.Blazor.WebAssembly.Cookie.Models;

namespace AltairCA.Blazor.WebAssembly.Cookie
{
    public interface IAltairCABlazorCookieUtil
    {
        /// <summary>
        /// Set a object in the cookie
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="span"></param>
        /// <param name="path"></param>
        /// <param name="domain"></param>
        /// <param name="secure"></param>
        /// <returns></returns>
        Task SetValueAsync(string key, object value, TimeSpan? span = null, string? path = null,
            string? domain = null, bool? secure = null, SameSite? sameSite = null);
        /// <summary>
        /// Set a string in the cookie
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="span"></param>
        /// <param name="path"></param>
        /// <param name="domain"></param>
        /// <param name="secure"></param
        /// <returns></returns>
        Task SetValueAsync(string key, string value, TimeSpan? span = null, string? path=null, string? domain=null, bool? secure = null, SameSite? sameSite = null);
       
        Task<string> GetValueAsync(string key);
       
        Task<T> GetValueAsync<T>(string key) where T : class;
        
        Task RemoveAsync(string key, string? path = null);

    }
}