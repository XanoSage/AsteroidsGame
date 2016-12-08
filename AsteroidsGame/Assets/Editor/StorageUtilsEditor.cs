using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public static class StorageUtilsEditor
{
	[MenuItem("File/Storage/ClearAll")]
	private static void ClearAll()
	{
		ClearAllStorage ();
		PlayerPrefs.DeleteAll();
		Caching.CleanCache ();
	}

	[MenuItem("File/Storage/Open persistent storage location")]
	private static void OpenPersistentStorage()
	{
		EditorUtility.RevealInFinder(Application.persistentDataPath);
	}

	[MenuItem("File/Storage/Open temp folder location")]
	private static void OpenTemptStorage()
	{
		EditorUtility.RevealInFinder(Application.temporaryCachePath);
	}

	[MenuItem ("File/Storage/Clear storage")]
	public static void ClearAllStorage()
	{
		ClearFolder(Application.persistentDataPath);
		ClearFolder(Application.temporaryCachePath);
	}

	private static void ClearFolder(string folderName)
	{
		var dir = new DirectoryInfo(folderName);

		foreach (DirectoryInfo di in dir.GetDirectories())
			di.Delete(true);

		foreach (FileInfo fi in dir.GetFiles())
			fi.Delete();
	}
}
