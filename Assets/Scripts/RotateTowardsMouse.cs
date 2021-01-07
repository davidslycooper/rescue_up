using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsMouse : MonoBehaviour
{
    public bool bottom;
    public bool top;

    void FixedUpdate()
    {
        Vector3 mousePos = Input.mousePosition;

        if ((top && bottom) || (top && mousePos.y > Screen.height/2) || (bottom && mousePos.y < Screen.height/2)) {
            mousePos.z = 0;

            Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
            mousePos.x = mousePos.x - objectPos.x;
            mousePos.y = mousePos.y - objectPos.y;

            float angle = (Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg) - 90;
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5f * Time.deltaTime);
        }
    }
}
