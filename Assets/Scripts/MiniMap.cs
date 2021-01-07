using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public GameObject minimap;
    bool activated;
    GameObject mm;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && Input.GetKey(KeyCode.LeftControl))
        {
            if (!activated)
            {
                mm = Instantiate(minimap, new Vector3(0, 0, -20), Quaternion.identity);
                activated = true;
            }
            else
            {
                Destroy(mm, 0);
                activated = false;
            }
        }
        
    }
}
