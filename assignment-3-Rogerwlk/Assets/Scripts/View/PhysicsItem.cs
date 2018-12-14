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
		protected bool isAnimating;

        //used for changing color bonus1
        private Renderer[] m_renders;
        private List<Color> origin_colors;
		///////////////////////////////////////////////////////////////////////////
		//
		// Inherited from MonoBehaviour
		//
		
		protected void Awake()
		{

		}

		protected void Start()
		{
            origin_colors = new List<Color>();
            m_renders = gameObject.GetComponentsInChildren<Renderer>(); //get all renderer components
            //Debug.Log(m_renders.ToString());
            //record original color
            foreach(Renderer r in m_renders)
            {
                origin_colors.Add(r.material.color);
                //Debug.Log("color: "+r.material.color.ToString());
            }
            AddPhysics();
			MakeThrowable();
            //gameObject.tag = "PhysicsItem"; // not correct place to add collision tag
		}

		protected void Update()
		{	
			UpdateScale();
		}

		///////////////////////////////////////////////////////////////////////////
		//
		// PhysicsItem Functions
		//

		protected void UpdateScale()
		{
			if (activeHand != null && SteamVR_Input._default.inActions.OnPad.GetState(activeHand.handType)) {
				//TODO: Fill
				float delta_y_scale = SteamVR_Input._default.inActions.ScaleItem.GetAxisDelta(activeHand.handType).y;
				if (delta_y_scale != 0) {
					Vector3 temp = gameObject.transform.localScale;
					Debug.Log(delta_y_scale.ToString());
					temp.x += delta_y_scale * SCALE_MULTIPLIER;
					temp.y += delta_y_scale * SCALE_MULTIPLIER;
					temp.z += delta_y_scale * SCALE_MULTIPLIER;
					if (temp.x < 0.1f)
						temp.x = 0.1f;
					if (temp.y < 0.1f)
						temp.y = 0.1f;
					if (temp.z < 0.1f)
						temp.z = 0.1f;

					gameObject.transform.localScale = temp;
				}
			}
		}

		protected void AddPhysics()
		{
			//TODO: Fill
			rigidbody = gameObject.AddComponent<Rigidbody>();
            //rigidbody.tag = "PhysicsItem"; //not correct place to add collision tag
			var children_components = gameObject.GetComponentsInChildren<MeshFilter>();
			foreach (MeshFilter c in children_components) {
				MeshCollider temp = c.gameObject.AddComponent<MeshCollider>();
				temp.convex = true;
                temp.tag = "PhysicsItem"; // correct place to add tag is MeshCollider
			}
		}

		protected void MakeThrowable()
		{
			//TODO: Fill
			gameObject.AddComponent<Interactable>();
			gameObject.AddComponent<VelocityEstimator>();
			Throwable t = gameObject.AddComponent<Throwable>();
			t.releaseVelocityStyle = ReleaseStyle.ShortEstimation;
			t.restoreOriginalParent = true;
		}

		protected void SetRigidBodyEnabled(bool value)
		{
			rigidbody.useGravity = !value;
			rigidbody.isKinematic = value;
		}

		protected IEnumerator ReturnToOrigin()
		{
			isAnimating = true;

			//TODO: Fill
			yield return new WaitForSeconds(3);

			SetRigidBodyEnabled(true);

			Transform temp = GameObject.FindWithTag("Respawn").transform;
			gameObject.transform.localScale = temp.localScale;
			gameObject.transform.position = temp.position;
			gameObject.transform.rotation = temp.rotation;
			SetRigidBodyEnabled(false);
			isAnimating = false;
		}

		////////////////////////////////////////
		//
		// Event Functions

		//Called with SendMessage from Valve.VR.InteractionSystem.Hand
		protected void OnAttachedToHand(Hand hand)
		{
			activeHand = hand;
            
            foreach(Renderer r in m_renders)
            {
                r.material.color = Color.red;
            }
        }

		//Called with SendMessage from Valve.VR.InteractionSystem.HAnd
		protected void OnDetachedFromHand(Hand hand)
		{
			activeHand = null;
            // return to original color
            int i = 0;
            foreach(Renderer r in m_renders)
            {
                r.material.color = origin_colors[i];
                i++;
            }
		}

		protected void OnCollisionEnter(Collision collision)
		{
			//The tag of the GameObject with the collider we hit.
			string colliderTag = collision.collider.gameObject.tag;

			if (colliderTag == "Platform" || colliderTag == "Floor") {
				if (!isAnimating)
					StartCoroutine(ReturnToOrigin());
			}
		}

	}
}