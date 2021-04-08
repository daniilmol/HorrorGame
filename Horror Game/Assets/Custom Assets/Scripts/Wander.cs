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
    private bool canSeePlayer;
    private bool searching;

    private float timeSinceLostPlayer = Mathf.Infinity;
    private float timeSinceStartedSearchingForPlayer = Mathf.Infinity;
    private Vector3 randomErrorVector = new Vector3(-5, -5, -5);

    [SerializeField] AudioClip scream;
    [SerializeField] AudioClip bells;
    [SerializeField] float detectionRadius = 5f;
    [SerializeField] float killRadius = 1f;
    [SerializeField] float suspicionTime = 5f;
    private float searchingTime = 20f;

 
    void OnEnable () {
        canSeePlayer = false;
        searching = false;
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
Vector3 searchingPosition;
    private void losePlayer(){
        if(chasing){
            previousPosition = GameObject.FindGameObjectWithTag("Player").transform;
            timeSinceLostPlayer = 0;
            timeSinceStartedSearchingForPlayer = 0;
            chasing = false;
            //agent.SetDestination(previousPosition.position);
        }else if(timeSinceLostPlayer < suspicionTime){
            timeSinceLostPlayer+=Time.deltaTime;
            agent.SetDestination(previousPosition.position);
        }else{
            timeSinceLostPlayer = Mathf.Infinity;
            playAppropriateAudio(scream, bells);
            Vector3 newPos = transform.position;
            idleWalk(newPos);
        }
    }

    public GameObject FindClosestDoor() {
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

    private void idleWalk(Vector3 searchPosition){
       timer += Time.deltaTime;
       if(searchPosition != randomErrorVector && timeSinceStartedSearchingForPlayer < suspicionTime + 20){
           print("searching...");
           searching = true;
           timeSinceStartedSearchingForPlayer += Time.deltaTime;
           if(timer >= wanderTimer + 3 && transform.position != pos){
                GameObject door = FindClosestDoor();
                door.GetComponent<Animator>().SetBool("isopen", true);
            }
            if (timer >= wanderTimer + 3) {
                Vector3 newPos = RandomNavSphere(searchPosition, 15, -1);
                agent.SetDestination(newPos);
                pos = newPos;
                timer = 0;
            }
       }else{
            print("not searching...");
            searching = false;
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
    }

    private void FireRayCasts(){
        Vector3 targetPosition = GameObject.FindGameObjectWithTag("Player").transform.position + new Vector3(0, 1.5f, 0f);
        Vector3 hostPosition = gameObject.transform.position + new Vector3(0, 1f, 0);
        Ray ray = new Ray(hostPosition, (targetPosition-hostPosition).normalized*10);
        Debug.DrawRay(hostPosition, (targetPosition-hostPosition).normalized*10);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100)){
            if(hit.transform == GameObject.FindGameObjectWithTag("Player").transform){
                canSeePlayer = true;
            }else{
                canSeePlayer = false;
            }
        }
    }

    void Update () {
        if(agent.enabled==false) {
            return;
        }
        FireRayCasts();
        if(Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, gameObject.transform.position) < killRadius && !GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().isHidden() && !GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().isDead()){
            agent.isStopped=true;
            killPlayer();
        }
        if(!GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().isHidden() && canSeePlayer){
            chasePlayer();
            return;
        }else{
            losePlayer();
        }
        if(!searching)
        idleWalk(randomErrorVector);
    }
 
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition (randDirection, out navHit, dist, layermask);
        return navHit.position;
    }
}
