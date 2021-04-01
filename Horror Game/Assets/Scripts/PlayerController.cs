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
        //footstepPlayer.Stop();
    }

    void checkForInput(){
        if(Input.GetKeyDown(KeyCode.F)){
            if(!GetComponentInChildren<Light>().enabled)
                GetComponentInChildren<Light>().enabled = true;
            else
                GetComponentInChildren<Light>().enabled = false;
        }
        if(Input.GetKey(KeyCode.LeftShift)){
            Camera mainCamera = GameObject.FindObjectOfType<Camera>();
            mainCamera.GetComponent<CameraScript>().setSpeed(3);
            footstepPlayer.pitch = 1;
        }if(!Input.GetKey(KeyCode.LeftShift)){
            Camera mainCamera = GameObject.FindObjectOfType<Camera>();
            mainCamera.GetComponent<CameraScript>().setSpeed(2);
            footstepPlayer.pitch = 0.75f;
        }
    }

    void Update(){
        Vector3 currentPosition = transform.position;
        checkForInput();
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