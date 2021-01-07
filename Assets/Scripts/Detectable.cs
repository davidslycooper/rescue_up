using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detectable : MonoBehaviour
{
    Transform player;
    CircleCollider2D coll;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        coll = GetComponent<CircleCollider2D>();
    }
}
