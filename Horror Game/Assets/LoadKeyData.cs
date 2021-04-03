using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadKeyData : MonoBehaviour {
    
    private KeyCode[] defaultBindings = {KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D};
    private int[] playerBindings;
    private int bindingCount;
    private GameObject controls;
    
    private void fillPlayerBindings(){
        playerBindings = new int[bindingCount];
        playerBindings[0] = PlayerPrefs.GetInt("Forward");
        playerBindings[1] = PlayerPrefs.GetInt("Left");
        playerBindings[2] = PlayerPrefs.GetInt("Backward");
        playerBindings[3] = PlayerPrefs.GetInt("Right");
    }

    public void Apply(){

    }

    void Awake() {
        controls = GameObject.Find("Controls");
        bindingCount = controls.transform.childCount / 2;
        fillPlayerBindings();
    }

    void OnEnable() {
        int index = 0;
        foreach(Transform control in controls.transform){
            if(control.gameObject.GetComponent<Button>() != null){
                if(playerBindings[index]==0)
                    control.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text=defaultBindings[index++].ToString();
                else
                    control.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text=((KeyCode)(playerBindings[index++])).ToString();
            }
        }
    }
}
