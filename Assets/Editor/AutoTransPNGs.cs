using UnityEngine;
using UnityEditor;
using UnityEditor.VersionControl;

public class TextureImporterSettings:AssetPostprocessor
{
	void OnPreprocessTexture()
	{
		TextureImporter textureImporter = (TextureImporter)assetImporter;

		if(textureImporter != null && assetPath.ToLower().Contains(".png"))
		{
			// Ensure "Alpha is Transparency" is checked
			textureImporter.alphaIsTransparency = true;
			Debug.Log(textureImporter.assetPath + " transparency activated");
			//Debug.unityLogger.logHandler.LogFormat(LogType.Log, null, textureImporter.assetPath + " transparency activated");

			// Optionally, you can also set other default settings
			//textureImporter.textureType = TextureImporterType.Default;
			//textureImporter.alphaSource = TextureImporterAlphaSource.FromInput;
		}
	}
}