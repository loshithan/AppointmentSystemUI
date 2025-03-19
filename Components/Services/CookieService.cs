using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace AppointmentUI.Components.Services
{
    public class CookieService(IJSRuntime jsRuntime)
    {
        private readonly IJSRuntime _jsRuntime = jsRuntime;

        // Method to set a cookie
        public async Task SetCookieAsync(string name, string value, int days)
        {
            await _jsRuntime.InvokeVoidAsync("setCookie", name, value, days);
        }

        // Method to get a cookie
        public async Task<string> GetCookieAsync(string name)
        {
            return await _jsRuntime.InvokeAsync<string>("getCookie", name);
        }

        // Method to delete a cookie
        public async Task DeleteCookieAsync(string name)
        {
            await _jsRuntime.InvokeVoidAsync("deleteCookie", name);
        }
    }
}