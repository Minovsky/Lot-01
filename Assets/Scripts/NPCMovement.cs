using UnityEngine;
using System.Collections;

public class NPCMovement : MonoBehaviour 
{
	public float speed						= 10.0f;
	public float reverseSpeed				= 10.0f;
	public float turnSpeed					= 2.5f;
	
	private float tempTurnSpeed				= 0.0f;
	private bool isTurning					= false;
	
	private Rigidbody2D myRigidbody;

	//Random.seed = (int) System.DateTime.Now.Ticks;

	void Start () 
	{
		myRigidbody = GetComponent<Rigidbody2D> ();
	}
	
	void FixedUpdate () 
	{
		myRigidbody.AddRelativeForce(new Vector2(0,speed));

	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Trigger")
		{
			myRigidbody.velocity = Vector3.zero;
			myRigidbody.angularVelocity = 0.0f; 
			if(other.transform.position.x > 0)
				myRigidbody.rotation += Random.Range(-1,2) * 90.0f;
			else
				myRigidbody.rotation += Random.Range(-1,2) * 90.0f;
			Debug.Log (myRigidbody.rotation);
		}
	}

	IEnumerator Turn()
	{
		yield return new WaitForSeconds (1.25f/8);
		tempTurnSpeed = 0;
		isTurning = false;
	}
}
