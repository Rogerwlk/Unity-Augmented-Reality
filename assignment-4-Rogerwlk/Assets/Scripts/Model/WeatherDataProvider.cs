using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using SimpleJSON;

namespace CornellTech.Model
{
	/// <summary>
	/// Weather data provider using the OpenWeatherMap API, located here: https://openweathermap.org/current
	/// </summary>
	public class WeatherDataProvider : MonoBehaviour
	{
		//readonly
		protected string URL_TEMPLATE = "http://api.openweathermap.org/data/2.5/weather?id={0}&appid=bbf2e81b2d3dad99112b82f387d8202d";

		///////////////////////////////////////////////////////////////////////////
		//
		// Inherited from MonoBehaviour
		//
		
		protected void Awake ()
		{

		}
		
		protected void Start ()
		{	
			
		}
		
		protected void Update ()
		{	

		}
		
		///////////////////////////////////////////////////////////////////////////
		//
		// WeatherDataProvider Functions
		//

		public void LoadCurrentWeather(string cityID, Action<WeatherData> callback)
		{
			StartCoroutine (LoadCurrentWeatherCoroutine (cityID, callback));
		}

		/// <summary>
		/// Loads the current weather from the OpenWeatherMap API. Documentation here: https://openweathermap.org/current
		/// </summary>
		/// <returns>The current weather coroutine.</returns>
		/// <param name="cityID">City I.</param>
		/// <param name="callback">Callback.</param>
		protected IEnumerator LoadCurrentWeatherCoroutine (string cityID, Action<WeatherData> callback)
		{
			//generate the url with the specified cityID
			string url = string.Format (URL_TEMPLATE, cityID);

			//create our webrequest object
			UnityWebRequest www = UnityWebRequest.Get (url);

			//Load the web content
			yield return www.SendWebRequest ();

			//save the return text to a string
			string jsonText = www.downloadHandler.text;

			//parse the text into a JSONNode object
			JSONNode json = JSON.Parse (jsonText);

			//Get the info we want
			string weatherMain = json ["weather"] [0] ["main"].Value;
			float mainTemp = json ["main"] ["temp"].AsFloat;

			//create a class with the data
			WeatherData weatherData = new WeatherData (weatherMain, mainTemp);

			//callback with the data
			if(callback!=null)
				callback (weatherData);
		}
	}
}