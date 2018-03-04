using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaySystem : MonoBehaviour {

	private const int bufferFrames = 10000;
	public static int framesLeft;
	private List<MyKeyFrame> keyFrames = new List<MyKeyFrame>(bufferFrames);
	private Rigidbody _rigidbody;
	private int framePosition = 0;	// Where to start Playback
	private bool rewinding = false, replaying = false;

	// Use this for initialization
	void Start () {
		_rigidbody = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (GameManager.Recording)		Record ();
		else if (GameManager.Replaying)	Playback ();
		else if (GameManager.Rewinding)	Rewind ();
		framesLeft = bufferFrames - keyFrames.Count;
	}

	void Record () {
		replaying = false;
		rewinding = false;
		if (_rigidbody)
			_rigidbody.isKinematic = false;
		float time = Time.time;
		float realTime = Time.unscaledTime;
		if (keyFrames.Count < bufferFrames)
			keyFrames.Add (new MyKeyFrame (time, transform.position, transform.rotation));
		else {
			keyFrames.RemoveAt (keyFrames.Count-1);
			keyFrames.Insert (0, new MyKeyFrame (time, transform.position, transform.rotation));
		}
	}

	void Playback (){
		if (!replaying){
			framePosition = 0;	// Reset Playback position
			replaying = true;
		}
		if (_rigidbody)
			_rigidbody.isKinematic = true;
		transform.position = keyFrames [framePosition % keyFrames.Count]._position;
		transform.rotation = keyFrames [framePosition++ % keyFrames.Count]._rotation;
	}

	void Rewind (){
		if (!rewinding){
			framePosition = keyFrames.Count-1;	// Reset Playback position
			rewinding = true;
		}
		if (framePosition < 0) {
			return;//framePosition = keyFrames.Count;
		}
		if (_rigidbody)
			_rigidbody.isKinematic = true;
		transform.position = keyFrames [framePosition]._position;
		transform.rotation = keyFrames [framePosition]._rotation;
		keyFrames.RemoveAt (framePosition--);
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
