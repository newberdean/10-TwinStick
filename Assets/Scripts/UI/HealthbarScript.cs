using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarScript : MonoBehaviour {

	Slider slider;
	Image fillRect;

	public void HealthReady(){
		slider = GetComponent<Slider> ();
		fillRect = slider.transform.GetChild (1).GetChild (0).gameObject.GetComponent<Image> ();
		slider.value = slider.maxValue;
		slider.onValueChanged.AddListener (delegate {OnSliderWasChanged(); });
		OnSliderWasChanged ();
	}

	void OnDisabled(){    if (slider) slider.onValueChanged.RemoveAllListeners ();    }
	void OnDestroy() {    if (slider) slider.onValueChanged.RemoveAllListeners ();    }

	public void ChangeSlider(float h)   {    slider.value = h;     }
	public void incrementSlider(float h){    slider.value += h;    }
	public void decrementSlider(float h){    slider.value -= h;    }

	public void OnSliderWasChanged(){
		if (slider)
			fillRect.color = new Color (1f-(slider.value/slider.maxValue), slider.value/slider.maxValue, 0f);
	}
}
