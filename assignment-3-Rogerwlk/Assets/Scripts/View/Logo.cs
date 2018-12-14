using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CornellTech.View
{
	/// <summary>
	/// This class manages the Cornell Tech logo in the scene.
	/// </summary>
	public class Logo : MonoBehaviour
	{
		//Readonly/const
		protected readonly int COLLISION_LIMIT = 5;
		
		/////Protected/////
		//References
		protected LogoPiece[] pieces;
		//Primitives

		///////////////////////////////////////////////////////////////////////////
		//
		// Inherited from MonoBehaviour
		//
		private int collision_count;

		protected void Awake ()
		{
			collision_count = 0;
			pieces = GetComponentsInChildren<LogoPiece> ();
			for (int i = 0; i < pieces.Length; i++)
			{
				pieces [i].CollisionEnteredAction += OnCollisionEntered;
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
		// Logo Functions
		//

		protected void FallApart()
		{
			//TODO: Fill
			foreach (LogoPiece p in pieces) {
				if (p.ShouldFall) {
					p.AddRigidbody();
				}
			}
		}
		
		////////////////////////////////////////
		//
		// Event Functions

		protected void OnCollisionEntered()
		{
			//TODO: Fill
			collision_count++;
			if (collision_count == COLLISION_LIMIT) {
				FallApart();
				collision_count = 0;
			}
		}

	}
}