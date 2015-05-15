using System;
using Android.App;
using Android.Content;
using Android.Speech;

namespace SpeechTest1
{
	public class SpeechRecognizerActivityHelper
	{
		const int VOICE = 293874;
		readonly Activity activity;

		public SpeechRecognizerActivityHelper(Activity activity) {
			this.activity = activity;
		}

		public void StartActivityForResult () {
			var voiceIntent = new Intent (RecognizerIntent.ActionRecognizeSpeech);
			voiceIntent.PutExtra (RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
			voiceIntent.PutExtra (RecognizerIntent.ExtraPrompt, Application.Context.GetString (Resource.String.messageSpeakNow));
			voiceIntent.PutExtra (RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
			voiceIntent.PutExtra (RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);
			voiceIntent.PutExtra (RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
			voiceIntent.PutExtra (RecognizerIntent.ExtraMaxResults, 1);
			voiceIntent.PutExtra (RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);

			activity.StartActivityForResult (voiceIntent, VOICE);
		}

		public string HandleActivityResult(int requestCode, Result resultVal, Intent data) {
			string recognizedText = string.Empty;

			if (requestCode == VOICE)
			{
				if (resultVal == Result.Ok)
				{
					var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
					if (matches.Count != 0)
					{
						foreach (var item in matches) {
							recognizedText += "\n" + item;
						}
					}
				}
			}

			return recognizedText;
		}
	}
}

