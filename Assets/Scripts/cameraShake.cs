using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraShake : MonoBehaviour
{
    private float shakeDuration;
    private bool doShake;
    private float shakeTimer;
    private Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
	{
		shakeDuration = 0f;
        doShake = false;
        shakeTimer = 0f;
    }

	public void shake(float duration) {
        shakeDuration = duration;
        doShake = true;
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!doShake) return;

        shakeTimer += Time.deltaTime;
        if (shakeTimer >= shakeDuration) {
            doShake = false;
            transform.position = initialPosition;
            shakeTimer = 0.0f;
        }

        Vector3 currentCameraPosition = transform.position;
        Vector3 randomCameraPosition = new Vector3(
            currentCameraPosition.x + (Random.Range(0f, 1f) - 0.5f) * 0.3f,
            currentCameraPosition.y + (Random.Range(0f, 1f) - 0.5f) * 0.3f,
            currentCameraPosition.z + (Random.Range(0f, 1f) - 0.5f) * 0.3f
            );

        transform.position = randomCameraPosition;
    }
}
