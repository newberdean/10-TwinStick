using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using USACPI = UnityStandardAssets.CrossPlatformInput;

public class ExitScript : MonoBehaviour {

	GameObject cam;
	GameObject ball;

	ParticleSystem mysystem;
	ParticleSystem childsys;

	public bool exitEnabled = true, timedExit = false;

	bool triggered = false;

	// Use this for initialization
	void Start () {
		if (timedExit) {
			Invoke ("Enable", GameManager.TIME_LEFT*0.9f);
		}
		cam = GameObject.Find ("Camera Stand");
		ball = GameObject.FindGameObjectWithTag ("Player");
		mysystem = GetComponent<ParticleSystem> ();
		childsys = transform.GetChild(0).GetComponent<ParticleSystem> ();
		if (exitEnabled) {
			mysystem.Play (false);
		}
	}

	void OnTriggerStay (Collider other){
		if (other.CompareTag ("Player") && exitEnabled && !triggered) {
			triggered = true;
			ReplaySystemManager.LevelEnd = true;
			GameManager.StopCountdown ();
			cam.GetComponent<MoveCamera> ().enabled = false;
			ball.GetComponent<UnityStandardAssets.Vehicles.Ball.Ball> ().enabled = false;
			ball.GetComponent<UnityStandardAssets.Vehicles.Ball.BallUserControl> ().enabled = false;
			Rigidbody rb = ball.GetComponent<Rigidbody> ();
			rb.velocity = Vector3.zero;
			rb.rotation = Quaternion.identity;
			rb.isKinematic = true;
			mysystem.Stop ();
			foreach (GameObject g in GameObject.FindGameObjectsWithTag("Grabbable")) {
				rb = g.GetComponent<Rigidbody> ();
				rb.AddExplosionForce (200f, transform.position, 100f, 0f, ForceMode.Force);
			}
			childsys.Play ();
			Invoke ("BounceAway", 2f);
		}
	}

	void BounceAway (){
		Animator anim = ball.GetComponent<Animator> ();
		anim.enabled = true;
		float time = 1f;
		AnimatorClipInfo[] ACI = anim.GetCurrentAnimatorClipInfo (0);
		for (int i = 0; i < ACI.Length; i++) {
			if (ACI [i].clip.name.Contains("Exit")) {
				time = ACI [i].clip.length;
				break;
			}
		}
		anim.SetTrigger ("EndLevel");
		Invoke ("NextLevel", time);
	}

	void NextLevel (){
		LevelManager.LoadNextLevel ();
	}

	public void Enable(){
		GetComponent<MeshRenderer> ().enabled = true;
		exitEnabled = true;
		mysystem.Play (false);
	}

	public void Disable(){
		exitEnabled = false;
		mysystem.Stop (false);
	}
}