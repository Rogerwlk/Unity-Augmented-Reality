using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using PolyToolkit;

namespace CornellTech.Model
{
	/// <summary>
	/// PolyDataProvider is a wrapper responsible for communicating with the Google Poly API.
	/// </summary>
	public class PolyDataProvider : MonoBehaviour
	{
		/////Protected/////
		//References
		protected Dictionary<string,PolyAsset> polyAssetDictionary = new Dictionary<string, PolyAsset> ();

		///////////////////////////////////////////////////////////////////////////
		//
		// Inherited from MonoBehaviour
		//
		
		protected void Awake ()
		{

		}
		
		protected void Start ()
		{	
			
		}
		
		protected void Update ()
		{	

		}
		
		///////////////////////////////////////////////////////////////////////////
		//
		// PolyDataProvider Functions
		//

		public PolyAsset GetPolyAssetWithName (string name)
		{
			if (polyAssetDictionary.ContainsKey (name))
				return polyAssetDictionary [name];
			else
			{
				Debug.Log ("We don't have a PolyAsset with that name!");
				return null;
			}
		}

		////////////////////////////////////////
		//
		// List Assets 

		public void ListAssets (int count, Action<List<PolyAsset>> callback)
		{
			PolyListAssetsRequest request = PolyListAssetsRequest.Featured ();
			request.maxComplexity = PolyMaxComplexityFilter.SIMPLE;
			request.pageSize = count;
			PolyApi.ListAssets (request, (result) =>
			{
				OnAssetsListed (result, callback);
			});
		}

		protected void OnAssetsListed (PolyStatusOr<PolyListAssetsResult> result, Action<List<PolyAsset>> callback)
		{
			if (result.Ok)
			{
				for (int i = 0; i < result.Value.assets.Count; i++)
				{
					string name = result.Value.assets [i].name;
					if (!polyAssetDictionary.ContainsKey (name))
						polyAssetDictionary.Add (name, result.Value.assets [i]);
				}
				callback (result.Value.assets);
			}
			else
			{
				Debug.Log (string.Format ("Failed to list assets; result status: {0}. Returning null.", result.Status));
				callback (null);
			}
		}

		////////////////////////////////////////
		//
		// Fetch Thumbnails

		public void FetchThumbnails (List<PolyAsset> assets, Action<List<PolyAsset>> callback)
		{
			StartCoroutine (FetchThumbnailsCoroutine (assets, callback));
		}

		protected IEnumerator FetchThumbnailsCoroutine (List<PolyAsset> assets, Action<List<PolyAsset>> callback)
		{
			int loadedThumbnailCount = 0;

			for (int i = 0; i < assets.Count; i++)
			{
				int index = i;
				FetchThumbnail (assets [index], (success) =>
				{
					string assetDisplayName = assets [index].displayName;
					if (success)
					{
						loadedThumbnailCount++;
					}
					else
					{
						Debug.Log ("Failed to load asset thumbnail: " + assetDisplayName);
						loadedThumbnailCount = -1;
					}
				});
			}
			yield return new WaitUntil (() => (loadedThumbnailCount == assets.Count) || (loadedThumbnailCount == -1));

			if (loadedThumbnailCount == -1)
			{
				OnAssetThumbnailsFetched (false, assets, callback);
			}
			else
			{
				OnAssetThumbnailsFetched (true, assets, callback);
			}

		}

		protected void FetchThumbnail (PolyAsset assetToFetch, Action<bool> callback)
		{
			PolyApi.FetchThumbnail (assetToFetch, (PolyAsset fetchedAsset, PolyStatus status) =>
			{
				callback (status.ok);
			});
		}

		protected void OnAssetThumbnailsFetched (bool success, List<PolyAsset> assets, Action<List<PolyAsset>> callback)
		{
			if (success)
			{
				callback (assets);
			}
			else
			{
				Debug.Log ("Failed to fetch asset thumnails. Returning assets.");
				callback (assets);
			}
		}

		////////////////////////////////////////
		//
		// Import Asset

		public void ImportAsset (PolyAsset asset, Action<GameObject> callback)
		{
			// Set the import options.
			PolyImportOptions options = PolyImportOptions.Default ();
			// We want to rescale the imported mesh to a specific size.
			options.rescalingMode = PolyImportOptions.RescalingMode.FIT;
			// The specific size we want assets rescaled to.
			options.desiredSize = .1f;
			// We want the imported assets to be recentered such that their centroid coincides with the origin.
			options.recenter = true;

			PolyApi.Import (asset, options, (importedAsset, result) =>
			{
				OnAssetImported (importedAsset, result, callback);
			});
		}

		protected void OnAssetImported (PolyAsset asset, PolyStatusOr<PolyImportResult> result, Action<GameObject> callback)
		{
			if (result.Ok)
			{
				callback (result.Value.gameObject);
			}
			else
			{
				Debug.Log (string.Format ("Failed to import asset; result status: {0}. Returning null.", result.Status));
				callback (null);
			}
		}
	}
}