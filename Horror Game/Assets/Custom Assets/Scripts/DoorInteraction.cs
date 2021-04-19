using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : Interactable {
    bool open = false;
    public override void Interact(){
        print("TOGGLING DOOR");
        if(!GetComponent<Animator>().GetBool("isopen") && !isPlaying(GetComponent<Animator>(), "Close Door")){
            GetComponent<Animator>().SetBool("isopen", true);
            GetComponent<AudioSource>().Play();
        }
        else if(GetComponent<Animator>().GetBool("isopen") && !isPlaying(GetComponent<Animator>(), "Open Door")){
            GetComponent<Animator>().SetBool("isopen", false);
            GetComponent<AudioSource>().Play();
        }
    }
    bool isPlaying(Animator anim, string stateName){
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(stateName) && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            return true;
        else
            return false;
    }
    void Update(){
        checkAvailability(gameObject, 73);
    }
}
