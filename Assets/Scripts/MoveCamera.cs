using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using USACPI = UnityStandardAssets.CrossPlatformInput;

public class MoveCamera : MonoBehaviour {

	public Transform ballTransform;

	public float speedH = 2.0f;
	public float speedV = 2.0f;

	//private float yaw = 0.0f;
	private float pitch = 0.0f;

	// Use this for initialization
//	void Start () {	}
	
	// Update is called once per frame
	void LateUpdate () {
		//if (GameManager.Recording) {
			transform.position = ballTransform.position;
			// yaw = Mathf.Clamp(yaw + (speedH * Input.GetAxis("Mouse X")), -50f, 50f);
			// yaw = Mathf.Clamp(yaw + (speedH * 10 * Input.GetAxis("Right Stick Horizontal")), -50f, 50f);
			pitch = Mathf.Clamp (pitch - (speedV * Input.GetAxis ("Mouse Y")), -30f, 30f);
			pitch = Mathf.Clamp (pitch - (speedV * 10 * Input.GetAxis ("Right Stick Vertical")), -50f, 50f);

			transform.eulerAngles = new Vector3 (0f, /*yaw*/0f, pitch);
		//}
	}
}
