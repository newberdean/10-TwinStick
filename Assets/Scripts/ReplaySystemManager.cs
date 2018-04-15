using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using USACPI = UnityStandardAssets.CrossPlatformInput;

public class ReplaySystemManager : MonoBehaviour {

	public static ReplaySystemManager REWINDMANAGER;
	public static bool Recording = true, Rewinding = false, Replaying = false, LevelEnd = false;

	public GameObject rec, rew, rep, stp, lmt; // Record, Rewind, Replay, Stop, Limit;

	private bool rewindLast = false, replayLast = false, canRewind = true, canReplay = true;
	private CanvasGroup limitGroup;
	private Image imageToEmpty;
	private Text limitText;

	// Use this for initialization
	void Start () {
		REWINDMANAGER = this;
		if (lmt) {
			limitGroup = lmt.GetComponent<CanvasGroup> ();
			imageToEmpty = lmt.transform.GetChild (0).GetComponent<Image> ();
			limitText = lmt.transform.GetChild (1).GetComponent<Text> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (USACPI.CrossPlatformInputManager.GetButtonDown ("Options") && !GameManager.LEVEL_DONE)
			GameManager.TogglePause ();

		if (GameManager.CAN_MANIPULATE_TIME) {
			bool LTrigger = USACPI.CrossPlatformInputManager.GetButton ("Left Trigger 2");
			bool LMouse = USACPI.CrossPlatformInputManager.GetButton ("Fire1");
			bool ReplayButton	= LTrigger || LMouse;

			bool RTrigger = USACPI.CrossPlatformInputManager.GetButton ("Right Trigger 2");
			bool RMouse = USACPI.CrossPlatformInputManager.GetButton ("Fire2");
			bool RewindButton	= RTrigger || RMouse;

			// Check if we are over a UI element, and if we are, check if we are over the pause button.
			if (EventSystem.current.IsPointerOverGameObject ()) {
				PointerEventData pointerData = new PointerEventData (EventSystem.current) {
					position = Input.mousePosition
				};

				List<RaycastResult> results = new List<RaycastResult> ();
				EventSystem.current.RaycastAll (pointerData, results);

				foreach (RaycastResult result in results) {
					if (result.gameObject.CompareTag ("PauseButton")) {
						// We're over the pause button - in this case, swallow the attempted replay/rewind.
						ReplayButton = LTrigger;
						RewindButton = RTrigger;
						break;
					}
				}
			}

			// If we're NOT holding a Replay/Rewind Button, and we're NOT Rewinding/Replaying, we must be Recording.
			Recording = !((ReplayButton && !Rewinding && canReplay) || (RewindButton && !Replaying && canRewind)) || LevelEnd;

			if (ReplayButton && !Rewinding && canReplay && !LevelEnd) {
				Replaying = canReplay = replayLast = true;
				canRewind = rewindLast = false;
				if (lmt)
					SetUpLimit ();
			} else if (RewindButton && !Replaying && canRewind && !LevelEnd) {
				Rewinding = canRewind = rewindLast = true;
				canReplay = replayLast = false;
				if (lmt)
					SetUpLimit ();
			} else {
				Rewinding = Replaying = false;
			}

			if (lmt) {
				if (imageToEmpty.fillAmount > 0) {
					imageToEmpty.fillAmount = Mathf.Max (0f, imageToEmpty.fillAmount - (Time.deltaTime * 2));
				} else if (limitGroup.alpha > 0) {
					limitGroup.alpha = Mathf.Max (0f, limitGroup.alpha - (Time.deltaTime * 2));
				} else {
					canReplay = true;
					canRewind = true;
				}
			}
		} else if (!GameManager.CAN_MANIPULATE_TIME && LevelEnd) {
			Rewinding = Replaying = false;
			Recording = true;
		}
		// UI: Set which indicators show based on status.
		if (rec)
			rec.SetActive (Recording);
		if (rep)
			rep.SetActive (Replaying);
		if (rew)
			rew.SetActive (Rewinding);
		if (stp)
			stp.SetActive (Mathf.Approximately (Time.timeScale, 0f) || ReplaySystem.framesLeft >= ReplaySystem.bufferFrames);
	}

	void SetUpLimit (){
		imageToEmpty.fillAmount = 1f;
		limitGroup.alpha = 1f;
		limitText.text = "CAN'T " + (rewindLast ? "REPLAY" : (replayLast ? "REWIND" : "EVEN"));
		// If user rewound last, then can't replay. If user replayed last, then can't rewind. If error; SASS. JUST SASS.
	}

	public void SetUpLMT (){
		limitGroup = lmt.GetComponent<CanvasGroup> ();
		imageToEmpty = lmt.transform.GetChild (0).GetComponent<Image> ();
		limitText = lmt.transform.GetChild (1).GetComponent<Text> ();
	}
}