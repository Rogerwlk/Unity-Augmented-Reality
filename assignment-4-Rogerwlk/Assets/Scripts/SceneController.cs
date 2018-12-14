using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using CornellTech.View;
using CornellTech.Model;
using PolyToolkit;

namespace CornellTech.Controller
{
	/// <summary>
	/// Scene controller.
	/// This class is responsible for managing the scene, and routing communication between the Model and View.
	/// </summary>
	public class SceneController : MonoBehaviour
	{
		//readonly/const
		//Full list available for download here: http://bulk.openweathermap.org/sample/ (city.list.json.gz)
		protected readonly Dictionary<WeatherCity,string> WEATHER_CITY_TO_ID = new Dictionary<WeatherCity, string> () {

			{WeatherCity.NewYork,"5128581"},
			{WeatherCity.LosAngeles,"5368361"},
			{WeatherCity.Tokyo,"1850147"},
            {WeatherCity.Nanjing,"1799963"},
            {WeatherCity.WashingtonDC,"4140963"},
		};


		//Serialized Fields
		[Header ("Model")]
		[SerializeField]
		protected WeatherDataProvider weatherDataProvider;
		[SerializeField]
		protected PolyDataProvider polyDataProvider;
		[Header ("View")]
		[SerializeField]
		protected WeatherManager weatherManager;
		[SerializeField]
		protected ModelBrowser modelBrowser;
		[SerializeField]
		protected PhysicsItemManager physicsItemManager;
		[SerializeField]
		protected Button restartSceneButton;
		
		/////Protected/////
		//References
		protected Dictionary<string,ModelViewData> modelViewDataDictionary = new Dictionary<string, ModelViewData> ();

		///////////////////////////////////////////////////////////////////////////
		//
		// Inherited from MonoBehaviour
		//
		// Read more about these events here: https://docs.unity3d.com/Manual/ExecutionOrder.html
		// 
		
		protected void Awake ()
		{
			modelBrowser.ModelViewDataSelected += OnModelViewDataSelected;

			restartSceneButton.onClick.AddListener (OnResetSceneButtonClicked);

			weatherManager.CityButtonSelectedAction += OnWeatherCityButtonSelected;
		}

		protected void Start ()
		{	
			LoadPolyAssets ();
			LoadCurrentWeather (WeatherCity.NewYork);
		}

		protected void Update ()
		{	

		}

		
		///////////////////////////////////////////////////////////////////////////
		//
		// SceneController Functions
		//

		protected void LoadPolyAssets ()
		{
			polyDataProvider.ListAssets (12, OnPolyAssetsListed);
		}

		protected void LoadCurrentWeather(WeatherCity city)
		{
			if (!WEATHER_CITY_TO_ID.ContainsKey (city))
			{
				Debug.LogError ("Can't find this city's ID in the dictionary; returning.");
				return;
			}
			//gets the string that the OpenWeatherMap API uses to identify the city.
			string cityID = WEATHER_CITY_TO_ID [city];

			//Call the weather data provider to do the actual loading.
			this.weatherDataProvider.LoadCurrentWeather (cityID,OnCurrentWeatherLoaded);
		}

		////////////////////////////////////////
		//
		// Event Functions

		protected void OnResetSceneButtonClicked()
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene (0);
		}

		////////////////////////////////////////
		//
		// Weather Event Functions

		protected void OnWeatherCityButtonSelected(WeatherCity weatherCity)
		{
			LoadCurrentWeather (weatherCity);
		}

		protected void OnCurrentWeatherLoaded(WeatherData weatherData)
		{
			WeatherViewData weatherViewData = Convert (weatherData);

			this.weatherManager.SetData (weatherViewData);
		}

		////////////////////////////////////////
		//
		// Weather Model To View Conversion

		//Converts model object to view object
		protected WeatherViewData Convert (WeatherData weatherData)
		{
			//conditions string to enum
			WeatherConditions conditions = WeatherConditions.Clear;
			if(Enum.IsDefined(typeof(WeatherConditions),weatherData.weatherMainCategory))
				conditions = (WeatherConditions) Enum.Parse (typeof(WeatherConditions), weatherData.weatherMainCategory);

			//kelvin to farenheit
			float temperatureFarenheit = weatherData.temperature*(9f/5f) - 459.67f;

			//create the view data for the view scripts to use.
			WeatherViewData weatherViewData = new WeatherViewData (conditions, temperatureFarenheit);
			return weatherViewData;
		}

		////////////////////////////////////////
		//
		// Poly Event Functions

		protected void OnPolyAssetsListed (List<PolyAsset> assets)
		{
			if (assets == null)
				return;
			
			polyDataProvider.FetchThumbnails (assets, OnPolyThumbnailsFetched);
		}

		protected void OnPolyThumbnailsFetched (List<PolyAsset> assets)
		{
			if (assets == null)
				return;
			
			List<ModelViewData> modelViewDataList = new List<ModelViewData> ();
			for (int i = 0; i < assets.Count; i++)
			{
				PolyAssetData polyAssetData = new PolyAssetData (assets [i]);
				ModelViewData modelViewData = Convert (polyAssetData);
				modelViewDataList.Add (modelViewData);
			}
			modelBrowser.DisplayModels (modelViewDataList);
		}

		protected void OnModelViewDataSelected (ModelViewData modelViewData)
		{
			PolyAsset polyAsset = polyDataProvider.GetPolyAssetWithName (modelViewData.ID);
			polyDataProvider.ImportAsset (polyAsset, OnPolyAssetImported);
		}

		protected void OnPolyAssetImported (GameObject gameObject)
		{
			if (gameObject == null)
				return;
			
			physicsItemManager.SetupGameObject (gameObject);
		}

		////////////////////////////////////////
		//
		// Poly Model To View Conversion

		//Converts model object to view object
		protected ModelViewData Convert (PolyAssetData polyAssetData)
		{
			string id = GetModelViewDataID (polyAssetData);

			ModelViewData modelViewData = null;

			if (modelViewDataDictionary.ContainsKey (id))
				modelViewData = modelViewDataDictionary [id];
			else
			{
				modelViewData = new ModelViewData ();
				modelViewDataDictionary.Add (id, modelViewData);
			}

			Update (modelViewData, polyAssetData);

			return modelViewData;
		}

		//Updates view object from model object
		protected void Update (ModelViewData modelViewData, PolyAssetData polyAssetData)
		{
			modelViewData.ID = polyAssetData.polyAsset.name;
			modelViewData.name = polyAssetData.polyAsset.displayName;
			modelViewData.authorName = polyAssetData.polyAsset.authorName;
			modelViewData.thumbnailTexture = polyAssetData.polyAsset.thumbnailTexture;
		}

		//Gets a unique ID from a PolyAssetData object
		protected string GetModelViewDataID (PolyAssetData polyAssetData)
		{
			return polyAssetData.polyAsset.name;
		}
	}
}

namespace CornellTech.View
{
	//enums
	public enum WeatherCity
	{
		NewYork,
		LosAngeles,
		Tokyo,
        Nanjing,
        WashingtonDC,
	}
	public enum WeatherConditions
	{
		Thunderstorm,
		Drizzle,
		Rain,
		Snow,
		Atmosphere,
		Clear,
		Clouds,
	}

	//classes

	public class ModelViewData
	{
		//Unique ID
		public string ID;
		//display name
		public string name;
		//author name
		public string authorName;
		//thumbnail
		public Texture2D thumbnailTexture;
	}



	public class WeatherViewData
	{
		public WeatherConditions conditions;
		//farenheit
		public float temperature;

		public WeatherViewData (WeatherConditions conditions, float temperature)
		{
			this.conditions = conditions;
			this.temperature = temperature;
		}
		
	}
}

namespace CornellTech.Model
{
	public class PolyAssetData
	{
		public PolyAsset polyAsset;

		public PolyAssetData (PolyAsset polyAsset)
		{
			this.polyAsset = polyAsset;
		}
	}

	public class WeatherData
	{
		//weather.main
		public string weatherMainCategory;
		//kelvin
		public float temperature;

		public WeatherData (string weatherMainCategory, float temperature)
		{
			this.weatherMainCategory = weatherMainCategory;
			this.temperature = temperature;
		}
	}
}