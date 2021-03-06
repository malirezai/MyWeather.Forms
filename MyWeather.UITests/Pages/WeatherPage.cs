﻿using System;
using System.Linq;
using System.Threading;

using Xamarin.UITest;

using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace MyWeather.UITests
{
	public class WeatherPage : BasePage
	{
		readonly Query ConditionLabel;
		readonly Query GetWeatherButton;
		readonly Query FeedbackButton;
		readonly Query LocationEntry;
		readonly Query TempLabel;
		readonly Query UseGPSSwitch;
		readonly Query GetWeatherActivityIndicator;

		public WeatherPage(IApp app, Platform platform) : base(app, platform, AutomationIdConstants.WeatherPageTitle)
		{
			ConditionLabel = x => x.Marked(AutomationIdConstants.ConditionLabel);
			GetWeatherButton = x => x.Marked(AutomationIdConstants.GetWeatherButton);
			LocationEntry = x => x.Marked(AutomationIdConstants.LocationEntry);
			TempLabel = x => x.Marked(AutomationIdConstants.TempLabel);
			UseGPSSwitch = x => x.Marked(AutomationIdConstants.UseGPSSwitch);
			GetWeatherActivityIndicator = x => x.Marked(AutomationIdConstants.GetWeatherActivityIndicator);

			if (OniOS)
				FeedbackButton = x => x.Marked(AutomationIdConstants.FeedbackButton);
			else
				FeedbackButton = x => x.Class("ActionMenuItemView");

			WaitForPageToLoad(20, this);
		}

		public string GetConditionText()
		{
			App.WaitForElement(ConditionLabel);
			return App.Query(ConditionLabel)?.FirstOrDefault()?.Text ?? string.Empty;
		}

		public string GetTemperatureText()
		{
			App.WaitForElement(TempLabel);
			return App.Query(TempLabel)?.FirstOrDefault()?.Text ?? string.Empty;
		}

		public void TapGetWeatherButton()
		{
			App.Tap(GetWeatherButton);
			App.Screenshot("Get Weather Button Tapped");
		}

		public void ToggleGPSSwitch()
		{
			App.Tap(UseGPSSwitch);
			App.Screenshot("GPS Switch Toggled");
		}

		public void EnterLocation(string location)
		{
			App.Tap(LocationEntry);
			App.ClearText();
			App.EnterText(location);
			App.DismissKeyboard();
			App.Screenshot($"Entered Location: {location}");
		}

		public void WaitForActivityIndicator(int timeoutInSeconds = 60)
		{
			App.WaitForElement(GetWeatherActivityIndicator, "Activity Indicator Never Appeared", TimeSpan.FromSeconds(timeoutInSeconds));
			App.Screenshot("Activity Indicator Appeared");
		}

		public void WaitForNoActivityIndicator(int timeoutInSeconds = 60)
		{
			App.WaitForNoElement(GetWeatherActivityIndicator, "Activity Indicator Never Disappeared", TimeSpan.FromSeconds(timeoutInSeconds));
			App.Screenshot("Activity Indicator Disappeared");
		}

		public bool IsWeatherPageVisible()
		{
			var getWeatherButtonQuery = App.Query(GetWeatherButton);
			return getWeatherButtonQuery.Length > 0;
		}

		public void TapFeedbackButton()
		{
			App.Tap(FeedbackButton);
			App.Screenshot("Feedback Button Tapped");

			if (OniOS)
			{
				App.Tap("Review Existing Feedback");
				App.Screenshot("Review Existing Feedback Tapped");
			}

			Thread.Sleep(1000);

			App.DismissKeyboard();
		}

		public bool IsFeedbackPageOpen()
		{
			string queryString;

			if (OniOS)
				queryString = "Feedback";
			else
				queryString = "input_name";

			var feedbackTitleQuery = App.Query(queryString);

			return feedbackTitleQuery.Length > 0;
		}
	}
}
