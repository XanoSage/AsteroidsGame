﻿// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class FileUtilities
{

	public static string CombinePathes(string path1, string path2)
	{
		if (path1[path1.Length - 1] == '\\')
		{
			char[] charArr = path1.ToCharArray();
			charArr[path1.Length - 1] = '/';
			path1 = new string(charArr);
		}
		if (path1[path1.Length - 1] != '/')
			return path1 + "/" + path2;
		else
			return path1 + path2;
	}

	public static string CompinePathes(params string[] paths)
	{
		if (paths == null || paths.Length <= 0)
			return string.Empty;

		var res = paths[0];
		for (int i = 1; i < paths.Length; ++i)
			res = CombinePathes(res, paths[i]);

		return res;
	}
}


