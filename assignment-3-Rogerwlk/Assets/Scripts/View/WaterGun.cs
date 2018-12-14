using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace CornellTech.View
{
	/// <summary>
	/// This class represents the water gun GameObject in the scene.
	/// </summary>
	public class WaterGun : MonoBehaviour
	{
		/////Protected/////
		//References
		protected ParticleSystem bubblesParticleSystem;
		protected AudioSource bubblesAudioSource;
		protected Throwable throwable;

		///////////////////////////////////////////////////////////////////////////
		//
		// Inherited from MonoBehaviour
		//
		
		protected void Awake ()
		{
			throwable = GetComponent<Throwable> ();
			throwable.onPickUp.AddListener (OnPickedUp);
			throwable.onDetachFromHand.AddListener (OnReleased);

			bubblesParticleSystem = GetComponentInChildren<ParticleSystem> ();
			bubblesAudioSource = GetComponentInChildren<AudioSource> ();
		}
		
		protected void Start ()
		{	
			SetBubblesEnabled (false);
		}
		
		protected void Update ()
		{	
			
		}
		
		///////////////////////////////////////////////////////////////////////////
		//
		// WaterGun Functions
		//

		protected void SetBubblesEnabled(bool enabled)
		{
			ParticleSystem.EmissionModule emissionModule = bubblesParticleSystem.emission;
			emissionModule.enabled = enabled;

			if (enabled)
				bubblesAudioSource.Play ();
			else
				bubblesAudioSource.Stop ();
		}
		
		////////////////////////////////////////
		//
		// Event Functions

		protected void OnPickedUp()
		{
			SetBubblesEnabled (true);
		}

		protected void OnReleased()
		{
			SetBubblesEnabled (false);
		}

	}
}