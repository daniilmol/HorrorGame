using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    Transform player;
    Animator door;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        door = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        if(Vector3.Distance(player.position, gameObject.transform.position) <= 4 && Vector3.Angle(transform.position - player.position, player.forward) < 30){
            if(Input.GetKeyDown(KeyCode.E)){
                print("HELLO");
                door.SetTrigger("OpenClose");
            }
        }
    }
}
