using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour {

	public Slider musicSlider;
	public Slider difficultySlider;

	private LevelManager levelManager = LevelManager.LEVELMANAGER;

	public void Awake(){
		musicSlider.value = PlayerPrefsManager.GetMasterVolume();
		difficultySlider.value = PlayerPrefsManager.GetDifficulty();
	}

	void OnDisabled(){
		musicSlider.onValueChanged.RemoveAllListeners ();
	}

	void OnDestroy(){
		musicSlider.onValueChanged.RemoveAllListeners ();
	}

	public void SaveAndExit(){
		if (musicSlider.value != PlayerPrefsManager.GetMasterVolume ()) {
			PlayerPrefsManager.SetMasterVolume (musicSlider.value);
		}
		if (difficultySlider.value != PlayerPrefsManager.GetDifficulty ()) {
			PlayerPrefsManager.SetDifficulty ((int)difficultySlider.value);
		}
		if (levelManager) {
			levelManager.LoadLevel ("_Start");
		}
	}

	public void ResetAndExit(){
		PlayerPrefs.DeleteAll ();
		PlayerPrefsManager.CheckSettings ();

		if (levelManager) {
			levelManager.LoadLevel ("_Start");
		}
	}

	public void SetDefaults(){
		musicSlider.value = 1f;
		difficultySlider.value = 2f;
	}
}