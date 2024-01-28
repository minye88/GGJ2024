using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fart : MonoBehaviour
{
    public float fartBarRaiseSpeed;
    public float fartBarDropSpeed;
    public float particleEmissionRate;
    public GameObject fartParticle;
    public GameObject fartParticleParent;
    public GameObject directionMarker;
    public GameObject particleSpawnPoint;
    public GameObject fartSoundObject;
    private AudioSource fartSound;
    public GameObject explosionSoundObject;
    private AudioSource explosionSound;
    private Image fartBar;
    private float emitPeriod;
    private float emitTimer;
    private float directionDeviation;
    private float sizeDeviation;
    public GameObject detectionRadius;
    public GameObject poop;
    public GameObject poops;

    // Start is called before the first frame update
    void Start()
    {
        fartBar = GameObject.Find("Fart Bar").GetComponent<Image>();

        // if (fartBar)
        //     Debug.Log("Fartbar found");
        // else
        //     Debug.Log("Fartbar not found");

        emitPeriod = 1 / particleEmissionRate;
        emitTimer = 0.0f;
        directionDeviation = 0.3f;
        sizeDeviation = 0.2f;

        fartSound = fartSoundObject.GetComponent<AudioSource>();
        explosionSound = explosionSoundObject.GetComponent<AudioSource>();
    }

    private bool holdingItIn() {
        if (Input.GetButton("Fire1"))
            return true;
        else
            return false;
    }

    private void keepFarting() {
        
        emitTimer += Time.deltaTime;

        if (emitTimer >= emitPeriod && fartParticleParent.transform.childCount < particleEmissionRate) {
            // instantiate particle
            GameObject particle = Instantiate(fartParticle, fartParticleParent.transform);
            particle.transform.position = particleSpawnPoint.transform.position;

            // randomize particle size
            Vector3 scale = particle.transform.localScale;
            float randomSize = scale.x + (Random.Range(0.0f, sizeDeviation) * Random.Range(0, 2));
            scale.x = scale.y = scale.z = randomSize;
            particle.transform.localScale = scale;

            // randomise direction and assign to script
            Vector3 direction = directionMarker.transform.position - particleSpawnPoint.transform.position;
            direction.x += (Random.Range(0.0f, directionDeviation) * Random.Range(0, 2));
            direction.y += (Random.Range(0.0f, directionDeviation) * Random.Range(0, 2));
            direction.z += (Random.Range(0.0f, directionDeviation) * Random.Range(0, 2));
            //Debug.Log(direction);
            particle.GetComponent<fartParticle>().setDirection(direction);
        
            emitTimer = 0.0f;
        }

        if (!fartSound.isPlaying)
            fartSound.Play();
    }

    private void stopFarting() {
       if (fartSound.isPlaying)
           fartSound.Pause();
    }

    private void raiseMeter() {
        fartBar.fillAmount += (fartBarRaiseSpeed * 0.0001f);
        //Debug.Log("raising");
    }

    private void dropMeter() {
        fartBar.fillAmount -= (fartBarDropSpeed * 0.0001f);
        //Debug.Log("dropping");
    }

    private void explode() {
        Debug.Log("explode");

        fartBar.fillAmount = 0.0f;
        explosionSound.Play();

        float currentRadiusModifier = detectionRadius.GetComponent<detectionRadius>().getRadiusModifier();
        detectionRadius.GetComponent<detectionRadius>().setRadiusModifier(currentRadiusModifier + 5.0f);

        // camera shake
        Camera.main.GetComponent<cameraShake>().shake(2f);

        // create shit on the ground
        GameObject newPoop = Instantiate(poop, poops.transform);
        newPoop.transform.position = transform.position;
    }

    private bool checkMeterFull() {
        if (fartBar.fillAmount >= 1.0f)
            return true;

        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (holdingItIn()) {
            stopFarting();
            raiseMeter();

            if (checkMeterFull())
                explode();

        } else {
            dropMeter();
            keepFarting();
        }
            
        //Debug.Log("Number of particles: " + fartParticleParent.transform.childCount);

    }
}
