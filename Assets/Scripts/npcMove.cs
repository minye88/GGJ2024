using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class npcMove : MonoBehaviour
{
    protected Animator animator;
    public GameObject npcModel;
    public GameObject waypointParent;
    public float walkSpeed;
    public bool pauseAtWaypoint;
    private float pauseDuration;
    public bool debugMode;
    public bool printDebug;
    private List<GameObject> waypoints;
    private GameObject destination;
    private bool walk;
    private float timer;
    private bool forceStop;

    // Start is called before the first frame update
    void Start()
    {
        waypoints = new List<GameObject>();
        for (int i = 0; i < waypointParent.transform.childCount; ++i) {
            waypoints.Add(waypointParent.transform.GetChild(i).gameObject);
            
            if (!debugMode)
                waypointParent.transform.GetChild(i).GetComponent<Renderer>().enabled = false;
        }

        destination = waypoints[0];

        walk = true;
        timer = 0.0f;
        forceStop = false;

        animator = npcModel.GetComponent<Animator>();
    }

    private GameObject getNextDestination() {
        

        int index = waypoints.IndexOf(destination);
        
        if (printDebug)
            Debug.Log("Reached index " + index);


        if (index < waypoints.Count - 1) {
            ++index;
        } else {
            index = 0;
        }

        if (printDebug)
            Debug.Log("Next index " + index);

        return waypoints[index];
    }

    private bool reachedDestination() {
        float distFromDest = Vector3.Distance(npcModel.transform.position, destination.transform.position);

        if (distFromDest < 0.1)
            return true;

        return false;
    }

    private void walkAround() {
        if (!destination) return;

        Vector3 headingDirection = (destination.transform.position - npcModel.transform.position).normalized;

        // CHANGE DIRECTION METHOD 1
        // Quaternion headingChange = Quaternion.FromToRotation(transform.forward, headingDirection);
        // transform.localRotation *= headingChange;

        // CHANGE DIRECTION METHOD 2
        npcModel.transform.rotation = Quaternion.FromToRotation(Vector3.forward, headingDirection);

        float step = Time.deltaTime * walkSpeed;
        npcModel.transform.position = Vector3.MoveTowards(npcModel.transform.position, destination.transform.position, step);

        if (reachedDestination()) {

            if (debugMode)
                destination.GetComponent<Renderer>().material.color = Color.white;

            destination = getNextDestination();

            if (debugMode)
                destination.GetComponent<Renderer>().material.color = Color.green;

            walk = false;
            
            // change animation
            animator.enabled = false;
        }
    }

    private void rest() {
        timer += Time.deltaTime;

        if (timer >= pauseDuration) {
            timer = 0.0f;
            walk = true;
            animator.enabled = true;
        }
    }

    public void stopWalking() {
        forceStop = true;
    }

    public void startWalking()
    {
        forceStop = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (forceStop) return;

        if (walk)
            walkAround();
        else
            rest();
    }
}
