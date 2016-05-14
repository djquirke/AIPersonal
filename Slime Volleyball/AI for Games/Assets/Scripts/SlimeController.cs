using UnityEngine;
using System.Collections;

public class SlimeController : MonoBehaviour {
	bool jumping = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.A)){
			transform.Translate(0.075f*Vector3.left);
		}
		if (Input.GetKey(KeyCode.D)){
			transform.Translate(0.075f*Vector3.right);
		}
		if (Input.GetKeyDown(KeyCode.W) && !jumping){
			jumping = true;
			rigidbody.AddForce(175f*Vector3.up,ForceMode.Acceleration);
		}
	}

	public void stoppedJumping()
	{
		jumping = false;
	}
}
