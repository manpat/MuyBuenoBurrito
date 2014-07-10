using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {

	public float forwardSpeed;

	public float forward;
	
	// Use this for initialization
	void Start () 
	{
		forward = Input.GetAxis("Vertical");
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Adding velocity in a forward direction
		if (Input.GetKey(KeyCode.UpArrow))
		{
			rigidbody2D.AddForce(transform.up * forward * forwardSpeed);
		}


		// rotating right
		if (Input.GetKey(KeyCode.RightArrow))
		{
			transform.Rotate(0,0,-1.2f);
		}

		// rotating left
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			transform.Rotate(0,0,1.2f);
		}
	}
}
