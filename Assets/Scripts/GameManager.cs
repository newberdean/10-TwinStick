using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using USACPI = UnityStandardAssets.CrossPlatformInput;

public class GameManager : MonoBehaviour {

	/// <summary>
	/// An integer expressing whether the game is active.
	/// 0 = false (game is paused), 1 = true (gameActive).
	/// To use this variable, change the number from 0 to 1
	/// and back to toggle state, then multiply timeScales by the
	/// variable.
	/// 
	/// If the game is active (gameActive == 1), then gameplay
	/// proceeds normally, otherwise it's paused.
	/// </summary>
	private static int gameActive = 1, CountdownActive = 1;
	private static float fixedTimeStep = 0.02f, timeLeft = 60f;
	private static bool canManipulateTime = true, levelDone = false;
	private static string[] hints = {
		"Hint:\nThere are actually 2 exits in this \"tutorial\". Figuring out how to " +	// 0 Tutorial
			"get to the harder one could help you later.\nAlso bear in mind you have " +
			"no Time Limit. Feel free to experiment, or play around!",
		"Hint:\nREMEMBER: If you stop Replaying or REWINDING, whatever velocity you " +		// 1 Uphill Climb
			"had will be preserved.",
		"Hint:\nStuck? Made a mistake? If you need to \"reset\" the level, just begin " +	// 2 Maze
			"a replay and stop when you feel like it.\n<color=#ff0000ff>BEWARE! This " +
			"does NOT reset the time limit!</color>",
		"Hint:\nThe ONLY way to lose is to run out of time.\nIn other words, don't " +		// 3 Downhill Rush
			"worry too much if you fall off the edge of the world; just rewind or " +
			"replay and try again!",
		"Hint:\nThe ball moves by applying torque (spinning) - but all that energy " +
			"goes to waste in the air. When the ball LANDS though...",
		"Hint:\nRemember this place? That <i>tutorial</i> WAS good for something!",			// 5 Something Old
		"Hint:\nYou'll need to Activate the Exit this time.\nIn most simple " +			// 6 Something New
			"games (LIKE THIS!), it's usually done by pressing a Button. But how to get " +
			"there and back again?...",
		"Hint:\n... And now you have strange floor things you can touch. You only " +		// 7 I Can't Rhyme
			"lose if you run out of time, so what's the worst that could happen? " +
			"Also, note that buttons can be connected!",
		"Hint:\nDifferent floor things with different functions! You're welcome/I'm " +		// 8 F*** You Too
			"sorry. All floor devices are one way only.",
		"Hint:\n...Oh, sorry. The Exit appears to be late. <i>If you stay still, it " +		// 9 Late Finish
			"should be there momentarily</i>.",
		"Hint:\nThis is the Final Level. Use everything you've learned to finish!"			// 10 The Finale
	};
	private static GameManager gm;

	public static float FIXED_TIME_STEP		{
		get	{	return fixedTimeStep;	}
	}

	public static float TIME_LEFT			{
		get	{	return timeLeft;		}
	}

	public static bool CAN_MANIPULATE_TIME	{
		get	{	return canManipulateTime;}
	}
	public static bool LEVEL_DONE			{
		get	{	return levelDone;		}
	}
	public static string[] HINTS			{
		get	{	return hints;		}
	}
	public static GameManager GAMEMANGER	{
		get	{	return gm;				}
	}

	public static GameObject PAUSEMENU;

	// Use this for initialization
	void Start () {
		if (gm != null) {
			Destroy (gameObject);
		} else {
			gm = this;
			DontDestroyOnLoad (gameObject);
			fixedTimeStep = Time.fixedDeltaTime;
			PlayerPrefsManager.CheckSettings ();
		}
	}

	void Update (){
		timeLeft -= (gameActive * CountdownActive * Time.deltaTime);
		if (timeLeft <= 0) {
			LevelManager.LOSE ();
		}
	}
	
	public static void TogglePause(){
		gameActive = (gameActive + 1) % 2;
		CountdownActive = gameActive;
		Time.timeScale = 1 * gameActive;
		Time.fixedDeltaTime = fixedTimeStep * gameActive;
		canManipulateTime = (gameActive == 1);
		if (PAUSEMENU)
			PAUSEMENU.SetActive (gameActive == 1);
	}

	public static void Pause(){
		gameActive = CountdownActive = 0;
		Time.timeScale = Time.fixedDeltaTime = 0f;
		canManipulateTime = false;
		if (PAUSEMENU)
			PAUSEMENU.SetActive (true);
	}

	public static void Unpause(){
		gameActive = CountdownActive = 1;
		Time.timeScale = Time.fixedDeltaTime = 1f;
		canManipulateTime = true;
		if (PAUSEMENU)
			PAUSEMENU.SetActive (false);
	}

	public static void ResetCountdown (float seconds){
		levelDone = false;
		timeLeft = seconds;
		CountdownActive = 1;
		canManipulateTime = (gameActive == 1);
		if (PAUSEMENU)
			PAUSEMENU.SetActive (false);
	}

	public static void StopCountdown (){
		CountdownActive = 0;
		levelDone = true;
		canManipulateTime = false;
		if (PAUSEMENU)
			PAUSEMENU.SetActive (false);
	}
}
