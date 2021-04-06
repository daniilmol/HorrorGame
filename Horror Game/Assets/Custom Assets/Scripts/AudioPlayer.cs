using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour {

    AudioSource audioSource;

    float timeSincePlayed = Mathf.Infinity;
    float playTime;
    [SerializeField]float minute = 60f;

    void Start(){
        audioSource = GetComponent<AudioSource>();
    }

    void Update(){
        if(timeSincePlayed >= minute){
            minute = Random.Range(20, 61);
            audioSource.Play();
            timeSincePlayed = 0;
        }else{
            timeSincePlayed += Time.deltaTime;
        }
    }
}
