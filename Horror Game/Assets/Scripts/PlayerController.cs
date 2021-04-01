using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
    Vector3 lastPosition;
    [SerializeField]AudioClip[]footsteps;
    [SerializeField]AudioClip[]flashlightClicks;
    public AudioSource[] playerSounds;
    public AudioSource footstepPlayer;    
    public AudioSource itemPlayer;


    void Start(){
        playerSounds = GetComponents<AudioSource>();
        footstepPlayer = playerSounds[0];
        itemPlayer = playerSounds[1];
    }

    void playFootsteps(){
        int index = Random.Range(0, 7);
        if(!footstepPlayer.isPlaying){
            //footstepPlayer.clip = footsteps[index];
            footstepPlayer.PlayOneShot(footsteps[index]); 
        }
    }
    void stopFootsteps(){
        //footstepPlayer.Stop();
    }

    void checkForInput(){
        if(Input.GetKeyDown(KeyCode.F)){
            if(!GetComponentInChildren<Light>().enabled){
                if(!itemPlayer.isPlaying){
                    GetComponentInChildren<Light>().enabled = true;
                    itemPlayer.PlayOneShot(flashlightClicks[0]);
                }
            }else{
                if(!itemPlayer.isPlaying){
                    GetComponentInChildren<Light>().enabled = false;
                    itemPlayer.PlayOneShot(flashlightClicks[1]);
                }
            }
        }
        if(Input.GetKey(KeyCode.LeftShift)){
            Camera mainCamera = GameObject.FindObjectOfType<Camera>();
            mainCamera.GetComponent<CameraScript>().setSpeed(3);
            footstepPlayer.pitch = 1.5f;
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