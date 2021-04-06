using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.AI;
public class PlayerController : MonoBehaviour {

    Vector3 lastPosition;
    [SerializeField]AudioClip[]footsteps;
    [SerializeField]AudioClip[]flashlightClicks;
    [SerializeField]GameObject flashlight;
    public AudioSource[] playerSounds;
    public AudioSource footstepPlayer;    
    public AudioSource itemPlayer;
    private bool journalOpened;
    private bool varIsMoving;
    private bool sneaking;

    public GameObject getFlashlight(){
        return flashlight;
    }

    void Start(){
        playerSounds = GetComponents<AudioSource>();
        footstepPlayer = playerSounds[0];
        itemPlayer = playerSounds[1];
        journalOpened = false;
        sneaking = false;
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

    private void flashlightCheck(){
        if(Input.GetKeyDown(KeyCode.F)){
            if(!flashlight.GetComponentInChildren<FlashlightOffset>().isOn()){
                if(!itemPlayer.isPlaying){
                    flashlight.GetComponentInChildren<FlashlightOffset>().toggle();
                    itemPlayer.PlayOneShot(flashlightClicks[0]);
                }
            }else{
                if(!itemPlayer.isPlaying){
                    flashlight.GetComponentInChildren<FlashlightOffset>().toggle();
                    itemPlayer.PlayOneShot(flashlightClicks[1]);
                }
            }
        }
    }

    private void sprintCheck(){
        if(Input.GetKey(KeyCode.LeftShift) && varIsMoving && !sneaking){
            Camera mainCamera = Camera.main;
            mainCamera.GetComponent<CameraScript>().setSpeed(3);
            footstepPlayer.pitch = 1.5f;
        }if(!Input.GetKey(KeyCode.LeftShift) || !varIsMoving){
            Camera mainCamera = Camera.main;
            mainCamera.GetComponent<CameraScript>().setSpeed(2);
            footstepPlayer.pitch = 0.75f;
        }
    }

    private void journalCheck(){
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

    private void sneakCheck(){
        if(Input.GetKeyDown(KeyCode.LeftControl)){
            if(!sneaking){
                GetComponent<NavMeshAgent>().height=0.1f;
                Camera.main.transform.position=new Vector3(transform.position.x, transform.position.y+0.5f, transform.position.z);
                sneaking = true;
            }else if(sneaking){
                sneaking = false;
                GetComponent<NavMeshAgent>().height=1.5f;
                Camera.main.transform.position=new Vector3(transform.position.x, transform.position.y+1.632f, transform.position.z);
            }
        }
    }

    void checkForInput(){
        flashlightCheck();
        sprintCheck();
        sneakCheck();
    }
    
    void isMoving(){
        Vector3 currentPosition = transform.position;
        checkForInput();
        if(currentPosition == lastPosition){
            //Not Moving
            lastPosition = currentPosition;
            varIsMoving = false;
        }
        else{
            //Moving
            lastPosition = currentPosition;
            varIsMoving = true;
        }
    }
    
    void Update(){
        isMoving();
        if(!varIsMoving){
            stopFootsteps();
        }
        else{
            playFootsteps();
        }
    }
}