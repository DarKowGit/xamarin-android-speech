
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;

namespace SpeechTest1
{
	[Activity (Label = "SpeechTest1", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity	{
		TextView textResult;
		TextToSpeech_Android speaker;
		SpeechRecognizerActivityHelper listener;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			textResult = FindViewById<TextView> (Resource.Id.textView1);
			listener = new SpeechRecognizerActivityHelper (this);
		}

		protected override void OnStart ()
		{
			speaker = new TextToSpeech_Android (Application.ApplicationContext);
			speaker.Done += (sender, e) => listener.StartActivityForResult ();
			speaker.Speak ("Tell me something.");

			base.OnStart ();
		}

		protected override void OnStop ()
		{
			if (speaker != null) {
				speaker.Shutdown ();
				speaker = null;
			}

			base.OnStop ();
		}

		protected override void OnActivityResult(int requestCode, Result resultVal, Intent data)
		{
			string result = listener.HandleActivityResult (requestCode, resultVal, data);

			if (!string.IsNullOrEmpty (result)) {
				textResult.Text = result;

				if (null != speaker)
					speaker.Speak (result);
			}
			else
				textResult.Text = "No speech was recognised";

			base.OnActivityResult(requestCode, resultVal, data);
		}
	}
}


