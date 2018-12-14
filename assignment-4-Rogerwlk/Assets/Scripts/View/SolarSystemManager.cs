using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CornellTech.View
{
	/// <summary>
	/// Solar system manager.
	/// </summary>
	public class SolarSystemManager : MonoBehaviour
	{
		//Serialized
		[SerializeField]
		protected Button resetButton;
		
		/////Protected/////
		//References
		protected Planet[] planets;

		///////////////////////////////////////////////////////////////////////////
		//
		// Inherited from MonoBehaviour
		//

		//Registers the reset button event. Assigns the values to the planets array.
		protected void Awake ()
		{
            //TODO: Fill.
            resetButton.onClick.AddListener(OnResetButtonClicked);
            planets = GetComponentsInChildren<Planet>();
		}
		
		protected void Start ()
		{	

		}
		
		protected void Update ()
		{	

		}
		
		///////////////////////////////////////////////////////////////////////////
		//
		// SolarSystemManager Functions
		//

		////////////////////////////////////////
		//
		// Event Functions

		/// <summary>
		/// Calls Reset on each planet after the reset button is clicked.
		/// </summary>
		protected void OnResetButtonClicked()
		{
			//TODO: Fill.
            for (int i = 0; i < planets.Length; i++)
            {
                planets[i].Reset();
            }
		}
	}
}