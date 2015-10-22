using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour 
{
	public float speed						= 10.0f;
	public float reverseSpeed				= 10.0f;
	public float turnSpeed					= 2.5f;

	//private float tempTurnSpeed				= 0.0f;
	//private bool isTurning					= false;

	private Rigidbody2D myRigidbody;

	void Start () 
	{
		myRigidbody = GetComponent<Rigidbody2D> ();
		//StartCoroutine (TurnRight ());
	}

	void FixedUpdate () 
	{
		//myRigidbody.AddTorque(tempTurnSpeed);
		if(Input.GetKey("w"))
		{
			myRigidbody.AddRelativeForce(new Vector2(0,speed));

			if(Input.GetKeyDown("a"))
			{
				//myRigidbody.isKinematic = true;
				myRigidbody.velocity = Vector3.zero;
				myRigidbody.angularVelocity = 0.0f; 
				myRigidbody.rotation += 90.0f;
				//myRigidbody.isKinematic = false;
				/*if(!isTurning)
				{
					StartCoroutine (Turn());
					tempTurnSpeed = 20.0f;
				}*/
				//myRigidbody.AddTorque(turnSpeed);
			}
			
			else if(Input.GetKeyDown("d"))
			{
				myRigidbody.velocity = Vector3.zero;
				myRigidbody.angularVelocity = 0.0f; 
				myRigidbody.rotation -= 90.0f;
				/*if(!isTurning)
				{
					StartCoroutine (Turn());
					tempTurnSpeed = -20.0f;
				}*/
				//myRigidbody.AddTorque(-turnSpeed);
			}
		}

		if(Input.GetKey("s"))
		{
			myRigidbody.AddRelativeForce(new Vector2(0,-reverseSpeed));

			if(Input.GetKeyDown("a"))
			{
				myRigidbody.velocity = Vector3.zero;
				myRigidbody.angularVelocity = 0.0f; 
				myRigidbody.rotation -= 90.0f;
			}
			
			else if(Input.GetKeyDown("d"))
			{
				myRigidbody.velocity = Vector3.zero;
				myRigidbody.angularVelocity = 0.0f; 
				myRigidbody.rotation += 90.0f;
			}
		}
	}

	/*IEnumerator Turn()
	{
		//myRigidbody.AddTorque(tempTurnSpeed);
		yield return new WaitForSeconds (1.25f/8);
		tempTurnSpeed = 0;
		isTurning = false;
	}*/
}
