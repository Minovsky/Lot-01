using UnityEngine;
using System.Collections;

public class Numbers : MonoBehaviour {

	public Sprite[] numbers = new Sprite[10];
	private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ChangeNumber(int num)
	{
		spriteRenderer.sprite = numbers[num];
	}
}
