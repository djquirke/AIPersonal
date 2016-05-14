using UnityEngine;
using System.Collections;

public class GUIController : MonoBehaviour {

	private int playerScore;
	private int aiScore;

	void OnGUI()
	{
		GUI.TextArea(new Rect(100, 20, 150, 20), "Player Score: " + playerScore.ToString());
		GUI.TextArea(new Rect(Camera.main.pixelWidth - 200, 20, 150, 20), "AI Score: " + aiScore.ToString());
	}

	public void incrementPlayerScore() {playerScore++;}
	public void incrementAIScore() {aiScore++;}
}
