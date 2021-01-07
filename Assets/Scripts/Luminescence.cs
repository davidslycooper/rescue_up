using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class Luminescence : MonoBehaviour
{

    UnityEngine.Experimental.Rendering.Universal.Light2D light;
    bool enabled;
    float turnOff;

    [Range(0,2)]
    public float OnTime;

    void Start()
    {
        light = gameObject.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
        light.enabled = false;
    }

    void Update()
    {
        if (enabled && Time.time > turnOff)
        {
            light.enabled = false;
            enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            light.enabled = true;
            enabled = true;
            turnOff = Time.time + OnTime;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            light.enabled = true;
            enabled = true;
            turnOff = Time.time + OnTime;
        }
    }
}
