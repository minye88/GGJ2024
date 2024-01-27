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
    private Image fartBar;
    private float emitPeriod;
    private float emitTimer;
    private float particleDeviation;


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
        particleDeviation = 0.3f;
    }

    private bool holdingItIn() {
        if (Input.GetButton("Fire1"))
            return true;
        else
            return false;
    }

    private void keepFarting() {
        
        emitTimer += Time.deltaTime;

        if (emitTimer >= emitPeriod) {
            // instantiate particle
            GameObject particle = Instantiate(fartParticle, fartParticleParent.transform);
            particle.transform.position = particleSpawnPoint.transform.position;

            // randomise direction and assign to script
            Vector3 direction = directionMarker.transform.position - particleSpawnPoint.transform.position;
            direction.x += Random.Range(0.0f, particleDeviation);
            direction.y += Random.Range(0.0f, particleDeviation);
            direction.z += Random.Range(0.0f, particleDeviation);
            //Debug.Log(direction);
            particle.GetComponent<fartParticle>().setDirection(direction);
        
            emitTimer = 0.0f;
        }
    }

    private void stopFarting() {

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

    }

    private bool checkMeterFull() {
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
