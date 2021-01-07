using UnityEngine;

public class SpriteRandomizer : MonoBehaviour {

	public bool weighted;
	public Sprite[] sprites;

	void Start () {
		if (weighted) {
			GetComponent<SpriteRenderer>().sprite = sprites[Mathf.Abs(Random.Range(0, sprites.Length) - Random.Range(0, sprites.Length))];
		} else {
			GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
		}
	}
}
