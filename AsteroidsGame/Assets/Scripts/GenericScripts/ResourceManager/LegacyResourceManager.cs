using UnityEngine;
using System.Collections.Generic;

public static class LegacyResourceManager
{
	private static List<string> _singleSceneAssets = new List<string>();
	private static Dictionary<string, Object> _totalAssetsLoaded = new Dictionary<string, Object>();

	/// <summary>
	/// Unload resourses for object.
	/// </summary>
	/// <returns><c>true</c>, if unload resourses for object was used, <c>false</c> otherwise.</returns>
	/// <param name="objectName">Object name.</param>
	public static bool UnloadResoursesForObject(string objectName)
	{
		if (!_totalAssetsLoaded.ContainsKey(objectName))
			return false;

		Debug.Log("Unloading resources for object: " + objectName);

		Object objectToUnload = _totalAssetsLoaded[objectName];

		if ((objectToUnload as GameObject) == null)
			Resources.UnloadAsset(objectToUnload);

		objectToUnload = null;
		_totalAssetsLoaded.Remove(objectName);

		return true;
	}

	/// <summary>
	/// Loads the resources for object.
	/// </summary>
	/// <returns>The resources for object.</returns>
	/// <param name="objectName">Object name.</param>
	/// <param name="resourcePath">Resource path.</param>
	/// <param name="shouldBeUnloadedBetweenScenes">If set to <c>true</c> should be unloaded between scenes.</param>
	public static T LoadResourcesForObject<T>(string objectName, string resourcePath, bool shouldBeUnloadedBetweenScenes)
		where T : Object
	{
		UnityEngine.Object res;
		if (_totalAssetsLoaded.TryGetValue(objectName, out res))
		{
			if (res != null)
				return (T)res;

			_totalAssetsLoaded.Remove(objectName);
		}

		string path = FileUtilities.CombinePathes(resourcePath, objectName);

		var loadedObject = Resources.Load<T>(path);
		if (loadedObject != null)
		{
			_totalAssetsLoaded.Add(objectName, loadedObject);

			if (shouldBeUnloadedBetweenScenes)
				_singleSceneAssets.Add(objectName);
		}

		return loadedObject;
	}

	/// <summary>
	/// Loads the resources for object.
	/// </summary>
	/// <returns>The resources for object.</returns>
	/// <param name="objectName">Object name.</param>
	/// <param name="resourcePath">Resource path.</param>
	/// <param name="shouldBeUnloadedBetweenScenes">If set to <c>true</c> should be unloaded between scenes.</param>
	public static Object LoadResourcesForObject(string objectName, string resourcePath, bool shouldBeUnloadedBetweenScenes = true)
	{
		return LoadResourcesForObject<Object>(objectName, resourcePath, shouldBeUnloadedBetweenScenes);
	}

	/// <summary>
	/// Raises the scene changed event.
	/// </summary>
	public static void OnLevelWasLoaded(int level)
	{
		for (int i = 0; i < _singleSceneAssets.Count; i++)
			UnloadResoursesForObject(_singleSceneAssets[i]);

		_singleSceneAssets.Clear();
	}
}
