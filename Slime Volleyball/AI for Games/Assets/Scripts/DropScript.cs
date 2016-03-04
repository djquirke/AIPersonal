using UnityEngine;
using System.Collections;

public class DropScript : MonoBehaviour {
	public GameObject ball;
	public GameObject slime1;
	public GameObject slime2;
	Vector3 ballT;
	Vector3 slime1T;
	Vector3 slime2T;

	// Use this for initialization
	void Start () {
		ballT = ball.transform.position;
		slime1T = slime1.transform.position;
		slime2T = slime2.transform.position;
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.Equals(ball)){
			if (ball.transform.position.x > 0) 
				ball.transform.position = new Vector3(-5f,4f,0f);
			else 
				ball.transform.position = ballT;
			ball.rigidbody.velocity = Vector3.zero;
			slime1.transform.position = slime1T;
			slime1.rigidbody.velocity = Vector3.zero;
			slime2.transform.position = slime2T;
			slime2.rigidbody.velocity = Vector3.zero;
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
