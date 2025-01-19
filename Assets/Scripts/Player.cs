using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	// ENCAPSULATION
	private Transform playerBody;
	private GameObject haloBow;
	public GameObject projectile;
	private Projectile projectileScript;

	public float mouseSensitivity = 500f;
	private float xRotation = 0f;

	// Start is called before the first frame update
	void Start()
	{
		playerBody = transform;
		haloBow = GameObject.Find("HaloBow");
		projectileScript = haloBow.GetComponentInChildren<Projectile>();

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	// Update is called once per frame
	void Update()
	{
		if(Application.isFocused && !Cursor.visible)
		{
			// Rotate the player body horizontally
			float mouseX = UnityEngine.Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
			playerBody.Rotate(Vector3.up * mouseX);

			// Rotate the camera vertically (clamp rotation to avoid flipping)
			float mouseY = UnityEngine.Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
			xRotation -= mouseY;
			xRotation = Mathf.Clamp(xRotation, -90f, 90f);
			Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

			if(Input.GetMouseButton(0) && !projectileScript.getNocking() && !projectileScript.getDrawing())
			{
				projectileScript.drawProjectile();
			}
			else if(Input.GetMouseButtonUp(0) && projectileScript.getDrawing())
			{
				projectileScript.shootProjectile(); //ends charging
				//let projectile orient itself. it's a headache setting pos and rot from here
				GameObject newProjectile = Instantiate(projectile, haloBow.transform);
				projectileScript = newProjectile.GetComponent<Projectile>();
			}
		}
		//TODO: handle this differently for pause screen
		else if(Application.isFocused)
		{
			Cursor.visible = false;
		}
	}
}
