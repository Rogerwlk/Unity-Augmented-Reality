using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using CornellTech.View;
using CornellTech.Model;
using PolyToolkit;

namespace CornellTech.Controller
{
	/// <summary>
	/// Scene controller.
	/// This class is responsible for managing the scene, and routing communication between the Model and View.
	/// </summary>
	public class SceneController : MonoBehaviour
	{
		//Serialized Fields
		[Header("Model")]
		[SerializeField]
		protected PolyDataProvider polyDataProvider;

		[Header("View")]
		[SerializeField]
		protected ModelBrowser modelBrowser;

		[SerializeField]
		protected PhysicsItemManager physicsItemManager;

		[SerializeField]
		protected Button restartSceneButton;
		
		/////Protected/////
		//References
		protected Dictionary<string,ModelViewData> modelViewDataDictionary = new Dictionary<string, ModelViewData>();

		///////////////////////////////////////////////////////////////////////////
		//
		// Inherited from MonoBehaviour
		//
		// Read more about these events here: https://docs.unity3d.com/Manual/ExecutionOrder.html
		// 
		
		protected void Awake()
		{
			modelBrowser.ModelViewDataSelected += OnModelViewDataSelected;
			restartSceneButton.onClick.AddListener(OnResetSceneButtonClicked);
		}

		protected void Start()
		{	
			LoadPolyAssets();
		}

		protected void Update()
		{	

		}

		///////////////////////////////////////////////////////////////////////////
		//
		// SceneController Functions
		//

		protected void LoadPolyAssets()
		{
			polyDataProvider.ListAssets(12, OnPolyAssetsListed);
		}

		////////////////////////////////////////
		//
		// Event Functions

		protected void OnPolyAssetsListed(List<PolyAsset> assets)
		{
			if (assets == null)
				return;
			
			polyDataProvider.FetchThumbnails(assets, OnPolyThumbnailsFetched);
		}

		protected void OnPolyThumbnailsFetched(List<PolyAsset> assets)
		{
			if (assets == null)
				return;
			
			List<ModelViewData> modelViewDataList = new List<ModelViewData>();
			for (int i = 0; i < assets.Count; i++) {
				PolyAssetData polyAssetData = new PolyAssetData(assets[i]);
				ModelViewData modelViewData = Convert(polyAssetData);
				modelViewDataList.Add(modelViewData);
			}
			modelBrowser.DisplayModels(modelViewDataList);
		}

		protected void OnModelViewDataSelected(ModelViewData modelViewData)
		{
			PolyAsset polyAsset = polyDataProvider.GetPolyAssetWithName(modelViewData.ID);
			polyDataProvider.ImportAsset(polyAsset, OnPolyAssetImported);
		}

		protected void OnPolyAssetImported(GameObject gameObject)
		{
			if (gameObject == null)
				return;
			
			physicsItemManager.SetupGameObject(gameObject);
		}

		protected void OnResetSceneButtonClicked()
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(0);
		}

		////////////////////////////////////////
		//
		// Model To View Conversion

		//Converts model object to view object
		protected ModelViewData Convert(PolyAssetData polyAssetData)
		{
			string id = GetModelViewDataID(polyAssetData);

			ModelViewData modelViewData = null;

			if (modelViewDataDictionary.ContainsKey(id))
				modelViewData = modelViewDataDictionary[id];
			else {
				modelViewData = new ModelViewData();
				modelViewDataDictionary.Add(id, modelViewData);
			}

			Update(modelViewData, polyAssetData);

			return modelViewData;
		}

		//Updates view object from model object
		protected void Update(ModelViewData modelViewData, PolyAssetData polyAssetData)
		{
			modelViewData.ID = polyAssetData.polyAsset.name;
			modelViewData.name = polyAssetData.polyAsset.displayName;
			modelViewData.authorName = polyAssetData.polyAsset.authorName;
			modelViewData.thumbnailTexture = polyAssetData.polyAsset.thumbnailTexture;
		}

		//Gets a unique ID from a PolyAssetData object
		protected string GetModelViewDataID(PolyAssetData polyAssetData)
		{
			return polyAssetData.polyAsset.name;
		}
	}
}

namespace CornellTech.View
{
	public class ModelViewData
	{
		//Unique ID
		public string ID;
		//display name
		public string name;
		//author name
		public string authorName;
		//thumbnail
		public Texture2D thumbnailTexture;
	}

}

namespace CornellTech.Model
{
	public class PolyAssetData
	{
		public PolyAsset polyAsset;

		public PolyAssetData(PolyAsset polyAsset)
		{
			this.polyAsset = polyAsset;
		}
		
	}
}