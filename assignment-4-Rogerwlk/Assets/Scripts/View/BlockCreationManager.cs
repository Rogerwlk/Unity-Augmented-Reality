using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace CornellTech.View
{
	/// <summary>
	/// Manages the creation of blocks.
	/// </summary>
	public class BlockCreationManager : MonoBehaviour
	{
		//Serialized
		[SerializeField]
		protected Button dropBlocksButton;

        [SerializeField]
        protected Button deleteBlocksButton;

        /////Protected/////
        //References
        protected BlockCreator[] blockCreators;
		protected List<Block> activeBlocks = new List<Block>();
		protected List<Block> droppedBlocks = new List<Block>();

		///////////////////////////////////////////////////////////////////////////
		//
		// Inherited from MonoBehaviour
		//
		
		protected void Awake ()
		{
			blockCreators = GetComponentsInChildren<BlockCreator> ();

			RegisterEvents ();
		}
		
		protected void Start ()
		{	

		}
		
		protected void Update ()
		{	

		}
		
		///////////////////////////////////////////////////////////////////////////
		//
		// BlockCreationManager Functions
		//

		/// <summary>
		/// Registers the necessary actions/funcs.
		/// </summary>
		protected void RegisterEvents()
		{
			for (int i = 0; i < blockCreators.Length; i++)
			{
				blockCreators [i].BlockCreatedAction += OnBlockCreated;
				blockCreators [i].GetBlocksFunc += GetBlocks;
			}

			dropBlocksButton.onClick.AddListener(OnDropBlocksButtonPressed);
            deleteBlocksButton.onClick.AddListener(OnDeleteBlocksButtonPressed);
        }

		/// <summary>
		/// Returns all the active blocks for the creators to check against.
		/// </summary>
		/// <returns>The blocks.</returns>
		protected Block[] GetBlocks()
		{
			return this.activeBlocks.ToArray ();
		}
		
		////////////////////////////////////////
		//
		// Event Functions

		/// <summary>
		/// After the drop blocks button is pressed, we add physics and interactivity to the active blocks and update our lists.
		/// </summary>
		protected void OnDropBlocksButtonPressed()
		{
			//TODO: Fill.
            for (int i = 0; i < activeBlocks.Count; i++)
            {
                activeBlocks[i].AddPhysics();
                activeBlocks[i].MakeThrowable();
                droppedBlocks.Add(activeBlocks[i]);
            }
            activeBlocks.Clear();
		}

		/// <summary>
		/// After the delete blocks is pressed we delete the block gameobjects and update our lists.
		/// </summary>
		protected void OnDeleteBlocksButtonPressed()
		{
            //TODO: Fill.
            for (int i = 0; i < activeBlocks.Count; i++)
            {
                Destroy(activeBlocks[i].gameObject);
            }
            for (int i = 0; i < droppedBlocks.Count; i++)
            {
                Destroy(droppedBlocks[i].gameObject);
            }
            activeBlocks.Clear();
            droppedBlocks.Clear();
        }

		/// <summary>
		/// When a block is created we need to add it to our list.
		/// </summary>
		/// <param name="block">Block.</param>
		protected void OnBlockCreated(Block block)
		{
			activeBlocks.Add (block);
		}
	}
}