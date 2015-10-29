using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StandInCar : Car
{
	public Sprite[] carSprites = new Sprite[4];

    public int color = -1;
    public override void Start ()
    {
        if (color < 0) {
		int randomValue = Random.Range (0, 4);
		GetComponentInChildren<SpriteRenderer> ().sprite = carSprites [randomValue];
        color = randomValue;
        }
    }

    public override void Update()
    {
        /* DO NOTHING */
    }

    public override void UnPark()
    {
        World.Instance.LeaveFrom(worldLocation, direction);
        Destroy(this.gameObject);
        CarSpawner.Instance.AddCar();
    }

	public void ChangeColor(int value)
	{
        color = value;
		GetComponentInChildren<SpriteRenderer> ().sprite = carSprites [value];
	}
}
