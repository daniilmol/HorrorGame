using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Flicker : MonoBehaviour
{
    float timeSinceFlickered = Mathf.Infinity;
    float flickerTime;
    bool flickering;
    float flicker = 0.2f;
    float timeSinceNotFlickered = Mathf.Infinity;

    void Start() {
        flickering = true;
    }
    void initializeFlicker(){
        if(flickering && timeSinceNotFlickered >= flicker){
            flickering = false;
            System.Random r = new System.Random();
            flickerTime = (float)r.NextDouble();
            timeSinceFlickered = 0;
            GetComponent<Light>().range = 10f;
            GetComponent<Light>().intensity = 2f;
        }else{
            timeSinceFlickered += Time.deltaTime;
        }
    }
    void Update() {
        initializeFlicker();
        if(timeSinceFlickered >= flickerTime && !flickering){
            flickering = true;
            timeSinceNotFlickered = 0;
            GetComponent<Light>().range = 9f;
            GetComponent<Light>().intensity = 1.5f;
        }else{
            timeSinceNotFlickered += Time.deltaTime;
        }
    }
}