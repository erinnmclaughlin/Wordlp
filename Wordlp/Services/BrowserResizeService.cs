using Microsoft.JSInterop;

namespace Wordlp.Services;

/* https://github.com/chrissainty/BlazorBrowserResize/blob/master/BrowserResize/BrowserResize/BrowserResizeService.cs */
public class BrowserResizeService
{
    private IJSRuntime Js { get; }

    public static event Func<Task>? OnResize;

    public BrowserResizeService(IJSRuntime js)
    {
        Js = js;
    }

    [JSInvokable]
    public static async Task OnBrowserResize()
    {
        if (OnResize != null)
            await OnResize.Invoke();
    }

    public async Task<int> GetInnerHeight()
    {
        return await Js.InvokeAsync<int>("getInnerHeight");
    }
}
