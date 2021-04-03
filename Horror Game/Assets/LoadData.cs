using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadData : MonoBehaviour {
    float volume;
    GameObject sliderContainer;
    Slider volumeSlider;

    public void Apply(){
        PlayerPrefs.SetFloat("MasterVolume", volumeSlider.value);
        PlayerPrefs.Save();
    }

    void Awake() {
        sliderContainer = GameObject.Find("Master Volume Slider");
        volumeSlider = sliderContainer.GetComponent<Slider>();
    }

    void OnEnable() {
        if(PlayerPrefs.HasKey("MasterVolume")){
            volume = PlayerPrefs.GetFloat("MasterVolume");
        }else{
            volume = 0.5f;
        }
        volumeSlider.value = volume;
    }
}
