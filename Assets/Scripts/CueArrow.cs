using UnityEngine;

public class CueArrow : MonoBehaviour
{
    Vector3 cuePos;
    Transform player;

    EntityManager entityMan;

    bool coals;

    void Start()
    {
        cuePos = new Vector3(-5f, 10f);
        player = GameObject.FindWithTag("Player").transform;

        entityMan = transform.root.gameObject.GetComponent<EntityManager>();
    }

    void FixedUpdate()
    {
        if (!coals && entityMan.caught == 4)
        {
            if (player.position.x > 4f)
            {
                cuePos = new Vector3(0.2f, -12.7f);
            }
            coals = true;
        }

        if (entityMan.caught == 6)
        {
            gameObject.SetActive(false);
        }

        if (Vector3.Distance(player.position, cuePos) < 2f)
        {
            transform.position = new Vector3(0, 40f);
        }
        else
        {
            float x = cuePos.x - transform.position.x;
            float y = cuePos.y - transform.position.y;

            float angle = (Mathf.Atan2(y, x) * Mathf.Rad2Deg);
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5f);

            transform.position = player.position + transform.right * 1f;
        }
    }

    public void SetPos(Vector3 pos)
    {
        cuePos = pos;
    }
}
