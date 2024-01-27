using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class npcMove : MonoBehaviour
{
    protected Animator animator;
    public GameObject waypointParent;
    public float walkSpeed;
    public bool pauseAtWaypoint;
    public float pauseDuration;
    private List<GameObject> waypoints;
    private GameObject destination;
    private bool walk;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        waypoints = new List<GameObject>();
        for (int i = 0; i < waypointParent.transform.childCount; ++i) {
            waypoints.Add(waypointParent.transform.GetChild(i).gameObject);
        }

        destination = waypoints[0];

        walk = true;
        timer = 0.0f;

        animator = GetComponent<Animator>();
    }

    private GameObject getNextDestination() {
        int index = waypoints.IndexOf(destination);
    
        if (index < waypoints.Count - 1) {
            ++index;
        } else {
            index = 0;
        }

        return waypoints[index];
    }

    private void walkAround() {
        if (!destination) return;

        Vector3 headingDirection = (destination.transform.position - transform.position).normalized;
        
        // CHANGE DIRECTION METHOD 1
        // Quaternion headingChange = Quaternion.FromToRotation(transform.forward, headingDirection);
        // transform.localRotation *= headingChange;

        // CHANGE DIRECTION METHOD 2
        transform.rotation = Quaternion.FromToRotation(Vector3.forward, headingDirection);

        float step = Time.deltaTime * walkSpeed;
        transform.position = Vector3.MoveTowards(transform.position, destination.transform.position, step);

        if (transform.position == destination.transform.position) {
            destination = getNextDestination();
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

    // Update is called once per frame
    void Update()
    {
        if (walk)
            walkAround();
        else
            rest();
    }
}
