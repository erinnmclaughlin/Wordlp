using Microsoft.JSInterop;

namespace Wordlp.Services.Display;

/* https://github.com/chrissainty/BlazorBrowserResize/blob/master/BrowserResize/BrowserResize/BrowserResizeService.cs */
public class BrowserResizeService
{
    public static event Func<Task>? OnResize;

    private IJSRuntime Js { get; }

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
