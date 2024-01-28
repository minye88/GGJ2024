using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
	private Vector3 camStartPos;
	private Vector3 camEndPos;
	private bool moveCamera;
	private float cameraMoveSpeed;
	private Camera mainCamera;
	private float journeyLength;
	private float startTime;
	private Vector3 cameraCutsceneOffset;
	private Vector3 lookTarget;
	private GameObject fartSmellerObject;
	private float dialoguePauseDuration;
	private float dialoguePauseTimer;
	public GameObject player;
	public GameObject respawnPoint;
	public GameObject speechBubble;
	public GameObject speechText;
	public GameObject detectionRadius;
	private List<string> dialogueLines;
	private bool dialogueOnCooldown;
	private float dialogueCooldownTimer;

	// Start is called before the first frame update
	void Start()
    {
		cameraMoveSpeed = 10f;
		mainCamera = Camera.main;
		cameraCutsceneOffset = new Vector3(0, 1.75f, 0);
		dialoguePauseDuration = 5f;
		dialoguePauseTimer = 0.0f;

		dialogueLines = new List<string>();
		dialogueLines.Add("OH GOD MY NOSE");
		dialogueLines.Add("OMG GET OUT");
		dialogueLines.Add("STOP FARTING");
		dialogueLines.Add("THIS GUY JUST DROPPED A BOMB");

		dialogueOnCooldown = false;
		dialogueCooldownTimer = 0.0f;
	}

    // Update is called once per frame
    void Update()
    {
		if (moveCamera)
		{
			float debugDist = Vector3.Distance(mainCamera.transform.position, camEndPos);
			//Debug.Log(debugDist);


			// Distance moved equals elapsed time times speed..
			float distCovered = (Time.time - startTime) * cameraMoveSpeed;

			// Fraction of journey completed equals current distance divided by total distance.
			float fractionOfJourney = distCovered / journeyLength;

			mainCamera.transform.position = Vector3.Lerp(camStartPos, camEndPos, fractionOfJourney);

			//float step = Time.deltaTime * cameraMoveSpeed;
			//mainCamera.transform.position = Vector3.MoveTowards(camStartPos, camEndPos, step);

			mainCamera.transform.LookAt(lookTarget);

			if (Vector3.Distance(mainCamera.transform.position, camEndPos) <= 0.1f) {
				//Debug.Log("camera reached destination.");


				// show dialogue box (with randomized lines)
				showSpeechBubble();

				dialoguePauseTimer += Time.deltaTime;

				if (dialoguePauseTimer >= dialoguePauseDuration) {
					Debug.Log("Dialogue pause finished.");
					dialoguePauseTimer = 0.0f;

					hideSpeechBubble();

					// teleport player back to lobby
					//GameObject.Find("House").SetActive(false);
					player.GetComponent<CapsuleCollider>().enabled = false;
					//player.GetComponent<Rigidbody>().MovePosition(respawnPoint.transform.position);
					player.GetComponent<Rigidbody>().position = respawnPoint.transform.position;

					detectionRadius.GetComponent<detectionRadius>().resumeCollision();

					mainCamera.GetComponent<vThirdPersonCamera>().enabled = true;

					if (fartSmellerObject.GetComponent<npcMove>())
						fartSmellerObject.GetComponent<npcMove>().startWalking();

					moveCamera = false;
				}	
			}
		}
	}

	private void showSpeechBubble() {

		if (dialogueOnCooldown) {
			dialogueCooldownTimer += Time.deltaTime;

			if (dialogueCooldownTimer > dialoguePauseDuration) { 
				dialogueOnCooldown = false;
				dialogueCooldownTimer = 0.0f;
			}

			return;
		}

		//randomize text here
		int randomIndex = Random.Range(0, dialogueLines.Count);
		speechText.GetComponent<TextMeshProUGUI>().text = dialogueLines[randomIndex];

		//speechBubble.GetComponent<Image>().enabled = true;
		speechBubble.SetActive(true);

		//speechText.GetComponent<TextMeshPro>().enabled = true;
		speechText.SetActive(true);

		dialogueOnCooldown = true;
	}

	private void hideSpeechBubble() {
		speechBubble.SetActive(false);
		speechText.SetActive(false);
	}

	public void playCaughtCutscene(GameObject fartSmeller)
	{
		if (!fartSmeller) return;
		
		Debug.Log("fartSmeller name is " + fartSmeller.name);

		fartSmellerObject = fartSmeller;

		// stop camera from following player
		mainCamera.GetComponent<vThirdPersonCamera>().enabled = false;

		// make fart smeller stop walking (if it's a walking NPC)
		if (fartSmeller.GetComponent<npcMove>())
			fartSmeller.GetComponent<npcMove>().stopWalking();

		// look at fart smeller
		camStartPos = mainCamera.transform.position;

		//camEndPos = fartSmeller.transform.position;// + fartSmeller.transform.forward;
		//camEndPos = getGlobalPosition(fartSmeller);
		GameObject fartSmellerModel = fartSmeller.transform.Find("Female 4").gameObject;
		camEndPos = fartSmellerModel.transform.position + cameraCutsceneOffset + fartSmellerModel.transform.forward;

		lookTarget = fartSmellerModel.transform.position + cameraCutsceneOffset;
		

		//Debug.Log("camStartPos " + camStartPos);
		//Debug.Log("camEndPos " + camEndPos);

		journeyLength = Vector3.Distance(camStartPos, camEndPos);
		startTime = Time.time;

		// start camera movement
		moveCamera = true;

	}

	private Vector3 getGlobalPosition(GameObject obj) {
		Vector3 pos = obj.transform.position;

		while (obj.transform.parent) {
			obj = obj.transform.parent.gameObject;
			pos += obj.transform.position;
		}

		return pos;
	}
}
