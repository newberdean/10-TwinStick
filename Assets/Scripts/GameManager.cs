using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using USACPI = UnityStandardAssets.CrossPlatformInput;

public class GameManager : MonoBehaviour {

	public static GameManager GAMEMANAGER;
	public static bool Recording = true, Rewinding = false, Replaying = false;
	public GameObject rec, rew, rep, stp, lmt; // Record, Rewind, Replay, Stop, Limit;

	private bool rewindLast = false, replayLast = false, canRewind = true, canReplay = true;
	private CanvasGroup limitGroup;
	private Image imageToEmpty;
	private Text limitText;

	// Use this for initialization
	void Start () {
		if (GAMEMANAGER == null) {
			GAMEMANAGER = this;
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (gameObject);
		}
		limitGroup = lmt.GetComponent<CanvasGroup> ();
		imageToEmpty = lmt.transform.GetChild (0).GetComponent<Image> ();
		limitText = lmt.transform.GetChild (1).GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		Recording = !(
			((USACPI.CrossPlatformInputManager.GetButton ("Left Trigger 2") || USACPI.CrossPlatformInputManager.GetButton ("Fire1")) && !Rewinding && canReplay) ||
			((USACPI.CrossPlatformInputManager.GetButton ("Right Trigger 2") || USACPI.CrossPlatformInputManager.GetButton ("Fire2")) && !Replaying && canRewind)
		);

		if ((USACPI.CrossPlatformInputManager.GetButton ("Left Trigger 2") || USACPI.CrossPlatformInputManager.GetButton ("Fire1")) && !Rewinding && canReplay) {
			Replaying = canReplay = replayLast = true;
			canRewind = rewindLast = false;
			SetUpLimit ();
		} else if ((USACPI.CrossPlatformInputManager.GetButton ("Right Trigger 2") || USACPI.CrossPlatformInputManager.GetButton ("Fire2")) && !Replaying && canRewind) {
			Rewinding = canRewind = rewindLast = true;
			canReplay = replayLast = false;
			SetUpLimit ();
		} else {
			Rewinding = Replaying = false;
		}

		if (USACPI.CrossPlatformInputManager.GetButton ("Left Trigger 1")) {
			Time.timeScale = (Mathf.Max(0f, Time.timeScale - 0.1f));
		}

		if (USACPI.CrossPlatformInputManager.GetButton ("Right Trigger 1")) {
			Time.timeScale = (Mathf.Min(100f, Time.timeScale + 0.1f));
		}

		rec.SetActive (Recording);
		rep.SetActive (Replaying);
		rew.SetActive (Rewinding);
		stp.SetActive (Mathf.Approximately (Time.timeScale, 0f) || ReplaySystem.framesLeft >= ReplaySystem.bufferFrames);
		if (imageToEmpty.fillAmount > 0) {
			imageToEmpty.fillAmount = Mathf.Max (0f, imageToEmpty.fillAmount - (Time.deltaTime*2));
		} else if (limitGroup.alpha > 0) {
			limitGroup.alpha = Mathf.Max (0f, limitGroup.alpha - (Time.deltaTime*2));
		} else {
			canReplay = true;
			canRewind = true;
		}
	}

	void SetUpLimit (){
		imageToEmpty.fillAmount = 1f;
		limitGroup.alpha = 1f;
		limitText.text = "CAN'T " + (rewindLast ? "REPLAY" : (replayLast ? "REWIND" : "EVEN"));
		// If user rewound last, then can't replay. If user replayed last, then can't rewind. If error; SASS. JUST SASS.
	}
}