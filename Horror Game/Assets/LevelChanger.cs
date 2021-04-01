using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    public Animator animator;
    private int levelToLoad;
    private bool loadingNextScene;
    void Start(){
        GetComponentInChildren<Canvas>().enabled = true;
        loadingNextScene = false;
    }
    void Update(){
        
    }
    public void FadeToLevel(int levelIndex){
        if(!loadingNextScene){
            loadingNextScene = true;
            animator.SetTrigger("FadeOut");
            levelToLoad = levelIndex;
        }
    }
    public void OnFadeComplete(){
        loadingNextScene = false;
        SceneManager.LoadScene(levelToLoad);
    }
}
