using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	public int time = 120;
	public GameObject[] numberObjects = new GameObject[3];
	public GameObject late;
	public GameObject trainActive;
	public GameObject trax;
	public GameObject pedestrianSpawner;
	public bool trainArrived = false;
	public AudioClip[] radio = new AudioClip[3];
	public AudioClip trainArrivalSound;

	private int minutes;
	private int seconds;
	private bool reachedZero = false;
	private Numbers[] numScript = new Numbers[3];
	private SpriteRenderer lateSprite;
	private AudioSource audioSource;
	
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
		audioSource = GetComponent<AudioSource> ();
		if(!audioSource.isPlaying)
			audioSource.PlayOneShot(radio[Random.Range (0,radio.Length)]);
	}

	IEnumerator CountDown()
	{
		yield return new WaitForSeconds (1.0f);
		if (!reachedZero)
			time--;
		else
			time++;

		if (time == 5)
		{
			if(!trainArrived)
			{
					audioSource.PlayOneShot(trainArrivalSound);
			}
		}

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
			StartCoroutine(SpawnPedestrians());
		}
		reachedZero = true;
		lateSprite.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
		trainActive.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
	}

	IEnumerator SpawnPedestrians()
	{
		yield return new WaitForSeconds (2.0f);
		Instantiate (pedestrianSpawner);
	}
}
