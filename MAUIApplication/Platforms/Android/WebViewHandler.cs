using Android.Graphics;
using Android.Webkit;
using Java.Interop;
using Microsoft.Maui.Handlers;
using Android.Content;
using AndroidApp = Android.App.Application;


namespace MAUIApplication
{
    public partial class WebViewHandler : ViewHandler<Microsoft.Maui.Controls.WebView, Android.Webkit.WebView>
    {
        protected override partial Android.Webkit.WebView CreatePlatformView()
        {
            var webView = new Android.Webkit.WebView(Context);
            webView.Settings.JavaScriptEnabled = true;
            webView.SetWebViewClient(new JavascriptWebViewClient($@"
 
                function getCommandFromWeb(data) {{
                    jsBridge.getCommandFromWeb(data);
                }}
 
            "));
            webView.AddJavascriptInterface(new JSBridge(webView), "jsBridge");
            return webView;
        }

        protected override partial void ConnectHandler(Android.Webkit.WebView webView)
        {
            base.ConnectHandler(webView);
        }

        protected override partial void DisconnectHandler(Android.Webkit.WebView webView)
        {
            webView.Dispose();
            base.DisconnectHandler(webView);
        }

        private static partial void LoadSource(WebViewHandler webViewHandler, Microsoft.Maui.Controls.WebView webView)
        {
            if (webView.Source is HtmlWebViewSource html)
            {
                webViewHandler.PlatformView.LoadData(html.Html, null, null);
            }
            else if (webView.Source is UrlWebViewSource url)
            {
                webViewHandler.PlatformView.LoadUrl(url.Url);
            }
        }
    }

    public class JavascriptWebViewClient(string script) : WebViewClient
    {
        public override void OnPageStarted(Android.Webkit.WebView? webView, string? url, Bitmap? favicon)
        {
            base.OnPageStarted(webView, url, favicon);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                webView?.EvaluateJavascript(script, null);
            });
        }
    }

    public class JSBridge(Android.Webkit.WebView webView) : Java.Lang.Object, IWebActionCallback
    {

        [JavascriptInterface]
        [Export("getCommandFromWeb")]
        public void getCommandFromWeb(string command)
        {
            WebActions.Instance.getCommandFromWeb(command, this);
        }


        public async void SendAnswerToHtml<T>(T answer)
        {
                await Task.Run(() =>
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        webView?.EvaluateJavascript($@"
                        getAnswerFromMaui('{answer}');
                    ", null);
                    });
                });
        }
    }
}
