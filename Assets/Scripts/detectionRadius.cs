using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectionRadius : MonoBehaviour
{
	public GameObject gameManager;
	public GameObject fartParticleParent;

	// Start is called before the first frame update
	void Start()
    {

	}

    private void updateRadius(int scaler)
    {
        Vector3 newScale = this.transform.localScale;
        newScale.x = scaler;
        newScale.z = scaler;
        this.transform.localScale = newScale;
    }

    // Update is called once per frame
    void Update()
    {
        int particleCount = fartParticleParent.transform.childCount;
        //Debug.Log("number of particles: " + particleCount);
        updateRadius(particleCount);
		//Debug.Log(this.transform.localScale);
    }

    private void OnTriggerEnter(Collider other)
    {
		//// Use raycast to check if there is a wall in between player and NPC
		// Bit shift the index of the layer (7) to get a bit mask.
		// This would cast rays only against colliders in layer 7.
		//int layerMask = 1 << 7;
		//int layerMask = LayerMask.GetMask("Wall");

		// Calculate raycast direction
		Vector3 raycastDirection = other.transform.position - transform.parent.position;
		//Vector3 raycastDirection = other.transform.position - transform.position;

		//float maxDistance = raycastDirection.magnitude * 2;
		float maxDistance = Mathf.Infinity;

		//Debug.Log(raycastDirection);

		RaycastHit hit;
		//// Does the ray intersect any objects excluding the player layer
		//if (Physics.Raycast(transform.parent.position, raycastDirection, out hit, maxDistance, layerMask))
		if (Physics.Raycast(transform.parent.position, raycastDirection, out hit, maxDistance, (1 << LayerMask.NameToLayer("Wall") | 1 << LayerMask.NameToLayer("NPC"))))
		{
			//Debug.DrawRay(transform.position, raycastDirection * hit.distance, Color.yellow);
			//Debug.Log("Did Hit: " + hit.transform.name);

			//if (hit.transform.gameObject.layer == LayerMask.NameToLayer("NPC"))
			if (hit.collider == other)
			{
				Debug.Log("detected by " + hit.collider.name);

				// play cutscene and reset player position
				gameManager.GetComponent<gameManager>().playCaughtCutscene(other.transform.parent.gameObject);
			}
			else 
			{ 
				Debug.Log("undetected, blocked by " + hit.collider.name);
			}
		}
		else
		{
			//Debug.DrawRay(transform.position, raycastDirection * 1000, Color.white);
			//Debug.Log("Did not Hit");
		}
	}
}
