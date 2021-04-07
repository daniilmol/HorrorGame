using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wander : MonoBehaviour
{
   public float wanderRadius;
    public float wanderTimer;
 
    private Transform target;
    private NavMeshAgent agent;
    private float timer;

    private Transform previousPosition;
    private bool chasing;

    [SerializeField] AudioClip scream;
    [SerializeField] AudioClip bells;
 
    void OnEnable () {
        agent = GetComponent<NavMeshAgent> ();
        timer = wanderTimer;
        GetComponent<AudioSource>().clip=bells;
        chasing = false;
         foreach(Transform child in gameObject.transform) {
             try{
             child.gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;

             }catch(MissingComponentException e){}
         }
    }

    public static IEnumerator FadeOut (AudioSource audioSource, float FadeTime) {
        float startVolume = audioSource.volume;
 
        while (audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
             print("fading out");

            yield return new WaitForSeconds(FadeTime);
        }
 
        audioSource.Stop ();
        audioSource.volume = startVolume;
    }
    public static IEnumerator FadeIn (AudioSource audioSource, float FadeTime) {
        float startVolume = audioSource.volume;
 
        while (audioSource.volume < 0.041f) {
            audioSource.volume += startVolume * Time.deltaTime / FadeTime;
            print("fading in");
            yield return new WaitForSeconds(FadeTime);
        }
 
        audioSource.Play();
        audioSource.volume = startVolume;
    }

    private void killPlayer(){
        print("Player is dead");
    }

    private void playAppropriateAudio(AudioClip clipToStop, AudioClip clipToStart){
        if(GetComponent<AudioSource>().clip==clipToStop){
            GetComponent<AudioSource>().Stop();
            //StartCoroutine (Wander.FadeOut (GetComponent<AudioSource>(), 2f));
        }
        GetComponent<AudioSource>().clip=clipToStart;
        if(!GetComponent<AudioSource>().isPlaying){
            GetComponent<AudioSource>().Play();
            //StartCoroutine (Wander.FadeIn (GetComponent<AudioSource>(), 2f));
        }
    }

    private void chasePlayer(){
        agent.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
        playAppropriateAudio(bells, scream);
        previousPosition = gameObject.transform;
        chasing = true;     
    }

    private void losePlayer(){
        if(chasing){
            agent.SetDestination(previousPosition.position);
            playAppropriateAudio(scream, bells);
            chasing = false;
        }
    }

    private void idleWalk(){
       timer += Time.deltaTime;
 
        if (timer >= wanderTimer) {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }

    void Update () {
        if(Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, gameObject.transform.position) < 1f && !GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().isHidden()){
            killPlayer();
        }
        if(Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, gameObject.transform.position) < 5f && !GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().isHidden()){
            chasePlayer();
            return;
        }else{
            losePlayer();
        }
        idleWalk();
    }
 
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition (randDirection, out navHit, dist, layermask);
        return navHit.position;
    }
}
