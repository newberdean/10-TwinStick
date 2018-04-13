using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using USACPI = UnityStandardAssets.CrossPlatformInput;

public class DebugText : MonoBehaviour {

	private Text debugText;

	// Use this for initialization
	void Start () {
		debugText = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		debugText.text = "Frames Left: " + ReplaySystem.framesLeft + "\n" +
		"Time Scale: " + Time.timeScale + "\n\n" +
		"Horizontal Axis: " + USACPI.CrossPlatformInputManager.GetAxis ("Horizontal") + "\n" +
		"Vertical Axis: " + USACPI.CrossPlatformInputManager.GetAxis ("Vertical") + "\n\n" +
		"Horizontal Right-Stick Axis: " + USACPI.CrossPlatformInputManager.GetAxis ("Right Stick Horizontal") +
		"\nVertical Right-Stick Axis: " + USACPI.CrossPlatformInputManager.GetAxis ("Right Stick Vertical") +
		"\n\nHorizontal D-pad Axis: " + USACPI.CrossPlatformInputManager.GetAxis ("Dpad Horizontal") + "\n" +
		"Vertical D-pad Axis: " + USACPI.CrossPlatformInputManager.GetAxis ("Dpad Vertical") + "\n\n" +
		"Left Trigger 2 Axis: " + USACPI.CrossPlatformInputManager.GetAxis ("Left Trigger 2 Axis") + "\n" +
		"Right Trigger 2 Axis: " + USACPI.CrossPlatformInputManager.GetAxis ("Right Trigger 2 Axis") + "\n\n" +
		"SQUARE: " + USACPI.CrossPlatformInputManager.GetButton ("Square") + "\n" +
		"CROSS: " + USACPI.CrossPlatformInputManager.GetButton ("Cross") + "\n" +
		"CIRCLE: " + USACPI.CrossPlatformInputManager.GetButton ("Circle") + "\n" +
		"TRIANGLE: " + USACPI.CrossPlatformInputManager.GetButton ("Triangle") + "\n" +
		"L1: " + USACPI.CrossPlatformInputManager.GetButton ("Left Trigger 1") + "\n" +
		"R1: " + USACPI.CrossPlatformInputManager.GetButton ("Right Trigger 1") + "\n" +
		"L2: " + USACPI.CrossPlatformInputManager.GetButton ("Left Trigger 2") + "\n" +
		"R2: " + USACPI.CrossPlatformInputManager.GetButton ("Right Trigger 2") + "\n" +
		"SHARE: " + USACPI.CrossPlatformInputManager.GetButton ("Share") + "\n" +
		"OPTIONS: " + USACPI.CrossPlatformInputManager.GetButton ("Options") + "\n" +
		"L3: " + USACPI.CrossPlatformInputManager.GetButton ("Left Trigger 3") + "\n" +
		"R3: " + USACPI.CrossPlatformInputManager.GetButton ("Right Trigger 3") + "\n" +
		"PSN: " + USACPI.CrossPlatformInputManager.GetButton ("PSN") + "\n" +
		"TOUCHPAD: " + USACPI.CrossPlatformInputManager.GetButton ("TouchPad") + "\n";
	}
}
