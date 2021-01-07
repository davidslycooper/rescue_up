using UnityEngine;
using UnityEngine.UI;

public class ImageRandomizer : MonoBehaviour
{
	public bool awake;
	public Sprite[] sprites;

	void Start()
	{
		if (awake)
		{
			GetComponent<Image>().sprite = sprites[Random.Range(0, sprites.Length)];
		}
	}

	public void Set(int set)
    {
		Sprite sprite = sprites[0];
			if (set == 0)
			{
				sprite = sprites[Random.Range(0, 3)];
			}
			else if (set == 1)
			{
				sprite = sprites[Random.Range(3, 5)];
			}
			else if (set == 2)
			{
				sprite = sprites[Random.Range(5, 8)];
			}
			else if (set == 3)
			{
				sprite = sprites[Random.Range(8, 10)];
			}
			GetComponent<Image>().sprite = sprite;
	}
}
