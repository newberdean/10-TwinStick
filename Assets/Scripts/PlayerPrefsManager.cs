using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour {

	const string MASTER_VOLUME_KEY = "master_volume";
	const string DIFFICULTY_KEY = "difficulty";
	const string LEVEL_KEY = "level_unlocked_";
	const string MASTER_VOLUME_KEY_SET_KEY = "master_volume_set";
	const string DIFFICULTY_KEY_SET_KEY = "difficulty_set";

	public void Awake(){
		CheckSettings();
	}

	public static void CheckSettings(){
		if (PlayerPrefs.GetInt(MASTER_VOLUME_KEY_SET_KEY) == 0) {
			PlayerPrefs.SetFloat (MASTER_VOLUME_KEY, 0.5f);
		}
		if (PlayerPrefs.GetInt(DIFFICULTY_KEY_SET_KEY) == 0) {
			PlayerPrefs.SetInt (DIFFICULTY_KEY, 1);
		}
		for (int i = 0; i < 5; i++) {
			if (PlayerPrefs.GetInt (LEVEL_KEY + i.ToString()) == 0) {
				PlayerPrefs.SetInt (LEVEL_KEY + i.ToString(), 1);
			}
		}
		if (PlayerPrefs.GetInt(LEVEL_KEY + 14.ToString()) == 0) {
			PlayerPrefs.SetInt (LEVEL_KEY + 14.ToString(), 1);
		}
		if (PlayerPrefs.GetInt(LEVEL_KEY + 15.ToString()) == 0) {
			PlayerPrefs.SetInt (LEVEL_KEY + 15.ToString(), 1);
		}
	}

	public static void SetMasterVolume(float volume){
		if(volume >= 0f && volume <= 1f){
			PlayerPrefs.SetFloat (MASTER_VOLUME_KEY, volume);
			PlayerPrefs.SetInt (MASTER_VOLUME_KEY_SET_KEY, 1);
		}
		else{
			Debug.LogError ("ERROR: PlayerPrefsManager.SetMasterVolume " +
							"ONLY accepts float values between 0f and 1f." +
							"You supplied a value of " + volume.ToString() + ", which " +
							"is outside this range.");
		}
	}

	public static float GetMasterVolume(){
		return PlayerPrefs.GetFloat (MASTER_VOLUME_KEY);
	}

	public static void UnlockLevel(int level){
		if (level >= 0 && level < LevelManager.LEVELCOUNT){
			PlayerPrefs.SetInt (LEVEL_KEY + level.ToString(), 1);
		}
		else{
			Debug.LogError ("ERROR: PlayerPrefsManager.UnlockLevel is attempting to " +
							"unlock a scene that is NOT in the build order (There are " + 
							LevelManager.LEVELCOUNT.ToString() + " Scenes in the build order, and " +
							"you supplied a value of " + level.ToString() + ", which is outside the " +
							"build order).");
		}
	}

	public static void LockLevel(int level){
		if (level >= 0 && level < LevelManager.LEVELCOUNT){
			PlayerPrefs.SetInt (LEVEL_KEY + level.ToString(), 0);
		}
		else{
			Debug.LogError ("ERROR: PlayerPrefsManager.UnlockLevel is attempting to " +
				"lock a scene that is NOT in the build order (There are " + 
				LevelManager.LEVELCOUNT.ToString() + " Scenes in the build order, and " +
				"you supplied a value of " + level.ToString() + ", which is outside the " +
				"build order).");
		}
	}

	public static void LockLevels(){
		for (int i = 0; i < LevelManager.LEVELCOUNT; i++){
			PlayerPrefs.SetInt (LEVEL_KEY + i.ToString(), 0);
		}
		for (int i = 0; i < 5; i++) {
			PlayerPrefs.SetInt (LEVEL_KEY + i.ToString(), 1);
		}
	}

	public static bool IsLevelUnlocked(int level){
		int levelValue = PlayerPrefs.GetInt (LEVEL_KEY + level.ToString ());
		bool isLevelUnlocked = levelValue == 1;

		if (level >= 0 && level < LevelManager.LEVELCOUNT){
			return isLevelUnlocked;
		}
		else{
			Debug.LogError ("ERROR: PlayerPrefsManager.IsLevelUnlocked is attempting to " +
				"check if a scene that is NOT in the build order is unlocked (There are " + 
				LevelManager.LEVELCOUNT.ToString() + " Scenes in the build order, and " +
				"you supplied a value of " + level.ToString() + ", which is outside the " +
				"build order).");
			return false;
		}
	}

	public static void SetDifficulty(int difficulty){
		if(difficulty >= 1 && difficulty <= 3){
			PlayerPrefs.SetInt (DIFFICULTY_KEY, difficulty);
			PlayerPrefs.SetInt (DIFFICULTY_KEY_SET_KEY, 1);
		}
		else{
			Debug.LogError ("ERROR: PlayerPrefsManager.SetDifficulty " +
				"ONLY accepts int values between 1 and 3." +
				"You supplied a value of " + difficulty.ToString() + ", which " +
				"is outside this range.");
		}
	}

	public static int GetDifficulty(){
		return PlayerPrefs.GetInt (DIFFICULTY_KEY);
	}

	public static void ResetLevels (){
		LockLevels ();
	}
}
