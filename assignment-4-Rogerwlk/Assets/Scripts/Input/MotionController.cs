using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CornellTech.View
{
	public class MotionController : MonoBehaviour
	{
		//readonly
		protected const float LINE_RENDERER_FADE_DURATION = 1f;

		/////Enums

		//Serialized
		[SerializeField]
		protected UnityEngine.EventSystems.MotionControlRaycaster motionControlRaycaster;
		[SerializeField]
		protected LineRenderer lineRenderer;

		/////Protected/////
		//References
		//Primitives
		protected Vector3 raycasterLocalPosition;
		protected float lastValidCollisionTime = -LINE_RENDERER_FADE_DURATION;

		//Actions/Funcs
	
		////////////////////////////////////////
		//
		// Properties


		///////////////////////////////////////////////////////////////////////////
		//
		// Inherited from MonoBehaviour
		//
		
		protected void Awake ()
		{				
			GetComponent<Valve.VR.InteractionSystem.Hand> ().RaycastEvent.AddListener (HitInteractable);
		}

		protected void Start ()
		{
			raycasterLocalPosition = motionControlRaycaster.transform.localPosition;
			lineRenderer.SetPosition (0, raycasterLocalPosition);
		}

		protected void Update ()
		{
			UpdateLineRenderer ();
		}

		protected void LateUpdate ()
		{

		}

		///////////////////////////////////////////////////////////////////////////
		//
		// MotionController Functions
		//

		protected void UpdateLineRenderer ()
		{
			float lineRendererVisibilityPercent = 1f - (Time.time - lastValidCollisionTime) / LINE_RENDERER_FADE_DURATION;

			//leave here
			bool showCanvasGraphic = (lineRendererVisibilityPercent == 1f);
			
			lineRenderer.enabled = lineRendererVisibilityPercent > 0;
			if (lineRenderer.enabled)
			{
				Color startColor = new Color (1f, 1f, 1f, lineRendererVisibilityPercent);
				Color endColor = new Color (1f, 1f, 1f, lineRendererVisibilityPercent);
				lineRenderer.SetColors (startColor, endColor);
			}
		}

		public void SetRaycastLineRendererDistance (float distance)
		{
			Vector3 position = Vector3.forward * distance + raycasterLocalPosition;
			lineRenderer.SetPosition (1, position);
		}


		////////////////////////////////////////
		//
		// Event Functions

		protected void HitInteractable(float distance)
		{
			UpdateHitObjects (distance);
			UpdateLastValidCollisionTime ();
		}

		protected void UpdateHitObjects (float distance)
		{
			Vector3 position = Vector3.forward * distance + raycasterLocalPosition;
			SetRaycastLineRendererDistance (distance);
		}

		protected void UpdateLastValidCollisionTime ()
		{
			lastValidCollisionTime = Time.time;
		}

	}
}
