using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CornellTech.View
{
	/// <summary>
	/// Weather manager.
	/// </summary>
	public class WeatherManager : MonoBehaviour
	{
		//Structs/classes
		[System.Serializable]
		public struct WeatherConditionsGameObject
		{
			public WeatherConditions conditions;
			public GameObject gameObject;
		}

		[System.Serializable]
		public struct WeatherCityButton
		{
			public WeatherCity city;
			public Button button;
		}

		[System.Serializable]
		public struct WeatherConditionsButton
		{
			public WeatherConditions conditions;
			public Button button;
		}

		//Serialized
		[SerializeField]
		protected WeatherCityButton[] cityButtons;
		[SerializeField]
		protected WeatherConditionsButton[] conditionsButtons;
		[SerializeField]
		protected WeatherConditionsGameObject[] conditionsGameObjects;
		[SerializeField]
		protected Text infoText;
		[SerializeField]
		protected Color activeButtonColor = Color.green;
		[SerializeField]
		protected Color inactiveButtonColor = Color.blue;

		//Actions/Funcs
		public Action<WeatherCity> CityButtonSelectedAction;

		///////////////////////////////////////////////////////////////////////////
		//
		// Inherited from MonoBehaviour
		//
		
		protected void Awake ()
		{
			for (int i = 0; i < cityButtons.Length; i++)
			{
				//Lambda wrap so we can send the info to the callback
				int index = i;
				cityButtons[i].button.onClick.AddListener(()=>
				{
					OnCityButtonSelected(cityButtons[index].city,cityButtons[index].button);
				});
			}
			for (int i = 0; i < conditionsButtons.Length; i++)
			{
				//Lambda wrap so we can send the info to the callback
				int index = i;
				conditionsButtons[i].button.onClick.AddListener(()=>
				{
					OnConditionsButtonSelected(conditionsButtons[index].conditions,conditionsButtons[index].button);
				});
			}
		}
		
		protected void Start ()
		{	

		}
		
		protected void Update ()
		{	

		}
		
		///////////////////////////////////////////////////////////////////////////
		//
		// WeatherManager Functions
		//

		/// <summary>
		/// Sets the weather data to the UI, showing the temperature and current weather conditions as text.
		/// Also calls SetConditions with the conditions from the weatherViewData.
		/// </summary>
		/// <param name="weatherViewData">Weather view data.</param>
		public void SetData (WeatherViewData weatherViewData)
		{
            //TODO: Fill.
            infoText.text = Math.Round(weatherViewData.temperature) + "° " + weatherViewData.conditions.ToString();
            SetConditions(weatherViewData.conditions);
		}

		/// <summary>
		/// Sets the conditions so only the appropriate GameObject/particle system is active.
		/// </summary>
		/// <param name="conditions">Conditions.</param>
		protected void SetConditions(WeatherConditions conditions)
		{
			for (int i = 0; i < conditionsGameObjects.Length; i++)
			{
				bool matches = conditionsGameObjects [i].conditions == conditions;
				conditionsGameObjects [i].gameObject.SetActive (matches);
			}
		}

		////////////////////////////////////////
		//
		// Event Functions

		protected void OnConditionsButtonSelected(WeatherConditions conditions, Button button)
		{
			SetConditions (conditions);
		}

		/// <summary>
		/// After a city button is selected we make it so all buttons are reset to the inactive color except for the one selected, which is set to the active color.
		/// Lastly, we fire our CityButtonSelectedAction action.
		/// </summary>
		/// <param name="city">City.</param>
		/// <param name="button">Button.</param>
		protected void OnCityButtonSelected (WeatherCity city, Button button)
		{
            //TODO: Fill.
            for (int i = 0; i < cityButtons.Length; i++)
            {
                if (cityButtons[i].city == city)
                {
                    cityButtons[i].button.SetNormalColor(activeButtonColor);
                }
                else
                {
                    cityButtons[i].button.SetNormalColor(inactiveButtonColor);
                }
                
            }
            CityButtonSelectedAction(city);
        }

	}
}