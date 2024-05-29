#if ANDROID

using Android.App;
using Android.Content;
using static AndroidX.Activity.Result.Contract.ActivityResultContracts;
using AndroidApp = Android.App.Application;



namespace MAUIApplication
{
    public class WebActions
    {
        public static WebActions Instance { get; } = new WebActions();

        public WebActions() { }

        public async void getCommandFromWeb(string command,IWebActionCallback webActionCallback)
        {
            string answer;
            if (command.Length == 13 && command[..13] == "call-calendar")
            {
                 answer = "open calendar";
                    MainActivity.androidAppCall(Intent.CategoryAppCalendar);
                

            }
            else if (command.Length == 12 && command[..12] == "call-gallery")
            {
                answer = "open gallery";
                MainActivity.androidAppCall(Intent.CategoryAppGallery);

            }
            else if (command.Length == 10 && command[..10] == "call-music")
            {
                  answer = "open music";
                MainActivity.androidAppCall(Intent.CategoryAppMusic);
            }
            else if (command.Length == 11 && command[..11] == "call-market")
            {
                answer = "open market";
                MainActivity.androidAppCall(Intent.CategoryAppMarket);
            }
            else if (command.Length == 12 && command[..12] == "call-browser")
            {
                answer = "open browser";
                MainActivity.androidAppCall(Intent.CategoryAppBrowser);
            }
            else if (command.Length == 14 && command[..14] == "call-get-image")
            {
                answer = "no photo return";
                MainActivity.androidGalleryImageCallAsync();
                answer = MainActivity.answer;
            }
            else
            {
                answer = "no activity";
            }
            webActionCallback.SendAnswerToHtml(answer);
        }

        




    }
}
#endif