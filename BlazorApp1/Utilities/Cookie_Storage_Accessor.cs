using Microsoft.JSInterop;

namespace BlazorApp1.Utilities
{
    public class Cookie_Storage_Accessor
    {
        private Lazy<IJSObjectReference> _accessorJsRef = new();
        private readonly IJSRuntime _jsRuntime;

        public Cookie_Storage_Accessor(IJSRuntime jSRuntime)
        {
            _jsRuntime = jSRuntime;
        }

        // this imports the js file to make the cookies
        private async Task Wait_For_Reference()
        {
            if (_accessorJsRef.IsValueCreated is false)
            {
                _accessorJsRef = new(await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "/js/Cookie_Storage_Accessor.js"));
            }
        }
        
        // this would Dispose of the cookie isn't use right now
        public async ValueTask Dispose_Async()
        {
            if (_accessorJsRef.IsValueCreated)
            {
                await _accessorJsRef.Value.DisposeAsync();
            }
        }

        // this woud get the cookie isn't used as of now
        public async Task<T> Get_Value_Async<T>(string key)
        {
            await Wait_For_Reference();

            var result = await _accessorJsRef.Value.InvokeAsync<T>("Get", key);

            return result;
        }

        // this will set the cookie
        public async Task Set_Value_Async<T>(string key, T value)
        {
            await Wait_For_Reference();
            await _accessorJsRef.Value.InvokeVoidAsync("Set", key, value);
        }

        // this will delete the Cookie with the js File
        public async Task Delete_Value_Async(string key)
        {
            await Wait_For_Reference();
            await _accessorJsRef.Value.InvokeVoidAsync("Delete", key);
        }
    }
}
