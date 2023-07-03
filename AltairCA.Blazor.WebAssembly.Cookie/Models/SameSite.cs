using System.ComponentModel;

namespace AltairCA.Blazor.WebAssembly.Cookie.Models;

public enum SameSite {
    
    [Description("lax")]
    Lax = 0,

    [Description("strict")]
    Strict = 1,

    [Description("none")]
    None = 2
    
}