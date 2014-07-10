using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {
	public float forwardSpeed;
	
	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		rigidbody2D.velocity = transform.up * Input.GetAxis("Vertical") * forwardSpeed;
		transform.Rotate(0, 0, -Input.GetAxis("Horizontal") * 1.2f);
	}
}
