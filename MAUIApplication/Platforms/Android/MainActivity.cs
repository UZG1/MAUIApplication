using Android.App;
using Android.Content;
using Android.Content.PM;
using AndroidApp = Android.App.Application;

namespace MAUIApplication
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        public static Android.Net.Uri image;
        private static bool isFinished;
        public static string answer;

        public static void androidAppCall(String app)
        {
            try
            {
                var context = AndroidApp.Context;
                var calendarIntent = new Intent(Intent.ActionMain);
                calendarIntent.AddCategory(app);
                calendarIntent.AddFlags(ActivityFlags.NewTask);
                context.StartActivity(calendarIntent);
            }
            catch (Exception ex)
            {

            }
        }

        public static async Task<string> androidGalleryImageCallAsync()
        {
            try
            {
                var context = AndroidApp.Context;
                var calendarIntent = new Intent();
                calendarIntent.SetType("image/*");
                calendarIntent.SetAction(Intent.ActionGetContent);

                // Ensure the MainActivity is running
                var mainActivity = Platform.CurrentActivity ?? throw new InvalidOperationException("MainActivity is not running.");
                Console.WriteLine("StartActivityForResult");
                // Use MainActivity to start the activity
                isFinished = false;
                mainActivity.StartActivityForResult(calendarIntent, 1);
                while(!isFinished)
                {
                    Task.Delay(100);
                }

                return image.ToString();
            }
            catch (Exception ex)
            {
                // Handle exception
            }
            return null;
        }

        public static async Task<string> GetFile(Context context, Android.Net.Uri contentUri)
        {

            var resolver = context.ContentResolver;
            try
            {
                var stream = resolver.OpenInputStream(contentUri);
                if (stream != null)
                {

                    byte[] byteImage = ReadFully(stream);
                    Console.WriteLine();
                    string base64Image = Convert.ToBase64String(byteImage);
                    Console.WriteLine("IMAGE КАРТИНКА" + base64Image);
                    return base64Image;
                }

            }

            catch (Exception ex)
            {
                // Handle exception
            }
            return null;
        }

        public static byte[] ReadFully(System.IO.Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                Console.WriteLine("Stream Байтов#"+ms.ToArray().ToString());
                return ms.ToArray();
            }
        }

        protected override async void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            Console.WriteLine("OnActivityResult");

            if (requestCode == 1)
            {
                Console.WriteLine("resultCode check");

                if (resultCode == Result.Ok)
                {
                    Console.WriteLine("Get the Uri of the selected file");

                    // Get the Uri of the selected file
                    var uri = data.Data;
                    string base64Image = await GetFile(AndroidApp.Context, uri);
                    answer = base64Image;
                    isFinished = true;
                }

            }
        }
    }
}
