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
    private Vector3 pos;

    [SerializeField] AudioClip scream;
    [SerializeField] AudioClip bells;
    [SerializeField] float detectionRadius = 5f;
    [SerializeField] float killRadius = 1f;
 
    void OnEnable () {
        agent = GetComponent<NavMeshAgent> ();
        if(agent.enabled==false) {
            foreach(Transform child in gameObject.transform) {
                try{
                    child.gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
                }catch(MissingComponentException e){}
            }
            return;
        }
        timer = wanderTimer;
        GetComponent<AudioSource>().clip=bells;
        chasing = false;
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
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().die();
        GameObject.FindGameObjectWithTag("Player").transform.LookAt(transform);     
        transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform);   
        Vector3 x = Vector3.Lerp(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position, 0.5f);   
        Vector3.MoveTowards(transform.position, x, 1);
        GetComponent<AudioSource>().Stop();
        GameObject.Find("Global Audio Player").GetComponent<AudioSource>().Play();
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
        GameObject door = FindClosestDoor();
        if(Vector3.Distance(transform.position, door.transform.position) < 3f){
            door.GetComponent<Animator>().SetBool("isopen", true);
        }
    }

    private void losePlayer(){
        if(chasing){
            agent.SetDestination(previousPosition.position);
            playAppropriateAudio(scream, bells);
            chasing = false;
        }
    }

    public GameObject FindClosestDoor()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Door");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos) {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance) {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    private void idleWalk(){
       timer += Time.deltaTime;
        if(timer >= wanderTimer  && transform.position != pos){
            GameObject door = FindClosestDoor();
            door.GetComponent<Animator>().SetBool("isopen", true);
        }
        if (timer >= wanderTimer) {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            pos = newPos;
            timer = 0;
        }
    }

    void Update () {
        if(agent.enabled==false) {
            return;
        }
        if(Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, gameObject.transform.position) < killRadius && !GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().isHidden() && !GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().isDead()){
            agent.isStopped=true;
            killPlayer();
        }
        if(Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, gameObject.transform.position) < detectionRadius && !GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().isHidden()){
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
