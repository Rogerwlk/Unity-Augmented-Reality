using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UIPrimitives
{
	public class UIAnimator : MonoBehaviour
	{
	
		//readonly

		//Serialized
	
		//Protected
		protected List<Queue<UIAnimation_Base>> myUIAnimations;
	
		//Delegates

		//Singleton


		///////////////////////////////////////////////////////////////////////////
		//
		// Inherited from MonoBehaviour
		//

		protected virtual void Awake ()
		{
			myUIAnimations = new List<Queue<UIAnimation_Base>> ();
		}
	
		///////////////////////////////////////////////////////////////////////////
		//
		// UIAnimator Functions
		//

		public int GetActiveAnimationCount()
		{
			int count = 0;
			//you called early and I haven't even had time to make the list!
			if (myUIAnimations == null)
				return count;
			for (int i = 0; i < myUIAnimations.Count; ++i)
			{
				count += myUIAnimations [i].Count;
			}
			return count;
		}

		public void SkipCurrentAnimation ()
		{
			//you called early and I haven't even had time to make the list!
			if (myUIAnimations == null)
				return;
			for (int i = 0; i < myUIAnimations.Count; ++i)
			{
				if (myUIAnimations [i].Count != 0)
				{
					myUIAnimations [i].Dequeue ();
				}
			}
		}

		public void ClearAllAnimations ()
		{
			//you called early and I haven't even had time to make the list!
			if (myUIAnimations == null)
				return;
			for (int i = 0; i < myUIAnimations.Count; ++i)
			{

				myUIAnimations [i].Clear ();
			}
		}

		protected bool UpdateAnimations ()
		{	
			bool updatedAnimation = false;	
			//Loop through the list of queues of animations and update the ones that are not empty
			for (int i = 0; i < myUIAnimations.Count; ++i)
			{			
				if (myUIAnimations [i].Count != 0)
				{
					UpdateAnimation (myUIAnimations [i]);
					updatedAnimation = true;
				}
			}
			return updatedAnimation;
		}

		protected void UpdateAnimation (Queue<UIAnimation_Base> currentUIAnimationQueue)
		{
			UIAnimation_Base currentUIAnimation = currentUIAnimationQueue.Peek ();
			//Initialize
			if (!currentUIAnimation.hasStarted)
				currentUIAnimation.StartAnimation ();
			//Update Before
			if (currentUIAnimation.updateBefore)
				currentUIAnimation.UpdateAnimation ();
			//Check If Done
			if (currentUIAnimation.IsComplete ())
			{
				currentUIAnimationQueue.Dequeue ();
				if (currentUIAnimation.loopCount != 0)
				{
					UIAnimation_Base newUIAnimation = currentUIAnimation.GetNewAnimation ();
					if (newUIAnimation.loopCount > 0)
						newUIAnimation.loopCount--;
					currentUIAnimationQueue.Enqueue (newUIAnimation);
				}
				else
				{
					//Calls the callback
					if (currentUIAnimation.onCompleteAction != null)
						currentUIAnimation.onCompleteAction ();
				}
			}
			//Update After
			if (!currentUIAnimation.updateBefore)
				currentUIAnimation.UpdateAnimation ();
		}
	
		////////////////////////////////////////
		//
		// Function Functions

	}
}
