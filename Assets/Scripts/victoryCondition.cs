using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class victoryCondition : MonoBehaviour
{
    public GameObject player;
    private bool playerWon;

    // Start is called before the first frame update
    void Start()
    {
        playerWon = false;
    }

    private void winEvent() { 
        // show dialogue 

        // reset player position

        // reset detection radius scale modifier
    }

    // Update is called once per frame
    void Update()
    {
        if (playerWon) return;

        float distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceFromPlayer <= 2.0f)
        {
            //Debug.Log("WIN");
            winEvent();
            playerWon = true;

            GameObject.Find("Detection Radius").GetComponent<detectionRadius>().setRadiusModifier(0.0f);
        }
    }
}
