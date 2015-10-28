using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	public int time = 120;
	public GameObject[] numberObjects = new GameObject[3];
	public GameObject late;
	public GameObject trainActive;
	public GameObject trax;
	public bool trainArrived = false;

	private int minutes;
	private int seconds;
	private bool reachedZero = false;
	private Numbers[] numScript = new Numbers[3];
	private SpriteRenderer lateSprite;
	
	void Start ()
	{
		minutes = time / 60;
		seconds = time % 60;
		StartCoroutine (CountDown ());
		numScript[0] = numberObjects[0].GetComponent<Numbers> ();
		numScript[1] = numberObjects[1].GetComponent<Numbers> ();
		numScript[2] = numberObjects[2].GetComponent<Numbers> ();
		lateSprite = late.GetComponent<SpriteRenderer> ();
		lateSprite.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
	}

	IEnumerator CountDown()
	{
		yield return new WaitForSeconds (1.0f);
		if (!reachedZero)
			time--;
		else
			time++;

		if (time == 0)
		{
			OnTrainArrived();
		}

		minutes = time / 60;
		seconds = time % 60;
		StartCoroutine (CountDown ());
	}
	
	void Update ()
	{
		numScript[0].ChangeNumber(minutes);
		numScript[1].ChangeNumber(seconds/10);
		numScript[2].ChangeNumber(seconds%10);
	}

    void OnTrainArrived()
    {
		if(!trainArrived)
		{
			trainArrived = true;
			Instantiate(trax);
		}
		reachedZero = true;
		lateSprite.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
		trainActive.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
	}
}
