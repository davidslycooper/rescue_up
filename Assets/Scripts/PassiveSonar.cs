using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSonar : MonoBehaviour
{

    CircleCollider2D coll;

    public AudioClip alert;
    Vector3 alertPos;

    void Start()
    {
        coll = GetComponent<CircleCollider2D>();
    }

    void Alert()
    {
        AudioSource.PlayClipAtPoint(alert, alertPos);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Detectable") && other.transform.position != alertPos)
        {
            alertPos = other.transform.position;
            Alert();
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "Enemy")
        {
            if (other.gameObject.GetComponent<EnemyBehavior>() != null)
            {
                EnemyBehavior enemy = other.gameObject.GetComponent<EnemyBehavior>();
                enemy.Roar();
            }
        } else if (other.gameObject.tag == "Treasure") 
        {
            if (other.gameObject.GetComponent<Treasure>() != null)
            {
                Treasure treasure = other.gameObject.GetComponent<Treasure>();
                treasure.Dolphin();
            }
        }
    }

    public void UpdateSonar()
    {
        coll.radius += 2;
    }
}