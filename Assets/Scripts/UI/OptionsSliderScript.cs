using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsSliderScript : MonoBehaviour {

	Slider slider;

	public Text attachedValue;
	public void Awake(){
		slider = GetComponent<Slider> ();
		slider.onValueChanged.AddListener (delegate {OnSliderWasChanged(); });
		OnSliderWasChanged ();
	}

	void OnDisabled(){
		slider.onValueChanged.RemoveAllListeners ();
	}

	void OnDestroy(){
		slider.onValueChanged.RemoveAllListeners ();
	}

	public void ChangeSlider(float h){
		slider.value = h;
	}

	public void incrementSlider(float h){
		slider.value += h;
	}

	public void decrementSlider(float h){
		slider.value -= h;
	}

	public void OnSliderWasChanged(){
		if (slider.name.Contains ("Difficulty")) {
			switch ((int)slider.value) {
			case 1:
				attachedValue.text = "Easy";
				break;
			case 2:
				attachedValue.text = "Normal";
				break;
			case 3:
				attachedValue.text = "Hard";
				break;
			default:
				attachedValue.text = "... OMFG WHAT DID YOU <b><i>DO</i></b>!?";
				break;
			}
		} else {
			attachedValue.text = Mathf.RoundToInt (slider.value * 100) + "%";
		}
	}
}
