using System;
using System.Collections.Generic;

using Android.Speech.Tts;
using Android.Content;

namespace SpeechTest1
{
	public class TextToSpeech_Android : Java.Lang.Object, TextToSpeech.IOnInitListener
	{
		class MyUtteranceProgressListener : UtteranceProgressListener {

			public event EventHandler<EventArgs> Done;
			public event EventHandler<EventArgs> Error;
			public event EventHandler<EventArgs> Start;

			public override void OnDone (string utteranceId)
			{
				if (Done != null)
					Done (this, EventArgs.Empty);
			}

			public override void OnError (string utteranceId)
			{
				if (Error != null)
					Error (this, EventArgs.Empty);
			}

			public override void OnStart (string utteranceId)
			{
				if (Error != null)
					Start (this, EventArgs.Empty);
			}
		}

		TextToSpeech speaker;
		MyUtteranceProgressListener listener;
		bool ready;
		int count = 0;
		string utteranceText;
		string utteranceID;

		public event EventHandler<EventArgs> Done;
		public event EventHandler<EventArgs> Start;

		public TextToSpeech_Android (Context context) {
			ready = false;

			listener = new MyUtteranceProgressListener ();
			listener.Start += (o, e) => {
				if (Start != null)
					Start (this, EventArgs.Empty);
			};
			listener.Done += (o, e) => {
				if (Done != null)
					Done (this, EventArgs.Empty);
			};
			listener.Error += (o, e) => {
				if (Done != null)
					Done (this, EventArgs.Empty);
			};

			speaker = new TextToSpeech (context, this);
		}

		public void Shutdown () {
			if (speaker != null) {
				ready = false;
				speaker.Stop ();
				speaker.Shutdown ();
			}
		}

		public void Speak (string text)
		{
			utteranceText = text;
			utteranceID = this.GetHashCode ().ToString () + (++count).ToString ();

			if (ready) {
				speaker.Speak (utteranceText, QueueMode.Flush, null, utteranceID);
			}
		}

		#region IOnInitListener implementation
		public void OnInit (OperationResult status)
		{
			if (status.Equals (OperationResult.Success)) {
				speaker.SetOnUtteranceProgressListener (listener);
				speaker.Speak (utteranceText, QueueMode.Flush, null, utteranceID);
				ready = true;
			} 
		}
		#endregion
	}
}
