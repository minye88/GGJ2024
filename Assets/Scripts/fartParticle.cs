using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fartParticle : MonoBehaviour
{
    public float minimiseRate;
    public float slowRate;
    public float lifetime;
    private Vector3 particleDirection;
    private float timeAlive;
    private Material material;
    //private float transparency;

    // Start is called before the first frame update
    void Start()
    {
        timeAlive = 0.0f;
        material = this.GetComponent<Renderer>().material;
        //transparency = 1.0f;
        //Debug.Log(material.color[0].ToString());
        
    }

    public void setDirection(Vector3 direction) {
        particleDirection = direction.normalized;
    }

    private void move() {
        this.transform.position += particleDirection * 0.005f;
    }

    private void removeParticle() {

        Color newColor = material.color;
        newColor.a -= 0.05f;
        material.color = newColor;

        if (material.color.a <= 0.1) 
            Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        timeAlive += Time.deltaTime;
        
        if (timeAlive >= lifetime) {
            removeParticle();
        }

        move();
    }
}
