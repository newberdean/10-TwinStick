using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaySystem : MonoBehaviour {

	public static readonly int bufferFrames = 100000;
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
		if (_rigidbody) {
			_rigidbody.isKinematic = false;/*
			if (keyFrames.Count > 0) {
				if (replaying)
					_rigidbody.velocity = keyFrames [framePosition]._velocity;
				else if (rewinding)
					_rigidbody.velocity = keyFrames [framePosition]._velocity * -1;
				replaying = false;
				rewinding = false;
			}*/
		}
		float time = Time.time;
		float realTime = Time.unscaledTime;
		if (keyFrames.Count < bufferFrames) {
			if (!_rigidbody) {
				keyFrames.Add (new MyKeyFrame (time, transform.position, transform.rotation));
			} else {
				keyFrames.Add (new MyKeyFrame (time, transform.position, _rigidbody.velocity, transform.rotation));
			}
		}
		else {
			keyFrames.RemoveAt (keyFrames.Count - 1);
			if (!_rigidbody) {
				keyFrames.Insert (0, new MyKeyFrame (time, transform.position, transform.rotation));
			} else {
				keyFrames.Insert (0, new MyKeyFrame (time, transform.position, _rigidbody.velocity, transform.rotation));
			}
		}
	}

	void Playback (){
		if (!replaying){
			framePosition = 0;	// Reset Playback position
			replaying = true;
		}
		if (keyFrames.Count <= 0) {
			return;//framePosition = keyFrames.Count;
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
	public Vector3 _position, _velocity;
	public Quaternion _rotation;

	public MyKeyFrame (float time, Vector3 pos, Vector3 rot){
		_time = time;
		_position = pos;
		_rotation = Quaternion.Euler(rot);

		_velocity = Vector3.zero;
	}

	public MyKeyFrame (float time, Vector3 pos, Quaternion rot){
		_time = time;
		_position = pos;
		_rotation = rot;

		_velocity = Vector3.zero;
	}

	public MyKeyFrame (float time, Vector3 pos, Vector3 vel, Vector3 rot){
		_time = time;
		_position = pos;
		_velocity = vel;
		_rotation = Quaternion.Euler(rot);
	}

	public MyKeyFrame (float time, Vector3 pos, Vector3 vel, Quaternion rot){
		_time = time;
		_position = pos;
		_velocity = vel;
		_rotation = rot;
	}
}
