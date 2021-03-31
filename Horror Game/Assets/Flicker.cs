using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Flicker : MonoBehaviour
{
    float timeSinceFlickered = Mathf.Infinity;
    float flickerTime;
    bool flickering;

    void Start() {
        flickering = true;    
    }
    void initializeFlicker(){
        if(flickering){
            flickering = false;
            System.Random r = new System.Random();
            flickerTime = (float)r.NextDouble();
            timeSinceFlickered = 0;
            GetComponent<Light>().range = 26.47f;
            GetComponent<Light>().intensity = 1.5f;
        }else{
            timeSinceFlickered += Time.deltaTime;
        }
    }
    void Update() {
        initializeFlicker();
        if(timeSinceFlickered >= flickerTime){
            flickering = true;
            GetComponent<Light>().range = 10f;
            GetComponent<Light>().intensity = 0.8f;
        }
    }
}
