using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveBuilding : Interactable {
    public override void Interact() {
        LevelChanger lc = FindObjectOfType(typeof(LevelChanger)) as LevelChanger;
        lc.FadeToLevel(1);
    }

    void Update(){
        checkAvailability(gameObject, 71);
    }
}
