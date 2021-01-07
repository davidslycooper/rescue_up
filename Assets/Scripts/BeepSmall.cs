using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeepSmall : MonoBehaviour
{
    Beep beep;

    GameObject player;
    float distance;

    bool alreadyDetect;
    public int direction;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        beep = GetComponentInParent<Beep>();
    }

    void Update()
    {
        UpdatePos();
    }

    void UpdatePos()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        transform.localPosition = Vector3.zero + Vector3.up * distance /4f * direction;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Cave") && !alreadyDetect)
        {
            beep.Blocked();
            alreadyDetect = true;
        }
    }
}
