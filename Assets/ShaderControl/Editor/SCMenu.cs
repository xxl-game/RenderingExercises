using UnityEngine;
using UnityEditor;
using System;

namespace ShaderControl
{
	public class SCMenu : UnityEditor.Editor
	{
		[MenuItem ("Assets/Browse Shaders...", false, 200)]
		static void BrowseShaders (MenuCommand command)
		{
			SCWindow.ShowWindow();
		}

	}
}
