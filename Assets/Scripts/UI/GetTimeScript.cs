using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetTimeScript : MonoBehaviour {

	GameManager gm;
	Text txt;

	// Use this for initialization
	void Start () {
		gm = GameManager.GAMEMANGER;
		txt = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (gm && txt) {
			float time = GameManager.TIME_LEFT;
			if (GameManager.LEVEL_DONE)
				txt.color = Color.green;
			else if (time > 10 && time <= 30)
				txt.color = Color.yellow;
			else if (time > 0 && time <= 10)
				txt.color = Color.red;
			else if (time <= 0)
				txt.color = Color.black;
			if (time > 0)
				txt.text = (Mathf.FloorToInt (time / 60)).ToString ("00") + ":" + (Mathf.FloorToInt (time % 60)).ToString ("00");
			else
				txt.text = "-:--";
		}
	}
}
