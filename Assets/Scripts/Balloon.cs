using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
	private bool isLost = false;

    // Start is called before the first frame update
    void Start()
    {

	}

	// Update is called once per frame
	void Update()
	{
		if(isLost)
		{
			//TODO: ascend
		}
	}

	//TODO: detect cloud and pop

	public void GetLost()
	{
		transform.SetParent(null);
		isLost = true;
	}
}
