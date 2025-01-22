using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class CanvasFrame2DMode
{
	static CanvasFrame2DMode()
	{
		Selection.selectionChanged += OnSelectionChanged; //add my OnSelectionChanged to Unity's selectionChanged event
	}

	private static void OnSelectionChanged()
	{
		if(Selection.activeGameObject != null)
		{
			GameObject gameObject = Selection.activeGameObject;
			Canvas canvas = gameObject.GetComponent<Canvas>();
			bool isGOCanvas = true;

			while(canvas == null && gameObject.transform.parent != null)
			{
				gameObject = gameObject.transform.parent.gameObject;
				canvas = gameObject.GetComponent<Canvas>();
				isGOCanvas = false;
			}

			if(canvas != null)
			{
				SceneView.lastActiveSceneView.in2DMode = true;
				if(isGOCanvas) //only frame if we selected Canvas, not its children
				{
					RectTransform rectTransform = canvas.GetComponent<RectTransform>();
					Vector3 sizeDelta = (Vector3)rectTransform.sizeDelta;
					Bounds bounds = new Bounds(rectTransform.position, sizeDelta/2);
					SceneView.lastActiveSceneView.Frame(bounds, true);
				}
			}
			else if(SceneView.lastActiveSceneView.in2DMode)
			{
				SceneView.lastActiveSceneView.in2DMode = false;
			}
		}
	}

}
