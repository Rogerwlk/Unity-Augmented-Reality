using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CornellTech.View
{
	/// <summary>
	/// This class manages the physics items.
	/// </summary>
	public class PhysicsItemManager : MonoBehaviour
	{
		//Serialized
		[SerializeField]
		protected Transform	itemSpawnAnchorTransform;

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
		// PhysicsItemManager Functions
		//

		public void SetupGameObject(GameObject gameObject)
		{
			gameObject.AddComponent<PhysicsItem> ();
			gameObject.transform.SetParent (transform);
			gameObject.transform.position = itemSpawnAnchorTransform.position;
		}
	}
}