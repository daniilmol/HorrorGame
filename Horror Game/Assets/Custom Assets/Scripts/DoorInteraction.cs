using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : Interactable {
    bool open = false;
    public override void Interact(){
        print("TOGGLING DOOR");
        if(!GetComponent<Animator>().GetBool("isopen"))
            GetComponent<Animator>().SetBool("isopen", true);
        else
            GetComponent<Animator>().SetBool("isopen", false);
    }
    void Update(){
        checkAvailability(gameObject, 73);
    }
}
