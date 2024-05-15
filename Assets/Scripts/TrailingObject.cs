using UnityEngine;
using System.Collections;

public class TrailingObject : MonoBehaviour {

	[HideInInspector]
	public GameObject player;
	[HideInInspector]
	public Material mat;

	Vector3 pos;

	TrailRenderer trail;

	void Start(){
		trail = GetComponent<TrailRenderer> ();
		if (mat != null) {
			trail.material = mat;
		}
	}

	void FixedUpdate () {
		pos = transform.position;
		pos.x = player.transform.position.x;
		pos.y = player.transform.position.y;
		pos.z = 0.5f;
		transform.position = pos;
	}
}
