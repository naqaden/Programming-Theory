using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
	// ENCAPSULATION
	public float scrollSpeedX = 0.0f;
	public float scrollSpeedY = 0.0f;
	#pragma warning disable CS0108 // Member hides deprecated inherited member;
	private Renderer renderer;
	private Vector2 offset = Vector2.zero;

	// Start is called before the first frame update
	void Start()
	{
		renderer = GetComponent<Renderer>();
	}

    // Update is called once per frame
    void Update()
	{
		offset.x += scrollSpeedX * Time.deltaTime;
		offset.y += scrollSpeedY * Time.deltaTime;
		renderer.material.mainTextureOffset = offset;
	}
}
