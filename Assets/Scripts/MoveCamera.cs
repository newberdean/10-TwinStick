using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using USACPI = UnityStandardAssets.CrossPlatformInput;

public class MoveCamera : MonoBehaviour {

	public static float fov = 60f;

	public Transform ballTransform;
	public float speedH = 2.0f, speedV = 2.0f;
	public bool allowMouseMovement = false;

	//private float yaw = 0.0f;
	private float pitch = 0.0f;
	private Camera cam;

	void Start (){
		cam = transform.GetChild(0).GetComponent<Camera>();
		cam.fieldOfView = fov;
	}

	void LateUpdate () {
		//if (GameManager.Recording) {
		transform.position = ballTransform.position;
		if (USACPI.CrossPlatformInputManager.GetButton ("Left Trigger 1")) {
			fov = Mathf.Clamp (cam.fieldOfView + 1, 30f, 90f);
			// Time.timeScale = (Mathf.Max (0f, Time.timeScale - 0.1f));
		}

		if (USACPI.CrossPlatformInputManager.GetButton ("Right Trigger 1")) {
			fov = Mathf.Clamp (cam.fieldOfView - 1, 30f, 90f);
			// Time.timeScale = (Mathf.Max (0f, Time.timeScale + 0.1f));
		}
		fov = Mathf.Clamp (cam.fieldOfView - (USACPI.CrossPlatformInputManager.GetAxis ("Mouse ScrollWheel") * 15f), 30f, 90f);

		cam.fieldOfView = fov;

		if (allowMouseMovement){
			// yaw = Mathf.Clamp(yaw + (speedH * Input.GetAxis("Mouse X")), -50f, 50f);
			// yaw = Mathf.Clamp(yaw + (speedH * 10 * Input.GetAxis("Right Stick Horizontal")), -50f, 50f);
			pitch = Mathf.Clamp (pitch - (speedV * USACPI.CrossPlatformInputManager.GetAxis ("Mouse Y")), -30f, 30f);
			pitch = Mathf.Clamp (pitch - (speedV * 10 * USACPI.CrossPlatformInputManager.GetAxis ("Right Stick Vertical")), -50f, 50f);
			transform.eulerAngles = new Vector3 (0f, /*yaw*/0f, pitch);
		}
		//}
	}
}
