using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CornellTech.View
{
	/// <summary>
	/// This class is reponsible for displaying an interactive 2D UI that displays thumbnails of 3D models.
	/// </summary>
	public class ModelBrowser : MonoBehaviour
	{
		//Serialized
		[SerializeField]
		protected Transform menuParent;
		[SerializeField]
		protected GameObject elementPrefab;
		[SerializeField]
		protected Text attributionText;
		
		//Actions/Funcs
		public Action<ModelViewData> ModelViewDataSelected;

		///////////////////////////////////////////////////////////////////////////
		//
		// Inherited from MonoBehaviour
		//
		
		protected void Awake ()
		{

		}
		
		protected void Start ()
		{	
			attributionText.text = "";
		}
		
		protected void Update ()
		{	

		}
		
		///////////////////////////////////////////////////////////////////////////
		//
		// ModelBrowser Functions
		//

		public void DisplayModels (List<ModelViewData> modelViewDataList)
		{
			for (int i = 0; i < modelViewDataList.Count; i++)
			{
				DisplayModel (modelViewDataList [i]);
			}

			elementPrefab.SetActive (false);
		}

		protected void DisplayModel (ModelViewData modelViewData)
		{
			GameObject assetMenuItemGameObject = GameObject.Instantiate (elementPrefab, menuParent);

			ModelBrowserElement modelBrowserElement = assetMenuItemGameObject.GetComponent<ModelBrowserElement> ();
			modelBrowserElement.SetData (modelViewData);
			modelBrowserElement.ClickedAction += OnElementClicked;
		}

		protected void DisplayAttributionText (ModelViewData modelViewData)
		{
			string text = string.Format ("{0} by {1}", modelViewData.name, modelViewData.authorName);
			attributionText.text = text;
		}

		////////////////////////////////////////
		//
		// Event Functions

		protected void OnElementClicked (ModelBrowserElement asset)
		{
			if (ModelViewDataSelected != null)
				ModelViewDataSelected (asset.ViewData);

			DisplayAttributionText (asset.ViewData);
		}
	}
}