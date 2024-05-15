using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {

	public float rotSpeed = 10;
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (0,0,rotSpeed);
	}
}
