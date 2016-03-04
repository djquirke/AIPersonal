using UnityEngine;
using System.Collections;

public class EyeScript : MonoBehaviour {

	public GameObject ball;
	public GameObject proxy;

	public GameObject eyeball;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		eyeball.transform.localPosition = (ball.transform.position - proxy.transform.position).normalized * 0.1f; 

		Debug.DrawLine(ball.transform.position, transform.position);
	}
}
