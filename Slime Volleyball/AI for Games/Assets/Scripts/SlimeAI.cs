using UnityEngine;
using System.Collections;

public class SlimeAI : MonoBehaviour {

	public GameObject ball;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (ball.transform.position.x < transform.position.x)
			goLeft();
		if (ball.transform.position.x > transform.position.x)
			goRight();
		if (Random.Range(1,10)==1)
			jump ();
	}

	//DO NOT MODIFY LEFT RIGHT AND JUMP FUNCTIONS!
	void goLeft(){
		if (transform.position.x > 1.5f)
			transform.Translate(0.075f*Vector3.left);
	}
	void goRight(){
		if (transform.position.x < 8.5f)
			transform.Translate(0.075f*Vector3.right);
	}
	void jump(){
		if (transform.position.y < 0.25f)
			rigidbody.AddForce(125f*Vector3.up,ForceMode.Acceleration);
	}
}
