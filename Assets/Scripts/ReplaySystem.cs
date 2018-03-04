using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaySystem : MonoBehaviour {

	public static readonly int bufferFrames = 100000;
	public static int framesLeft;
	private List<MyKeyFrame> keyFrames = new List<MyKeyFrame>(bufferFrames);
	private Rigidbody _rigidbody;
	private Vector3 rewoundVelocity = Vector3.zero; // When removing a frame during Rewind(), store the velocity.
	private int framePosition = 0;	// Where to start Playback
	private bool rewinding = false, replaying = false;

	// Use this for initialization
	void Start () {
		_rigidbody = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (GameManager.Recording)		Record ();
		else if (GameManager.Replaying)	rewoundVelocity = Playback ();
		else if (GameManager.Rewinding)	rewoundVelocity = Rewind ();
		framesLeft = bufferFrames - keyFrames.Count;
	}

	void Record () {
		if (_rigidbody) {
			_rigidbody.isKinematic = false;
			if ((rewinding || replaying) && keyFrames.Count > 0) {
				_rigidbody.velocity = rewoundVelocity;
				rewoundVelocity = Vector3.zero;
			}
		}
		replaying = false;
		rewinding = false;
		float time = Time.time;
		float realTime = Time.unscaledTime;
		if (keyFrames.Count < bufferFrames)
			keyFrames.Add (new MyKeyFrame (time, transform.position, (_rigidbody ? _rigidbody.velocity : Vector3.zero), transform.rotation));
		else {
			keyFrames.RemoveAt (keyFrames.Count-1);
			keyFrames.Insert (0, new MyKeyFrame (time, transform.position, (_rigidbody ? _rigidbody.velocity : Vector3.zero), transform.rotation));
		}
	}

	Vector3 Playback (){
		if (!replaying){
			framePosition = 0;	// Reset Playback position
			replaying = true;
		}
		if (keyFrames.Count <= 0) {
			return Vector3.zero;
		}
		if (_rigidbody)
			_rigidbody.isKinematic = true;
		transform.position = keyFrames [framePosition % keyFrames.Count]._position;
		transform.rotation = keyFrames [framePosition % keyFrames.Count]._rotation;
		Vector3 vel = keyFrames [framePosition++ % keyFrames.Count]._velocity;
		return vel;
	}

	Vector3 Rewind (){
		if (!rewinding){
			framePosition = keyFrames.Count-1;	// Reset Playback position
			rewinding = true;
		}
		if (framePosition < 0) {
			return Vector3.zero;
		}
		if (_rigidbody)
			_rigidbody.isKinematic = true;
		transform.position = keyFrames [framePosition]._position;
		transform.rotation = keyFrames [framePosition]._rotation;
		Vector3 vel = keyFrames [framePosition]._velocity * -1;
		keyFrames.RemoveAt (framePosition--);
		return vel;
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
