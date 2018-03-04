using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaySystem : MonoBehaviour {

	private const int bufferFrames = 100;
	private MyKeyFrame[] keyFrames = new MyKeyFrame[bufferFrames];
	private Rigidbody _rigidbody;

	// Use this for initialization
	void Start () {
		_rigidbody = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		Record ();

	}

	void Record () {
		_rigidbody.isKinematic = false;
		int frame = Time.frameCount % bufferFrames;
		Debug.Log ("Writing Frame: " + frame);
		transform.position = keyFrames [frame]._position;
		transform.rotation = keyFrames [frame]._rotation;
	}

	void Playback (){
		_rigidbody.isKinematic = true;
		int frame = Time.frameCount % bufferFrames;
		float time = Time.time;
		float realTime = Time.unscaledTime;
		Debug.Log ("READING Frame: " + frame);
		keyFrames [frame] = new MyKeyFrame (time, transform.position, transform.rotation);
	}
}

/// <summary>
/// A Structure for storing a GameObject's position and rotation at a point in time.
/// </summary>
public struct MyKeyFrame {

	public float _time;
	public Vector3 _position;
	public Quaternion _rotation;

	public MyKeyFrame (float time, Vector3 pos, Vector3 rot){
		_time = time;
		_position = pos;
		_rotation = Quaternion.Euler(rot);
	}

	public MyKeyFrame (float time, Vector3 pos, Quaternion rot){
		_time = time;
		_position = pos;
		_rotation = rot;
	}
}
