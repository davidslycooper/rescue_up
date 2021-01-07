using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class SubLightManager : MonoBehaviour
{
    UnityEngine.Experimental.Rendering.Universal.Light2D light;
    float startHeight;

    void Start()
    {
        light = gameObject.GetComponentInChildren<UnityEngine.Experimental.Rendering.Universal.Light2D>();
    }

    void Update()
    {
        if (startHeight == 0 || transform.position.y > startHeight - 9.5)
        {
            light.enabled = false;
        } else
        {
            light.enabled = true;
        }
    }

    public void SetStartPoint(Vector3 startPoint)
    {
        startHeight = startPoint.y;
    }

    public void DecBattery()
    {
        light.intensity -= 0.1f;
        light.pointLightOuterRadius -= 0.1f;
    }

    public void ChargeBattery(int power)
    {
        light.intensity += 0.1f * power;
        light.pointLightOuterRadius += 0.1f * power;
    }
}
