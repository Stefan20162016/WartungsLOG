using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;


/// ATTENTION EVERYONE: CHANGE 1080 and 1920 values below App.ScreenWidth/App.ScreenHeight, pan outside
namespace WartungsLOG.Utils
{
	public class PanContainer : ContentView
	{
		double x, y;

		public PanContainer()
		{
			Console.WriteLine("XXXX: in PanContainer constructor()");
			// Set PanGestureRecognizer.TouchPoints to control the 
			// number of touch points needed to pan
			var panGesture = new PanGestureRecognizer();
			panGesture.PanUpdated += OnPanUpdated;
			GestureRecognizers.Add(panGesture);
		}

		void OnPanUpdated(object sender, PanUpdatedEventArgs e)
		{
			var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
			var width = mainDisplayInfo.Width;
			var height = mainDisplayInfo.Height;
			Console.WriteLine("XXXX: OnPAN: w:" + width + " height: " + height);
			switch (e.StatusType)
			{

				case GestureStatus.Running:
					// Translate and ensure we don't pan beyond the wrapped user interface element bounds.
					Content.TranslationX = Math.Max(Math.Min(0, x + e.TotalX), -Math.Abs(Content.Width - width));
					Content.TranslationY = Math.Max(Math.Min(0, y + e.TotalY), -Math.Abs(Content.Height - height));
					break;

				case GestureStatus.Completed:
					// Store the translation applied during the pan
					x = Content.TranslationX;
					y = Content.TranslationY;
					break;
			}
		}
	}
}
