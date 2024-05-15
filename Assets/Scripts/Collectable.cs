using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour {

	public int scoreIncrease = 2;

	private bool collected = false;

	AudioSource aud;
	LevelDataController levelController;

	void Start(){
		levelController = GameObject.FindGameObjectWithTag ("LevelDataController").GetComponent<LevelDataController> ();
		aud = GetComponent<AudioSource> ();
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if ((col.gameObject.tag == "Player")&&(!collected)) {
			collected = true;
			if (PlayerPrefs.GetInt ("Sound") == 1) {
				aud.Play ();
			}
			levelController.ScoreIncrease (scoreIncrease);
			GetComponent<SpriteRenderer> ().enabled = false;    // Collectable is destroyed by the level data controller (Dont destroy here)
		}
	}
}
