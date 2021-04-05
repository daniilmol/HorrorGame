using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerInteraction : Interactable {
    public override void Interact(){
        print("Check computer");
    }

    void Update(){
        checkAvailability(gameObject, 73);
    }
}
