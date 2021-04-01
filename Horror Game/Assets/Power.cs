using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Power : Interactable {
    public override void Interact(){
        gameObject.transform.Find("Power Button").GetComponent<Animator>().SetTrigger("ClickButton");
        Light[] lights = FindObjectsOfType(typeof(Light)) as Light[];
        foreach(Light light in lights){
            light.GetComponentInParent<Flicker>().enabled=false;
            light.intensity = 0;
        }
        AudioSource[] audioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach(AudioSource ambience in audioSources){
            try{
                ambience.GetComponentInParent<AudioPlayer>().enabled = true;
            }catch(NullReferenceException nre){
            }
        }
        //RenderSettings.fog = true;
        //RenderSettings.fogDensity = 0.09f;
    }

    void Update(){
        checkAvailability(gameObject, 71);
    }
}
