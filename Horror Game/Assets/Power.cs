using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : Interactable {
    public override void Interact(){
        gameObject.transform.Find("Power Button").GetComponent<Animator>().SetTrigger("ClickButton");
        Light[] lights = FindObjectsOfType(typeof(Light)) as Light[];
        foreach(Light light in lights){
            light.GetComponentInParent<Flicker>().enabled=false;
            light.intensity = 0;
        }
    }

    void Update(){
        checkAvailability(gameObject);
    }
}
