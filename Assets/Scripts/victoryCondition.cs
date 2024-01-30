using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class victoryCondition : MonoBehaviour
{
    public GameObject player;
    public bool playerWon;

    private Vector3 camStartPos;
    private Vector3 camEndPos;
    private bool moveCamera;
    private float cameraMoveSpeed;
    private Camera mainCamera;
    private float journeyLength;
    private float startTime;
    private Vector3 cameraCutsceneOffset;
    private Vector3 lookTarget;

    private float dialoguePauseDuration;
    private float dialoguePauseTimer;
    public GameObject speechBubble;
    public GameObject speechText;
    private bool dialogueOnCooldown;
    private float dialogueCooldownTimer;
    public GameObject respawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        playerWon = false;

        cameraMoveSpeed = 10f;
        mainCamera = Camera.main;
        cameraCutsceneOffset = new Vector3(0, 3f, 0);

        dialoguePauseDuration = 5f;
        dialoguePauseTimer = 0.0f;

        dialogueOnCooldown = false;
        dialogueCooldownTimer = 0.0f;
    }

    public void playVictoryCutscene()
    {
        // stop camera from following player
        mainCamera.GetComponent<vThirdPersonCamera>().enabled = false;

        // look at fart smeller
        camStartPos = mainCamera.transform.position;
        camEndPos = player.transform.position + cameraCutsceneOffset + (player.transform.forward * 3f);
        lookTarget = player.transform.position;

        journeyLength = Vector3.Distance(camStartPos, camEndPos);
        startTime = Time.time;

        // start camera movement
        moveCamera = true;
    }

    private void showSpeechBubble()
    {
        if (dialogueOnCooldown)
        {
            dialogueCooldownTimer += Time.deltaTime;

            if (dialogueCooldownTimer > dialoguePauseDuration)
            {
                dialogueOnCooldown = false;
                dialogueCooldownTimer = 0.0f;
            }

            return;
        }

        speechText.GetComponent<TextMeshProUGUI>().text = "No one noticed my air biscuits! Yay!";

        //speechBubble.GetComponent<Image>().enabled = true;
        speechBubble.SetActive(true);

        //speechText.GetComponent<TextMeshPro>().enabled = true;
        speechText.SetActive(true);

        dialogueOnCooldown = true;
    }

    private void hideSpeechBubble()
    {
        speechBubble.SetActive(false);
        speechText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (moveCamera) {
            float debugDist = Vector3.Distance(mainCamera.transform.position, camEndPos);

            // Distance moved equals elapsed time times speed..
            float distCovered = (Time.time - startTime) * cameraMoveSpeed;

            // Fraction of journey completed equals current distance divided by total distance.
            float fractionOfJourney = distCovered / journeyLength;

            mainCamera.transform.position = Vector3.Lerp(camStartPos, camEndPos, fractionOfJourney);

            mainCamera.transform.LookAt(lookTarget);

            if (Vector3.Distance(mainCamera.transform.position, camEndPos) <= 0.1f)
            {
				//Debug.Log("camera reached destination.");

				// show dialogue box
				showSpeechBubble();

				dialoguePauseTimer += Time.deltaTime;

				if (dialoguePauseTimer >= dialoguePauseDuration)
				{
                    Debug.Log("Dialogue pause finished.");
					dialoguePauseTimer = 0.0f;

					hideSpeechBubble();

					// teleport player back to lobby
					player.GetComponent<CapsuleCollider>().enabled = false;
					player.GetComponent<Rigidbody>().position = respawnPoint.transform.position;
                    player.transform.Find("Detection Radius").GetComponent<CapsuleCollider>().enabled = false;
                    Debug.Log("VICTORY TELEPORTED");

                    GameObject.Find("Detection Radius").GetComponent<detectionRadius>().resumeCollision();

					mainCamera.GetComponent<vThirdPersonCamera>().enabled = true;

					moveCamera = false;
				}
			}
        }
        
        
        if (playerWon) return;

        float distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceFromPlayer <= 2.0f)
        {
            //Debug.Log("WIN");
            playVictoryCutscene();
            playerWon = true;
            GameObject.Find("Detection Radius").GetComponent<detectionRadius>().setRadiusModifier(0.0f);
        }
    }
}
