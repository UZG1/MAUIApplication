using PlatformWebView = Microsoft.UI.Xaml.Controls.WebView2;
using Microsoft.Maui.Handlers;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
//using Microsoft.UI.Xaml.Controls;


namespace MAUIApplication
{
    public partial class WebViewHandler : ViewHandler<WebView, PlatformWebView>
    {
        static SynchronizationContext sync;

        protected override partial PlatformWebView CreatePlatformView()
        {
            
            sync = SynchronizationContext.Current;
            PlatformWebView webview = new PlatformWebView();
           
            webview.NavigationCompleted += async (s, e) =>
            {
                var jsBridge = new JSBridge();
                 webview.EnsureCoreWebView2Async();

                //call from C#
                await webview.CoreWebView2.ExecuteScriptAsync(@"
                    getTest('qweqwe');
                ");

                webview.CoreWebView2.AddHostObjectToScript("jsbridge", jsBridge);

            };

            return webview;

        }

        protected override partial void ConnectHandler(PlatformWebView webView)
        {
            base.ConnectHandler(webView);
        }

        protected override partial void DisconnectHandler(PlatformWebView webView)
        {
            base.DisconnectHandler(webView);
        }

        private static partial void LoadSource(WebViewHandler webViewHandler, WebView webView)
        {
            
            if (webViewHandler.PlatformView.CoreWebView2 == null)
            {
                webViewHandler.PlatformView.EnsureCoreWebView2Async().AsTask().ContinueWith((t) =>
                {
                    sync.Post((o) => LoadSource(webViewHandler, webView), null);
                });
            }
            else if (webView.Source is HtmlWebViewSource html)
            {
                webViewHandler.PlatformView.NavigateToString(html.Html);
            }
            else if (webView.Source is UrlWebViewSource url)
            {
                webViewHandler.PlatformView.NavigateToString(url.Url);
            }
            
        }

        [ClassInterface(ClassInterfaceType.AutoDual)]
        [ComVisible(true)]
        public class JSBridge() : IWebActionCallback
        {
            public string ExampleProperty { get; set; } = "Hello from native!";

            public void getCommandFromWeb(String command)
            {
                //Action
            }

            public void SendAnswerToHtml<T>(T answer)
            {
                //Send result
            }
        }




    }
}
