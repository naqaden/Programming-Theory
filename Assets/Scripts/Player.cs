using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
	private Score score;

	private Transform playerBody;
	private GameObject haloBow;
	public GameObject projectile;
	public GameObject projectileHoly;
	private Projectile projectileScript;
	private bool isHoly = false;

	public float mouseSensitivity = 500f; //TODO add pause screen to adjust this
	private float xRotation = 0f;

	// Start is called before the first frame update
	void Start()
	{
		score = EventSystem.current.GetComponent<Score>();

		playerBody = transform;
		haloBow = GameObject.Find("HaloBow");
		projectileScript = haloBow.GetComponentInChildren<Projectile>();

		Cursor.lockState = CursorLockMode.Locked;
		//Cursor.visible = false;
	}

	// Update is called once per frame
	void Update()
	{
		if(Application.isFocused && !Cursor.visible)
		{
			handleCamera();
			handleBow();
		}
		//TODO: handle this differently for pause screen
		else if(Application.isFocused)
		{
			Cursor.visible = false;
		}
	}

	// ABSTRACTION
	private void handleCamera()
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

	// ABSTRACTION
	private void handleBow()
	{
		if(Input.GetMouseButton(0) && !projectileScript.getNocking() && !projectileScript.getDrawing()
			&& score.getScore() >= projectileScript.getDrawCost())
		{
			projectileScript.drawProjectile();
		}
		else if(Input.GetMouseButton(1) && !projectileScript.getNocking() && !projectileScript.getDrawing()
			 && score.getScore() >= projectileScript.getDrawCost() + 1) //TODO get holy cost
		{
			isHoly = true;
			GameObject newProjectile = Instantiate(projectileHoly, haloBow.transform);
			Destroy(projectileScript.gameObject);
			projectileScript = newProjectile.GetComponent<Projectile>(); //INHERITANCE
			projectileScript.drawProjectile();
		}
		else if(((Input.GetMouseButtonUp(0) && !isHoly) || (Input.GetMouseButtonUp(1) && isHoly)) && projectileScript.getDrawing())
		{
			score.updateScore(-projectileScript.getDrawCost()); //costs to shoot
			projectileScript.shootProjectile(); //ends charging
			//let projectile orient itself. it's a headache setting pos and rot from here
			GameObject newProjectile = Instantiate(projectile, haloBow.transform);
			projectileScript = newProjectile.GetComponent<Projectile>();
			isHoly = false;
		}
	}
}
