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
 
    // Use this for initialization
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
 
            yield return null;
        }
 
        audioSource.Stop ();
        audioSource.volume = startVolume;
    }
    public static IEnumerator FadeIn (AudioSource audioSource, float FadeTime, AudioClip clip) {
        float startVolume = audioSource.volume;
 
        while (audioSource.volume > 0) {
            audioSource.volume += startVolume * Time.deltaTime / FadeTime;
 
            yield return null;
        }
 
        audioSource.Play();
        audioSource.volume = startVolume;
    }
    // Update is called once per frame
    void Update () {
        if(Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, gameObject.transform.position) < 1f){

        }
        if(Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, gameObject.transform.position) < 5f && !GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().isHidden()){
            agent.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
            if(GetComponent<AudioSource>().clip==bells){
                print("stopped");
                GetComponent<AudioSource>().Stop();
            }
            previousPosition = gameObject.transform;
            chasing = true;
            //StartCoroutine (Wander.FadeOut(GetComponent<AudioSource>(), 0.5f));
            GetComponent<AudioSource>().clip=scream;
            if(!GetComponent<AudioSource>().isPlaying)
                GetComponent<AudioSource>().Play();
            return;
        }else{
            if(chasing){
                //StartCoroutine (Wander.FadeOut(GetComponent<AudioSource>(), 0.5f));
                agent.SetDestination(previousPosition.position);
                if(GetComponent<AudioSource>().clip==scream){
                    GetComponent<AudioSource>().Stop();
                }
                print("lost target");
                GetComponent<AudioSource>().clip=bells;
                if(!GetComponent<AudioSource>().isPlaying)
                    GetComponent<AudioSource>().Play();
                chasing = false;
            }
        }
        timer += Time.deltaTime;
 
        if (timer >= wanderTimer) {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }
 
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) {
        Vector3 randDirection = Random.insideUnitSphere * dist;
 
        randDirection += origin;
 
        NavMeshHit navHit;
 
        NavMesh.SamplePosition (randDirection, out navHit, dist, layermask);
 
        return navHit.position;
    }
}
