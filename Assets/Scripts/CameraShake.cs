using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private float shakeDuration;

    [Range(0, 1)]
    public float shakeMagnitude;

    [Range(0,5)]
    public float dampingSpeed;

    Vector3 initialPosition;

    void FixedUpdate()
    {
        if (shakeDuration > 0)
        {
            transform.position = initialPosition + (Vector3) Random.insideUnitCircle * shakeMagnitude;

            shakeDuration -= dampingSpeed;
        }
        else
        {
            shakeDuration = 0f;
        }
    }

    public void Shake()
    {
        initialPosition = transform.position;
        shakeDuration = 2.0f;
    }
}
