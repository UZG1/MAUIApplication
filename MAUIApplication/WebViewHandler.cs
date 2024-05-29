using Microsoft.Maui.Handlers;

#if IOS || MACCATALYST

using PlatformWebView = WebKit.WKWebView;

#elif ANDROID

using PlatformWebView = Android.Webkit.WebView;

#elif WINDOWS

using PlatformWebView = Microsoft.UI.Xaml.Controls.WebView2;

#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID)

using PlatformWebView = System.Object;

#endif 

namespace MAUIApplication
{
    public partial class WebViewHandler : ViewHandler<WebView, PlatformWebView>
    {
        public static PropertyMapper<WebView, WebViewHandler> WebViewPropertyMapper = new(ViewMapper)
        {
            [nameof(WebView.Source)] = LoadSource
        };

        public WebViewHandler()
            : base(WebViewPropertyMapper)
        {
        }

        protected override partial PlatformWebView CreatePlatformView();

        protected override partial void ConnectHandler(PlatformWebView webView);

        protected override partial void DisconnectHandler(PlatformWebView webView);

        private static partial void LoadSource(WebViewHandler webViewHandler, WebView webView);
    }
}
