using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void Play(){
        LevelChanger lc = FindObjectOfType(typeof(LevelChanger)) as LevelChanger;
        lc.FadeToLevel(1);
    }

    public void Quit(){
        Application.Quit();
    }
}
