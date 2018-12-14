using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CornellTech.View
{
	/// <summary>
	/// Manages the character's movement and animation through the scene.
	/// </summary>
	public class CharacterManager : MonoBehaviour
	{
		//Serialized
		[SerializeField]
		protected Transform characterTransform;
		[SerializeField]
		protected Transform[] points;
		[SerializeField]
		protected Color activeColor = Color.green;
		[SerializeField]
		protected Color inactiveColor = Color.red;
		
		/////Protected/////
		//References
		protected Coroutine activeCoroutine;
		protected NavMeshAgent navMeshAgent;
		protected Animator animator;
		//Primitives
		protected int destinationPointIndex;

		///////////////////////////////////////////////////////////////////////////
		//
		// Inherited from MonoBehaviour
		//
		
		protected void Awake ()
		{
			animator = GetComponentInChildren<Animator> ();
			navMeshAgent = GetComponentInChildren<NavMeshAgent> ();
			navMeshAgent.autoBraking = false;
		}
		
		protected void Start ()
		{	
			activeCoroutine = StartCoroutine (GotoNextPoint ());
		}
		
		protected void Update ()
		{	
			// Choose the next destination point when the agent gets close to the current one.
			if (activeCoroutine == null && !navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.3f)
				activeCoroutine = StartCoroutine (GotoNextPoint ());
		}
		
		///////////////////////////////////////////////////////////////////////////
		//
		// CharacterManager Functions
		//

		protected IEnumerator GotoNextPoint ()
		{
			WaveAtMainCamera ();

			yield return new WaitForSeconds (1f);

			StartWalking ();

			if (points.Length == 0)
			{
				Debug.LogError ("No points found; breaking.");
				yield break;
			}

			UpdateDestinationPointColors ();

			// Set the agent to go to the currently selected destination.
			navMeshAgent.SetDestination (points [destinationPointIndex].position);

			// Choose the next point in the array as the destination, cycling to the start if necessary.
			destinationPointIndex = (destinationPointIndex + 1) % points.Length;

			//Sets the coroutine to null so our update loop knows we are not animating anymore.
			activeCoroutine = null;
		}

		/// <summary>
		/// Updates the destination point colors so that the active point is green and the rest are red (or whatever colors are serialized).
		/// </summary>
		protected void UpdateDestinationPointColors()
		{
			for (int i = 0; i < points.Length; i++)
			{
				MeshRenderer renderer = points [i].GetComponentInChildren<MeshRenderer> ();
				bool isActive = (i == destinationPointIndex);
				Color color = isActive ? activeColor : inactiveColor;
				renderer.material.color = color;
			}
		}

		/// <summary>
		/// Stops the nav mesh from moving, makes the character transform look at the main camera, and tells the animator to cross fade into the "Wave" animation state.
		/// </summary>
		protected void WaveAtMainCamera()
		{
            //TODO: Fill.
            navMeshAgent.isStopped = true;
            characterTransform.LookAt(Camera.main.transform.position); // one liner for the following three lines
            // Vector3 relativePos = Camera.main.transform.position - characterTransform.position;
            // Quaternion rotation = Quaternion.LookRotation(relativePos);
            // characterTransform.rotation = rotation;
            animator.CrossFade("Wave", 0.2f);
		}

		/// <summary>
		/// Resumes nav mesh movement, and tells the animator to cross fade into the "Walking" animation state.
		/// </summary>
		protected void StartWalking()
		{
            //TODO: Fill.
            navMeshAgent.isStopped = false;
            animator.CrossFade("Walking", 0.2f);
		}

	}
}