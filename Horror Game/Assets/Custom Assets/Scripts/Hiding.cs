using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hiding : MonoBehaviour{
    void OnTriggerEnter(Collider other) {
        other.gameObject.GetComponent<PlayerController>().hidePlayer(true);
    }
    void OnTriggerExit(Collider other) {
        other.gameObject.GetComponent<PlayerController>().hidePlayer(false);
    }
}
