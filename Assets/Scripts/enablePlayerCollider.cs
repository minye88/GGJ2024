using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enablePlayerCollider : MonoBehaviour
{
    public GameObject player;
    public GameObject winTarget;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
	{
        float distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);
        //Debug.Log(distanceFromPlayer);


        if (distanceFromPlayer <= 10.0f)
        {
            
            player.GetComponent<CapsuleCollider>().enabled = true;
            //GameObject.Find("House").SetActive(true);
            //Debug.Log("COLLIDER ON");
            player.transform.Find("Detection Radius").GetComponent<CapsuleCollider>().enabled = true;

            winTarget.GetComponent<victoryCondition>().playerWon = false;
        }
        else if (distanceFromPlayer > 200.0f) {
            player.GetComponent<CapsuleCollider>().enabled = false;
            player.GetComponent<Rigidbody>().position = transform.position;
            player.transform.Find("Detection Radius").GetComponent<CapsuleCollider>().enabled = false;
        }
    }
}
