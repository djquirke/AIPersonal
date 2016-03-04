using UnityEngine;
using System.Collections;

public class SlimeController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.A)){
			transform.Translate(0.05f*Vector3.left);
		}
		if (Input.GetKey(KeyCode.D)){
			transform.Translate(0.05f*Vector3.right);
		}
		if (Input.GetKeyDown(KeyCode.W)){
			rigidbody.AddForce(250f*Vector3.up,ForceMode.Acceleration);
		}
	}
}
