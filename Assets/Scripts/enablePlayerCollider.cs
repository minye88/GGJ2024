using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enablePlayerCollider : MonoBehaviour
{
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
	{
        float distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);
        //Debug.Log(distanceFromPlayer);


        if (distanceFromPlayer <= 5.0f) { 
            player.GetComponent<CapsuleCollider>().enabled = true;
            //GameObject.Find("House").SetActive(true);
            //Debug.Log("COLLIDER ON");
        }
    }
}
