using UnityEngine;
using System.Collections;

public class JumpTrigger : MonoBehaviour {

	//public bool isFront = false;
	private float time = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Ball"))
		{
			time = 0;
			Jump ();
		}
	}

	void OnTriggerStay(Collider other)
	{
		if(other.CompareTag("Ball"))
		{
			time += Time.deltaTime;
			if(time > 0.5f)
			{
				time = 0;
				Jump();
			}
		}
	}

	void Jump()
	{
		this.transform.root.GetComponent<SlimeAI>().jump ();
		//else this.transform.root.GetComponent<SlimeAI>().jump (new Vector3(1, 1));
	}
}
