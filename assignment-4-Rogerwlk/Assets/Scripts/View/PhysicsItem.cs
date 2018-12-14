using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace CornellTech.View
{
	/// <summary>
	/// This class represents a physics item, an item which follows the laws of physics and interacts with the Valve interaction system.
	/// </summary>
	public class PhysicsItem : MonoBehaviour
	{
		//Readonly/const
		protected readonly float SCALE_MULTIPLIER = 1f;
		
		/////Protected/////
		//References
		protected Hand activeHand;
		protected Rigidbody rigidbody;
		//Primitives
		protected Vector3 startLocalPosition;
		protected Quaternion startLocalRotation;
		protected Vector3 startLocalScale;
		protected bool isAnimating;

		///////////////////////////////////////////////////////////////////////////
		//
		// Inherited from MonoBehaviour
		//
		
		protected void Awake ()
		{

		}
		
		protected void Start ()
		{	
			AddPhysics ();
			MakeThrowable ();

			startLocalPosition = transform.localPosition;
			startLocalRotation = transform.localRotation;
			startLocalScale = transform.localScale;
		}
		
		protected void Update ()
		{	
			UpdateScale ();
		}
		
		///////////////////////////////////////////////////////////////////////////
		//
		// PhysicsItem Functions
		//

		protected void UpdateScale()
		{
			if (activeHand != null)
			{

				Vector2 scaleDelta = SteamVR_Input._default.inActions.ScaleItem.GetAxisDelta (activeHand.handType);
				float scaleDeltaY = scaleDelta.y;

				if (Mathf.Abs (scaleDeltaY) > .1f)
					return;

				transform.localScale += Vector3.one * scaleDeltaY * SCALE_MULTIPLIER;
			}
		}

		protected void AddPhysics ()
		{
			rigidbody = gameObject.AddComponent<Rigidbody> ();

			MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter> ();

			for (int i = 0; i < meshFilters.Length; i++)
			{
				MeshCollider meshCollider = meshFilters [i].gameObject.AddComponent<MeshCollider> ();
				meshCollider.convex = true;
			}
		}

		protected void MakeThrowable ()
		{
			gameObject.AddComponent<Interactable> ();
			gameObject.AddComponent<VelocityEstimator> ();

			Throwable throwable = gameObject.AddComponent<Throwable> ();
			throwable.releaseVelocityStyle = ReleaseStyle.ShortEstimation;
			throwable.restoreOriginalParent = true;
		}

		protected void SetRigidBodyEnabled (bool value)
		{
			rigidbody.useGravity = !value;
			rigidbody.isKinematic = value;
		}

		protected IEnumerator ReturnToOrigin ()
		{
			isAnimating = true;

			yield return new WaitForSeconds (3f);

			SetRigidBodyEnabled (true);

			Vector3 animationStartLocalPosition = transform.localPosition;
			Quaternion animationStartLocalRotation = transform.localRotation;
			Vector3 animationStartLocalScale = transform.localScale;

			float percent = 0;
			while (percent < 1f)
			{
				transform.localPosition = Vector3.Lerp (animationStartLocalPosition, startLocalPosition, percent);
				transform.localRotation = Quaternion.Lerp (animationStartLocalRotation, startLocalRotation, percent);
				transform.localScale = Vector3.Lerp (animationStartLocalScale, startLocalScale, percent);

				percent += Time.deltaTime;

				yield return null;
			}

			SetRigidBodyEnabled (false);
			isAnimating = false;
		}
		
		////////////////////////////////////////
		//
		// Event Functions

		//Called with SendMessage from Valve.VR.InteractionSystem.Hand
		protected void OnAttachedToHand (Hand hand)
		{
			activeHand = hand;
		}

		//Called with SendMessage from Valve.VR.InteractionSystem.HAnd
		protected void OnDetachedFromHand (Hand hand)
		{
			activeHand = null;
		}
			
		protected void OnCollisionEnter (Collision collision)
		{
			//The tag of the GameObject with the collider we hit.
			string colliderTag = collision.collider.gameObject.tag;

			if (colliderTag == "Platform" || colliderTag == "Floor")
			{
				if (!isAnimating)
					StartCoroutine (ReturnToOrigin ());
			}
		}

	}
}