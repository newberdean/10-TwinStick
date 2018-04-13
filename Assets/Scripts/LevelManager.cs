using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public float autoLevelChange = 4f;
	public static string lastLevel;

	private static string winScreen = null;
	private static string loseScreen = null;
	private static bool winFound = false;
	private static bool loseFound = false;

	private static int levelCount;
	private static LevelManager lvlMan = null;

	public static int LEVELCOUNT{
		get{ return levelCount; }
	}
	public static LevelManager LEVELMANAGER{
		get{ return lvlMan; }
	}

	void Awake (){
		if (LEVELMANAGER != null) {
			Destroy (gameObject);
		} else {
			lvlMan = this;
			levelCount = SceneManager.sceneCountInBuildSettings;
			DontDestroyOnLoad (gameObject);

			// Find the Seperator Character in SceneUtility.GetScenePathByBuildIndex (?), so we can seperate it later.
			char separator = (SceneUtility.GetScenePathByBuildIndex (1).Contains ("/") ? '/' : '\\');

			for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++) {
				if (winFound && loseFound) {
					break;
				}
				if (!winFound && SceneUtility.GetScenePathByBuildIndex(i).Split(separator)[2].Contains ("Win")) {
					winScreen = SceneUtility.GetScenePathByBuildIndex (i).Split (separator) [2].Split('.')[0];
					winFound = true;
				}
				if (!loseFound && SceneUtility.GetScenePathByBuildIndex(i).Split(separator)[2].Contains ("Lose")) {
					loseScreen = SceneUtility.GetScenePathByBuildIndex(i).Split(separator)[2].Split('.')[0];
					loseFound = true;
				}
			}

			if (!winFound) {
				winScreen = "_Win";
				winFound = true;
			}
			if (!loseFound) {
				loseScreen = "_Lose";
				loseFound = true;
			}
			//Debug.Log ("WINSCREEN FOUND: " + winFound + "\nLOSESCREEN FOUND: " + loseFound);
			//Debug.Log ("WINSCREEN FOUND: " + winScreen + "\nLOSESCREEN FOUND: " + loseScreen);
		}
	}

	void OnEnable(){
		SceneManager.sceneLoaded += LevelLoaded;
	}
	void OnDisable(){
		SceneManager.sceneLoaded -= LevelLoaded;
	}
	void OnDestroy(){
		SceneManager.sceneLoaded -= LevelLoaded;
	}
	
	void LevelLoaded(Scene scene, LoadSceneMode mode){
		GameObject canditate = GameObject.FindGameObjectWithTag ("PauseMenu");
		if (canditate) {
			GameManager.PAUSEMENU = canditate.transform.GetChild (0).gameObject;
			for (int i = 0; i < GameManager.PAUSEMENU.transform.childCount; i++) {
				if (GameManager.PAUSEMENU.transform.GetChild (i).name.Equals ("Hint"))
					GameManager.PAUSEMENU.transform.GetChild (i).GetComponent<UnityEngine.UI.Text> ().text = GameManager.HINTS[int.Parse(scene.name.Split('l')[1])];
			}
		}
		if (scene.name.Contains ("_Level") && !scene.name.Equals ("_Levels")) {
			GameManager.ResetCountdown (60);
			lastLevel = scene.name;
		}
	}

	IEnumerator LoadAfterSeconds(){
		yield return new WaitForSeconds (autoLevelChange);
		LoadNextLevel ();
		yield return null;
	}

	public void LoadLevel(string name){
		if (name.Equals ("Retry")) {
			SceneManager.LoadScene (LevelManager.lastLevel);
		} else if (name.Equals ("Next")) {
			LoadNextLevel ();
		} else {
			SceneManager.LoadScene (name);
		}
	}

	public void QuitRequest(){
		Application.Quit();
	}

	public static void LoadNextLevel(){
		int nextLevel = SceneManager.GetActiveScene ().buildIndex + 1;
		if (!PlayerPrefsManager.IsLevelUnlocked (nextLevel))
			PlayerPrefsManager.UnlockLevel (nextLevel);
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex+1);
	}

	public static void LOSE(){
		if ((!SceneManager.GetActiveScene ().name.Contains ("_Level") || SceneManager.GetActiveScene ().name.Equals ("_Levels")) || 
			(SceneManager.GetActiveScene ().name.Equals ("_Level00")))
			GameManager.ResetCountdown (9999);			
		else {
			if (loseFound) {
				SceneManager.LoadScene (loseScreen);
			} else {
				SceneManager.LoadScene ("_Lose");
			}
		}
	}

	public static void WIN(){
		if (SceneManager.GetSceneByBuildIndex (SceneManager.GetActiveScene ().buildIndex).name.Contains ("Level")) {
			LoadNextLevel ();
		}
		else{
			SceneManager.LoadScene (winFound ? winScreen : "_Win");
		}
	}

	public void ResetLevels (){
		PlayerPrefsManager.ResetLevels ();
	}
}
