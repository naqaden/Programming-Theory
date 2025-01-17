using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	// ENCAPSULATION
	private Transform playerBody;
	public float mouseSensitivity = 500f;
	private float xRotation = 0f;

	// Start is called before the first frame update
	void Start()
	{
		playerBody = transform;

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	// Update is called once per frame
	void Update()
	{
		if(Application.isFocused)
		{
			// Rotate the player body horizontally
			float mouseX = UnityEngine.Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
			playerBody.Rotate(Vector3.up * mouseX);

			// Rotate the camera vertically (clamp rotation to avoid flipping)
			float mouseY = UnityEngine.Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
			xRotation -= mouseY;
			xRotation = Mathf.Clamp(xRotation, -90f, 90f);
			Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
		}
	}
}
