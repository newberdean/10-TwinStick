using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using USACPI = UnityStandardAssets.CrossPlatformInput;

public class GameManager : MonoBehaviour {

	public static GameManager GAMEMANAGER;
	public static bool Recording = true, Rewinding = false, Replaying = false;
	public GameObject rec, rew, rep, stp; // Record, Rewind, Replay, Stop;

	// Use this for initialization
	void Start () {
		if (GAMEMANAGER == null) {
			GAMEMANAGER = this;
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		Recording = !(
				USACPI.CrossPlatformInputManager.GetButton ("Left Trigger 2")	||
				USACPI.CrossPlatformInputManager.GetButton ("Right Trigger 2")	||
				USACPI.CrossPlatformInputManager.GetButton("Fire1")				||
				USACPI.CrossPlatformInputManager.GetButton("Fire2")
		);
		Replaying = (
			USACPI.CrossPlatformInputManager.GetButton ("Left Trigger 2") ||
			USACPI.CrossPlatformInputManager.GetButton ("Fire1")
		);
		Rewinding = (
			USACPI.CrossPlatformInputManager.GetButton ("Right Trigger 2") ||
			USACPI.CrossPlatformInputManager.GetButton ("Fire2")
		) && !Replaying;

		if (USACPI.CrossPlatformInputManager.GetButtonDown ("Left Trigger 1")) {
			Time.timeScale = (Mathf.Max(0f, Time.timeScale - 0.1f));
		}

		if (USACPI.CrossPlatformInputManager.GetButtonDown ("Right Trigger 1")) {
			Time.timeScale = (Mathf.Min(100f, Time.timeScale + 0.1f));
		}

		rec.SetActive (Recording);
		rep.SetActive (Replaying);
		rew.SetActive (Rewinding);
		stp.SetActive (Mathf.Approximately (Time.timeScale, 0f) || ReplaySystem.framesLeft >= ReplaySystem.bufferFrames);
	}
}
