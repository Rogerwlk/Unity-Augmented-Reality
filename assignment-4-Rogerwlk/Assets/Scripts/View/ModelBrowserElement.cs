using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CornellTech.View
{
	/// <summary>
	/// This class represents a single menu element with a thumbnail image and button.
	/// </summary>
	public class ModelBrowserElement : MonoBehaviour
	{
		/////Protected/////
		//References
		protected ModelViewData viewData;
		protected RawImage rawImage;
		protected Button button;
		//Primitives
		
		//Actions/Funcs
		public Action<ModelBrowserElement> ClickedAction;

		//Properties
		public ModelViewData ViewData {
			get {
				return this.viewData;
			}
		}

		///////////////////////////////////////////////////////////////////////////
		//
		// Inherited from MonoBehaviour
		//
		
		protected void Awake ()
		{
			rawImage = GetComponentInChildren<RawImage> ();
			button = GetComponentInChildren<Button> ();
			button.onClick.AddListener (OnClicked);
		}
		
		protected void Start ()
		{	

		}
		
		protected void Update ()
		{	

		}
		
		///////////////////////////////////////////////////////////////////////////
		//
		// ModelBrowserElement Functions
		//

		public void SetData(ModelViewData viewData)
		{
			this.viewData = viewData;
			rawImage.texture = viewData.thumbnailTexture;
		}
		
		////////////////////////////////////////
		//
		// Event Functions

		protected void OnClicked()
		{
			if (ClickedAction != null)
				ClickedAction (this);
		}

	}
}