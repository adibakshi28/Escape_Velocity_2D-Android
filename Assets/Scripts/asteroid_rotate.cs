using UnityEngine;
using System.Collections;

public class asteroid_rotate : MonoBehaviour {

	public float rotSpeed = 5,ossilationSpeed = 10;
	public Vector2 sizeRange;
	public Material trailMaterial;
	[HideInInspector]
	public bool osilate = false;

	private float z;
	private bool changed = false;

	LevelDataController levelController;
	ValueSetter valSet;

	void Start(){
		z = Random.Range (0, rotSpeed);
		Vector3 size = transform.localScale;
		float sz = Random.Range (sizeRange.x, sizeRange.y);
		size.x = sz;
		size.y = sz;
		size.z = sz;
		transform.localScale = size;
		if (Random.Range (1, 5) % 4 == 0) {
			osilate = true;
		}
		else {
			osilate = false;
		}

		if (osilate) {
			ossilationSpeed = Random.Range (0, ossilationSpeed);
		}
	}

	void FixedUpdate () {
		if (osilate) {
			transform.Translate (ossilationSpeed * Time.deltaTime, 0, 0);
		}

		else {
			transform.Rotate (0,0,z);
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if ((col.gameObject.tag == "Boundry")&&(!changed)) {
			ossilationSpeed = -1 * ossilationSpeed;
			changed = true;
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if (col.gameObject.tag == "Boundry") {
			changed = false;
		}
	}
}

















































