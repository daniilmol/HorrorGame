using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour{

    private float viewAngle = 75f;
    private float maxInteractionDistance = 2f;
    private float distance;
    private float angle;

    private GameObject player;
    private GameObject selectedObject;

    public abstract void Interact();
    
    /**
    * This method sets the current distance the player is from the interactable object. It also sets the angle of where the player looks relative to the interactable object's location.
    */
    private void setVariables(GameObject selectedObject){
        this.selectedObject = selectedObject;
        distance = Vector3.Distance(player.transform.position, selectedObject.transform.position);
        angle = Vector3.Angle(selectedObject.transform.position - player.transform.position, player.transform.forward);
    }

    /**
    * This method checks if the player is close to the interactable object and is looking at it. 
    * If so, it checks if the player clicks the E button, which calls any overriden Interact() method.
    */
    private void checkForInteraction(){
        Vector3 hostPosition = Camera.main.transform.position;
        Vector3 targetPosition = selectedObject.transform.position;
        Ray ray = new Ray(hostPosition, (targetPosition-hostPosition).normalized*10);
        Debug.DrawRay(hostPosition, (targetPosition-hostPosition).normalized*10);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, maxInteractionDistance)){
            print(hit.collider.gameObject.name + ", " + selectedObject.name);
            if(hit.collider.gameObject == selectedObject || hit.collider.gameObject.transform.parent == selectedObject.transform){
                float angle = Vector3.Angle((targetPosition - hostPosition), player.transform.forward);
                if(angle < viewAngle){
                    if(Input.GetKeyDown(KeyCode.E)){
                        Interact();
                    }
                }
            }
        }
    }

    public void checkAvailability(GameObject selectedObject, float angle){
        this.viewAngle = angle;
        setVariables(selectedObject);
        checkForInteraction();
    }

    void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
        
    }
}
