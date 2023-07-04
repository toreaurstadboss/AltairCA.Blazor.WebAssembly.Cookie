using System.ComponentModel;
using AltairCA.Blazor.WebAssembly.Cookie.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace AltairCA.Blazor.WebAssembly.Cookie
{

    internal class AltairCABlazorCookieUtil : IAltairCABlazorCookieUtil
    {
        readonly IJSRuntime JSRuntime;
        private readonly AltairCABlazorCookieConfigOptions _settings;
        public AltairCABlazorCookieUtil(IJSRuntime jsRuntime,IOptions<AltairCABlazorCookieConfigOptions> options)
        {
            JSRuntime = jsRuntime;
            _settings = options.Value;
        }

        public Task SetValueAsync(string key, object value, TimeSpan? span = null, string? path = null,
            string? domain = null, bool? secure = null, SameSite? sameSite = null, bool? partitioned = null, bool? isSession = null,
            int? maxAgeInSeconds = null)
        {
            return SetValueAsync(key, JsonConvert.SerializeObject(value), span, path, domain, secure, sameSite, partitioned,
                isSession);
        }
        public async Task SetValueAsync(string key, string value, TimeSpan? span = null, string? path=null, string? domain=null, 
            bool? secure = null, SameSite? sameSite = null, bool? partitioned = null, bool? isSession = null,
             int? maxAgeInSeconds = null)
        {
            if (string.IsNullOrWhiteSpace(path))
                path = _settings.Path;
            if (!span.HasValue)
                span = _settings.DefaultExpire;
            if (string.IsNullOrWhiteSpace(domain))
                domain = _settings.Domain;
            if (!secure.HasValue)
                secure = _settings.IsSecure;
            
            var curExp = span.HasValue && span.Value.Ticks > 0 && isSession != true && !maxAgeInSeconds.HasValue ?  DateToUTC(span.Value) : "";            
            
            List<string> keyvals = new List<string>();
            keyvals.Add($"{key}={value}");
            keyvals.Add($"expires={curExp}");
            keyvals.Add($"path={path}");
            if(!string.IsNullOrWhiteSpace(domain))
                keyvals.Add($"domain={domain}");
            if(secure.HasValue && secure.Value)
                keyvals.Add("secure");
            if (maxAgeInSeconds.HasValue && isSession != true)
            {
                keyvals.Add($"max-age={maxAgeInSeconds.Value}");
            }
            if (sameSite.HasValue){
                DescriptionAttribute desc = (DescriptionAttribute) typeof(SameSite).GetMember(sameSite.Value.ToString()).First().GetCustomAttributes(typeof(DescriptionAttribute), false).First();
                keyvals.Add($"samesite={desc.Description}");
            } 
            string cookieToSet = string.Join(";", keyvals);
            if (partitioned == true){
                cookieToSet += ";partitioned";
            }
            
            await SetCookie(cookieToSet);
        }

        public async Task RemoveAsync(string key,string path = null)
        {
            if (string.IsNullOrWhiteSpace(path))
                path = _settings.Path;
            List<string> keyvals = new List<string>();
            keyvals.Add($"{key}=");
            keyvals.Add($"Path={path}");
            keyvals.Add($"expires=Thu, 01 Jan 1970 00:00:01 GMT;");
            await SetCookie(string.Join(";", keyvals));
        }

        public async Task<T> GetValueAsync<T>(string key) where T : class
        {
            var res = await GetValueAsync(key);
            if (res == null)
                return default(T);
            return JsonConvert.DeserializeObject<T>(res);
        }
        public async Task<string> GetValueAsync(string key)
        {
            var cValue = await GetCookie();
            if (string.IsNullOrEmpty(cValue)) return null;                

            var vals = cValue.Split(';');
            foreach (var val in vals)
                if(!string.IsNullOrEmpty(val) && val.IndexOf('=') > 0)
                    if(val.Substring(0, val.IndexOf('=')).Trim().Equals(key, StringComparison.OrdinalIgnoreCase))
                        return val.Substring(val.IndexOf('=') + 1);
            return null;
        }

        private async Task SetCookie(string value)
        {
            await JSRuntime.InvokeVoidAsync("eval", $"document.cookie = \'{value}\'");
        }

        private async Task<string> GetCookie()
        {
            return await JSRuntime.InvokeAsync<string>("eval", $"document.cookie");
        }
        private static string DateToUTC(TimeSpan span) => DateTime.Now.Add(span).ToUniversalTime().ToString("R");
    }
}