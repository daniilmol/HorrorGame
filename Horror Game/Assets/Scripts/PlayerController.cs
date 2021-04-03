using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class PlayerController : MonoBehaviour {
    Vector3 lastPosition;
    [SerializeField]AudioClip[]footsteps;
    [SerializeField]AudioClip[]flashlightClicks;
    public AudioSource[] playerSounds;
    public AudioSource footstepPlayer;    
    public AudioSource itemPlayer;
    private bool journalOpened;

    void Start(){
        playerSounds = GetComponents<AudioSource>();
        footstepPlayer = playerSounds[0];
        itemPlayer = playerSounds[1];
        journalOpened = false;
    }

    void playFootsteps(){
        int index = UnityEngine.Random.Range(0, 7);
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
            Camera mainCamera = Camera.main;
            mainCamera.GetComponent<CameraScript>().setSpeed(3);
            footstepPlayer.pitch = 1.5f;
        }if(!Input.GetKey(KeyCode.LeftShift)){
            Camera mainCamera = Camera.main;
            mainCamera.GetComponent<CameraScript>().setSpeed(2);
            footstepPlayer.pitch = 0.75f;
        }
        if(Input.GetKeyDown(KeyCode.Tab) && !journalOpened){
            GameObject x = GameObject.Find("Journal");
            x.transform.GetChild(0).gameObject.SetActive(true);
            journalOpened = true;
        }else if(Input.GetKeyDown(KeyCode.Tab) && journalOpened){
            GameObject x = GameObject.Find("Journal");
            x.transform.GetChild(0).gameObject.SetActive(false);
            journalOpened = false;
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