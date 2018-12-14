using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace CornellTech.View
{
	/// <summary>
	/// Block creator, looks like the glue gun in the scene.
	/// </summary>
	public class BlockCreator : MonoBehaviour
	{
		//Readonly/const
		protected readonly float SNAP_INCREMENT = .1f;

		//Serialized
		[SerializeField]
		protected Transform drawPointTransform;
		[SerializeField]
		protected GameObject blockPrefab;
		[SerializeField]
		protected MeshRenderer glueMeshRenderer;
		[SerializeField]
		protected Color color = Color.white;
		
		/////Protected/////
		//References
		protected AudioClip spawnAudioClip;
		protected Hand activeHand;
		
		//Actions/Funcs
		public Action<Block> BlockCreatedAction;
		public Func<Block[]> GetBlocksFunc;

		///////////////////////////////////////////////////////////////////////////
		//
		// Inherited from MonoBehaviour
		//
		
		protected void Awake ()
		{
			//sets the color on the glue gun at runtime
			glueMeshRenderer.material.SetColor ("_BaseColorFactor", color);

			//load our sound effect
			spawnAudioClip = Resources.Load ("BlockSpawnAudioClip") as AudioClip;
		}
		
		protected void Start ()
		{	

		}
		
		protected void Update ()
		{	
			if (activeHand != null)
			{
				//TODO: Use SteamVR Input to find out if we are pressing the CreateBlock is 'down'.
				bool isPressing = SteamVR_Input._default.inActions.CreateBlock.GetStateDown(activeHand.handType);
				if (isPressing)
					CheckPosition ();
			}
		}
		
		///////////////////////////////////////////////////////////////////////////
		//
		// BlockCreator Functions
		//

		/// <summary>
		/// Checks the desired position to see if we can create a block there. If we can it creates one.
		/// </summary>
		protected void CheckPosition ()
		{
            //TODO: Fill.
            Vector3 pressPos = GetGridPressPosition();
            if (!IsOverlappingOtherBlock(pressPos))
            {
                CreateBlock(pressPos);
            }
		}

		/// <summary>
		/// Gets the nearest grid point to the desired position.
		/// </summary>
		/// <returns>The grid press position.</returns>
		protected Vector3 GetGridPressPosition()
		{
			//rounds to nearest 0.1 (snap to grid)
			float multiplier = 1f / SNAP_INCREMENT;

			Vector3 pressPosition = drawPointTransform.position;
			pressPosition.x = Mathf.Round (pressPosition.x * multiplier) / multiplier;
			pressPosition.y = Mathf.Round (pressPosition.y * multiplier) / multiplier;
			pressPosition.z = Mathf.Round (pressPosition.z * multiplier) / multiplier;
			return pressPosition;
		}

		/// <summary>
		/// Determines whether this position already has a block in it.
		/// </summary>
		/// <returns><c>true</c> if this position has a block in the position; otherwise, <c>false</c>.</returns>
		/// <param name="pressPosition">Press position.</param>
		protected bool IsOverlappingOtherBlock(Vector3 pressPosition)
		{
			//get the max squared distance, anything closer than this is in the same grid location
			//we use squared over normal distance to prevent expensive sqrt calls in distance calculations
			float maxSquaredDistance = Mathf.Pow (SNAP_INCREMENT / 2f, 2f);

			//Check distance relative to all other blocks
			float minSquaredDistance = float.MaxValue;
			Block[] blocks = GetBlocksFunc();
			for (int i = 0; i < blocks.Length; i++)
			{
				float squaredDistance = (blocks [i].transform.position - pressPosition).sqrMagnitude;
				if (squaredDistance < minSquaredDistance)
					minSquaredDistance = squaredDistance;
			}
			//minSquaredDistance would be zero if there was another block in the position but this is safer
			bool isOverlappingOtherBlock = minSquaredDistance < maxSquaredDistance;
			return isOverlappingOtherBlock;
		}

		/// <summary>
		/// Instantiates the block at the specified position, sets its color, and plays an audio clip at the specified point.
		/// Also fires the action for the parent.
		/// </summary>
		/// <param name="position">Position.</param>
		protected void CreateBlock (Vector3 position)
		{
            //TODO: Fill.
            GameObject newBlock = Instantiate<GameObject>(blockPrefab, position, Quaternion.Euler(0f,0f,0f));
            Block newBlockScript = newBlock.GetComponent<Block>();
            newBlockScript.SetColor(color);
            BlockCreatedAction(newBlockScript);

            AudioSource.PlayClipAtPoint(spawnAudioClip, position);
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


	}
}