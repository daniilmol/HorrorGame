using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightOffset : MonoBehaviour {
    private Vector3 vectOffset;
    private GameObject goFollow;
    private bool on;
    [SerializeField] private float speed = 3.0f;
    public bool isOn(){
        return on;
    }
    public void toggle(){
        if(on)
            on = false;
        else
            on = true;
    }
    void Start() {
        on = false;
        goFollow = Camera.main.gameObject;
        vectOffset = transform.position - goFollow.transform.position;
    }

    void Update() {
        if(on){
            GetComponent<Light>().intensity=2.5f;
        }else{
            GetComponent<Light>().intensity=0f;
        }
        transform.position = goFollow.transform.position + vectOffset;
        transform.rotation = Quaternion.Slerp(transform.rotation, goFollow.transform.rotation, speed * Time.deltaTime);
    }
}
