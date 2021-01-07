using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beep : MonoBehaviour
{
    ActiveSonar sonar;

    BeepBig big;

    AudioSource ping;
    AudioSource beepAS;
    public AudioClip pong;
    public AudioClip poc;

    [Range(1f,8f)]
    public float lifeTime;
    float destroyTime;

    float beepSpeed;

    int blocked;
    bool ponged;

	void Start()
	{
        big = GetComponentInChildren<BeepBig>();
        sonar = GameObject.FindWithTag("Player").GetComponentInChildren<ActiveSonar>();
        ping = GetComponents<AudioSource>()[0];
        beepAS = GetComponents<AudioSource>()[1];

        ping.pitch = UnityEngine.Random.Range(0.95f, 1.05f);
        ping.Play();

        destroyTime = Time.time + lifeTime;
    }

	void FixedUpdate()
	{
		transform.position = transform.position + transform.right * beepSpeed;

        if (Time.time > destroyTime)
        {
            Destroy(gameObject);
        }
	}

    public float GetSpeed()
    {
        return beepSpeed;
    }

    public void SetSpeed(float speed)
    {
        beepSpeed = speed;
    }

    public void Blocked()
    {
        blocked++;

        if (blocked > 2)
        {
            Poc();
            CannotDetect();
        }
    }

    public void Pong(Vector3 pos, string tag, int id)
    {
        beepAS.clip = pong;
        //beepAS.spatialBlend = 0;
        beepAS.Play();

        switch (tag)
        {
            case "Coal":
                sonar.Detect(0, pos, id);
                break;
            case "Uranium":
                sonar.Detect(1, pos, id);
                break;
            case "Quartz":
                sonar.Detect(2, pos, id);
                break;
            case "Amber":
                sonar.Detect(3, pos, id);
                break;
            case "Treasure":
                sonar.Detect(4, pos, id);
                break;
            case "Enemy":
                sonar.Detect(5, pos, id);
                break;
        }
    }

    public void Poc()
    {
        beepAS.clip = poc;
        beepAS.volume = 0.4f;
        beepAS.Play();
    }

    void CannotDetect()
    {
        big.CannotDetect();
    }

    public void Stop()
    {
        beepSpeed = 0;
        destroyTime = Time.time + 2f;
    }
}
