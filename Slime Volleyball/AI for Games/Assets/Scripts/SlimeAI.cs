using UnityEngine;
using System.Collections;

enum State
{
	IDLE,
	ATTACK
}

public class SlimeAI : MonoBehaviour {

	public GameObject ball;
	private State state = State.IDLE;
	private Vector3 idle_pos = new Vector3();
	private float attack_offset = 0;
	private float min_x = 1.3f;
	private float max_x = 9.5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		switch (state) {
		case State.IDLE:
			if(ball.transform.position.x > 0)//ball on ai side
			{
				attack_offset = Random.Range(0.25f, 0.8f);
				state = State.ATTACK;
				break;
			}
			if (idle_pos.x < transform.position.x)
				goLeft();
			if (transform.position.x < idle_pos.x)
				goRight();
			break;
		case State.ATTACK:
			if(ball.transform.position.x < 0) //ball on player side
			{
				float x = Random.Range(min_x, max_x - 3);
				idle_pos.x = x;
				state = State.IDLE;
				break;
			}
			if (ball.transform.position.x + attack_offset < transform.position.x)
				goLeft();
			if (ball.transform.position.x > transform.position.x - attack_offset)
				goRight();
			break;
		default:
			break;
		}
	}

	//DO NOT MODIFY LEFT RIGHT AND JUMP FUNCTIONS!
	void goLeft(){
		if (transform.position.x > min_x)
			transform.Translate(0.075f*Vector3.left);
	}
	void goRight(){
		if (transform.position.x < max_x)
			transform.Translate(0.075f*Vector3.right);
	}
	public void jump(){
		if (transform.position.y < 0.1f)
		{
			rigidbody.AddForce(175f*new Vector3(-0.75f, 0.75f, 0),ForceMode.Acceleration);
		}
	}
}
