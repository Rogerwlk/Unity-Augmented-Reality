using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CornellTech.Util {
	public class CaptureScreenshotUtility : MonoBehaviour {
		//readonly

		//Serialized
		public int scale = 2 ;

		
		/////Protected/////
		//References
		//Primitives
		

		///////////////////////////////////////////////////////////////////////////
		//
		// Inherited from MonoBehaviour
		//
		
		protected void Awake () {

		}

		protected void Start () {
			
		}

		protected void Update () {

			if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
				CaptureScreenshot ();
		}

		///////////////////////////////////////////////////////////////////////////
		//
		// TimeSkipUtility Functions
		//

		[EditorButtonAttribute]
		protected void CaptureScreenshot () {

			DateTime currentDateTime = System.DateTime.Now;
			string dateTimeString = "on_" + currentDateTime.ToShortDateString ().Replace ('/', '_') + "_at_" + currentDateTime.ToLongTimeString ().Replace (':', '_');
			string screenshotPath = string.Format ("Screenshot_{0}.png", dateTimeString);
			ScreenCapture.CaptureScreenshot (screenshotPath, scale);
		}
		////////////////////////////////////////
		//
		// Function Functions

	}
}
