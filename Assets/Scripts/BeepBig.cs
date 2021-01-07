using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeepBig : MonoBehaviour
{
    GameObject player;

    Beep beep;

    float adj;

    float distance;
    GameObject detected;
    bool alreadyDetect;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        beep = GetComponentInParent<Beep>();

        adj = beep.GetSpeed() / 3.5f;
    }

    void FixedUpdate()
    {
        UpdateCollider();
    }

    public void CannotDetect()
    {
        alreadyDetect = true;
    }

    void UpdateCollider()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        transform.localScale = new Vector2(Mathf.Max(distance * 7f, 5f), Mathf.Max(distance * 7f, 5f));

        transform.position -= transform.right * adj;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Detectable") && !alreadyDetect)
        {
            detected = other.gameObject;

            string tag = detected.tag;

            int id = 0;

            if (tag == "Coal" || tag == "Uranium" || tag == "Quartz" || tag == "Amber")
            {
                id = detected.GetComponentInParent<Caught>().mineralInd;
            }
            else if (tag == "Enemy")
            {
                id = detected.GetComponentInParent<EnemyBehavior>().GetId();
            } 

            beep.Pong(detected.transform.position, tag, id);
            beep.Stop();
            alreadyDetect = true;
        }
    }
}
