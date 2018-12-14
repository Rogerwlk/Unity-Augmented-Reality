using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CornellTech.View
{
	/// <summary>
	/// This class represents a single piece of the Cornll Tech logo.
	/// </summary>
	public class LogoPiece : MonoBehaviour
	{
		//Serialized
		[SerializeField]
		protected bool shouldFall = true;
		
		/////Protected/////
		//References
		protected Material material;
		
		//Actions/Funcs
		public Action CollisionEnteredAction;

		//Properties
		public bool ShouldFall {
			get {
				return this.shouldFall;
			}
		}

		private int hit;
		///////////////////////////////////////////////////////////////////////////
		//
		// Inherited from MonoBehaviour
		//
		
		protected void Awake()
		{
			material = GetComponent<Renderer>().material;
		}

		protected void Start()
		{	
			
		}

		protected void Update()
		{	

		}

		///////////////////////////////////////////////////////////////////////////
		//
		// LogoPiece Functions
		//

		public void AddRigidbody()
		{
			gameObject.AddComponent<Rigidbody>();
		}

		protected void RandomizeColor()
		{
			//TODO: Fill
			material.color = UnityEngine.Random.ColorHSV(0f, 1f, 0f, 1f, 0f, 1f, 0.3f, 0.7f);
		}

		////////////////////////////////////////
		//
		// Event Functions

		protected void OnCollisionEnter(Collision collision)
		{
            string colliderTag = collision.collider.gameObject.tag;

            if (colliderTag == "PhysicsItem")
            {
                RandomizeColor();
            }

			if (CollisionEnteredAction != null)
				CollisionEnteredAction();
		}
	}
}