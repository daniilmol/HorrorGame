using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
    AudioSource footstepPlayer;    
    Vector3 lastPosition;
    [SerializeField]AudioClip[]footsteps;

    void Start(){
        footstepPlayer = GetComponent<AudioSource>();
    }

    void playFootsteps(){
        int index = Random.Range(0, 7);
        if(!footstepPlayer.isPlaying){
            footstepPlayer.clip = footsteps[index];
            footstepPlayer.Play(); 
        }
    }
    void stopFootsteps(){

    }

    void Update(){
        Vector3 currentPosition = transform.position;
        if(currentPosition == lastPosition){
            //Not Moving
            stopFootsteps();
        }
        else{
            //Moving
            playFootsteps();
        }
        lastPosition = currentPosition;
    }
}