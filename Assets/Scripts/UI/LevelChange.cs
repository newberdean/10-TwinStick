using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelChange : MonoBehaviour {

	public string level;
	public bool levelButton, resetButton;

	void Start () {
		if (levelButton) {
			GetComponent<Button> ().interactable = PlayerPrefsManager.IsLevelUnlocked (int.Parse (name.Split (' ') [1]) + 3);
		}
		GetComponent<Button> ().onClick.AddListener (EXECUTE);
	}

	void EXECUTE(){
		Time.timeScale = 1f;
		if (resetButton) {
			LevelManager.LEVELMANAGER.ResetLevels ();
		}
		else if (level.Equals ("Quit")) {
			LevelManager.LEVELMANAGER.QuitRequest ();
		} else if (level.Equals ("Retry")) {
			LevelManager.LEVELMANAGER.LoadLevel (LevelManager.lastLevel);
		} else {
			LevelManager.LEVELMANAGER.LoadLevel (level);
		}
	}
}
