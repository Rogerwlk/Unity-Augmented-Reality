using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace CornellTech.View
{
	/// <summary>
	/// Represents a block GameObject.
	/// </summary>
	public class Block : MonoBehaviour
	{
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
		// Block Functions
		//

		public void SetColor(Color color)
		{
			GetComponentInChildren<MeshRenderer> ().material.color = color;
		}

		public void AddPhysics ()
		{
			gameObject.AddComponent<Rigidbody> ();

			MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter> ();

			for (int i = 0; i < meshFilters.Length; i++)
			{
				MeshCollider meshCollider = meshFilters [i].gameObject.AddComponent<MeshCollider> ();
				meshCollider.convex = true;
			}
		}

		public void MakeThrowable ()
		{
			gameObject.AddComponent<Interactable> ();
			gameObject.AddComponent<VelocityEstimator> ();

			Throwable throwable = gameObject.AddComponent<Throwable> ();
			throwable.releaseVelocityStyle = ReleaseStyle.ShortEstimation;
			throwable.restoreOriginalParent = true;
		}
	}
}