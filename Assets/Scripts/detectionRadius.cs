using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectionRadius : MonoBehaviour
{
    public GameObject fartParticleParent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void updateRadius(int scaler) {
        Vector3 newScale = this.transform.localScale;
        newScale.x = scaler;
        newScale.z = scaler;
        this.transform.localScale = newScale;
    }

    // Update is called once per frame
    void Update()
    {
        int particleCount = fartParticleParent.transform.childCount;
        updateRadius(particleCount);
        //Debug.Log(this.transform.localScale);
    }
}
