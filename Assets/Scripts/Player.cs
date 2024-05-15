using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	public GameObject cam,explosion,emptyObject,trailingObject,background;
	public float lowestSpeed = 9 ,maxSpeed = 20, blackHoleMovementspeed = 20, maxRotateSpeed = 500, attractionDistanceThreshold = 6f,accleration = 5;
	public AudioClip inOrbitClip;

	[HideInInspector]
	public bool inOrbit = false, enteredAstroidBelt = false,accControlEnabled = false, playerPaused = false;

	private Vector3 playerLinearMovementDirection;
	private bool orbitRight = false, enteredBlackHole = false,gameOver = false,correctPosition = false;
	private int closestObsticalIndex;
	private float rotateSpeed, massConstant = 0.1f,distanceDegree = 1.8f;
	private GameObject centreEmptyObject,createdTrailingObject;

	public float speed;

	LineRenderer lineRend;
	CameraFollow cameraScript;
	TestureScrolling bkTextureScript;
	AudioSource aud;
	LevelDataController levelController;

	ValueSetter valSet;

	void Awake(){
		levelController = GameObject.FindGameObjectWithTag ("LevelDataController").GetComponent<LevelDataController> ();

		cameraScript = cam.GetComponent<CameraFollow> ();
		bkTextureScript = background.GetComponent<TestureScrolling> ();
		cameraScript.player = this.gameObject;
		playerLinearMovementDirection = new Vector3 (0, 1, 0);
		speed = lowestSpeed;
	}

	void Start(){
		aud = GetComponent<AudioSource> ();
		lineRend = GetComponent<LineRenderer> ();
		lineRend.enabled = false;
		cameraScript.StartFollow ();
	}
		
	void Update(){
		if ((!gameOver)&&(!playerPaused)) {
			if ((!enteredBlackHole)&&(!enteredAstroidBelt)) {
				if (Input.GetMouseButtonDown (0)) {
					NearestObstical ();
					lineRend.enabled = true;
					bkTextureScript.scroll = false;
				} else {
					if (Input.GetMouseButtonUp (0) && (inOrbit)) {
						speed = ((rotateSpeed / maxRotateSpeed) * (maxSpeed - lowestSpeed)) + lowestSpeed;
						inOrbit = false;
						bkTextureScript.scroll = true;
						cameraScript.inOrbit = false;
						cameraScript.player = this.gameObject;
						lineRend.enabled = false;
						aud.Stop ();
						Destroy (createdTrailingObject);
						Destroy (centreEmptyObject);
					}
				}
			}
			else {                  // Inside Black Hole or Astroid Belt 
				if(enteredAstroidBelt){
					if (inOrbit) {
						inOrbit = false;
						aud.Stop ();
						Destroy (createdTrailingObject);
						Destroy (centreEmptyObject);
						lineRend.enabled = false;
					}
					bkTextureScript.scroll = true;
					if (transform.eulerAngles != Vector3.zero) {
						transform.eulerAngles = Vector3.Lerp (transform.eulerAngles, Vector3.zero, 50 * Time.deltaTime);
					}
					else {
						transform.eulerAngles = Vector3.zero;
					}
					if (!correctPosition) {
						if (Mathf.Abs(transform.position.x) > 0.3f) {
							GetComponent<CircleCollider2D> ().enabled = false;
							Vector3 pos = transform.position;
							if (pos.x > 0) {
								pos.x = pos.x - 0.1f;
							}
							else {
								pos.x = pos.x + 0.1f;
							}
							transform.position = pos;
						}
						else {
							GetComponent<CircleCollider2D> ().enabled = true;
							correctPosition = true;
						}
					}
				}
				if(accControlEnabled){
					Vector3 pos = transform.position;
					pos.x = pos.x + (Input.acceleration.x*blackHoleMovementspeed*Time.deltaTime);
					transform.position = pos;
				}
			}
		}
	}

	void FixedUpdate(){
		if ((!gameOver)&&(!playerPaused)) {
			if (inOrbit) {
				if (orbitRight) {
					transform.RotateAround (levelController.obsticalList [closestObsticalIndex].transform.position, Vector3.back, rotateSpeed * Time.deltaTime);
				} else {      // orbitLeft
					transform.RotateAround (levelController.obsticalList [closestObsticalIndex].transform.position, Vector3.forward, rotateSpeed * Time.deltaTime);
				}
				lineRend.SetPosition (0, this.transform.position);
				Vector3 obsPos = levelController.obsticalList [closestObsticalIndex].transform.position;
				//	obsPos.z = 0.2f;      // To make line below planet
				lineRend.SetPosition (1, obsPos);
			}
			else {
				transform.Translate (playerLinearMovementDirection*speed*Time.deltaTime);
				speed = speed + (accleration * Time.deltaTime);
				if (speed > lowestSpeed) {
					speed = speed - 0.1f;
				}
				else {
					speed = lowestSpeed;
				}
			}
		}
	}
		

	void NearestObstical(){
		float shortestDistance, distance;
		distance = Mathf.Abs(Vector3.Distance(this.transform.position,levelController.obsticalList[0].transform.position));
		shortestDistance = distance;
		closestObsticalIndex = 0;
		for (int i = 1; i < levelController.obsticalList.Count; i++) {
			distance = Mathf.Abs(Vector3.Distance(this.transform.position,levelController.obsticalList[i].transform.position));
			if (shortestDistance > distance) {
				shortestDistance = distance;
				closestObsticalIndex = i;
			}
		}
		if (shortestDistance < attractionDistanceThreshold) {
			if (levelController.obsticalList [closestObsticalIndex].transform.position.x > this.gameObject.transform.position.x) {
				orbitRight = true;
			}
			else {
				orbitRight = false;
			}
			SetRotationSpeed (shortestDistance,levelController.obsticalList [closestObsticalIndex]);
			ChangePlayerOrientation ();
			ChangeCameraFocus (shortestDistance);
			inOrbit = true;

			Vector3 pos = this.gameObject.transform.position;
			pos.z = 0.5f;
			createdTrailingObject = Instantiate (trailingObject, pos, Quaternion.identity)as GameObject;
			TrailingObject trO;
			trO = createdTrailingObject.GetComponent<TrailingObject> ();
			trO.player = this.gameObject;
			trO.mat = levelController.obsticalList [closestObsticalIndex].GetComponent<asteroid_rotate> ().trailMaterial;
			if (PlayerPrefs.GetInt ("Sound") == 1) {
				aud.clip = inOrbitClip;
				aud.loop = true;
				aud.Play ();
			}
		}
		else {
			Debug.Log ("No near obstical");
		}
	}

	void SetRotationSpeed(float shortestDistance,GameObject obstical){
		float mass;
		Vector2 minMaxMass = obstical.GetComponent<asteroid_rotate> ().sizeRange;
		mass = obstical.transform.localScale.x;
		float massNormalized = (mass - minMaxMass.x) / (minMaxMass.y - minMaxMass.x);  // Value ranges from 0 to 1 

		rotateSpeed = ((maxRotateSpeed * (1 - massConstant)) / (Mathf.Pow(shortestDistance,distanceDegree))) + ((maxRotateSpeed * massNormalized) * massConstant);
	}

	void ChangePlayerOrientation(){
		Vector3 a = this.gameObject.transform.position - levelController.obsticalList [closestObsticalIndex].transform.position;
		Vector3 b;
			
		if (orbitRight) {
			b = Vector3.forward;
		} else {
			b = Vector3.back;
		}

		transform.up = Vector3.Cross (a, b);
	}

	void ChangeCameraFocus(float shortestDistance){
		if (!levelController.panCamera) {
			Vector3 pos = (this.gameObject.transform.position + levelController.obsticalList [closestObsticalIndex].transform.position) / 2;
			centreEmptyObject = Instantiate (emptyObject, pos, Quaternion.identity)as GameObject;
			centreEmptyObject.transform.parent = this.gameObject.transform;
			cameraScript.player = centreEmptyObject;
			cameraScript.inOrbit = true;
		}
		else {
			cameraScript.player = levelController.obsticalList [closestObsticalIndex];
			switch ((int)shortestDistance) {
			case 1:
				cameraScript.maxCameraSize = cameraScript.minCameraSize;
				break;
			case 2:
				cameraScript.maxCameraSize = cameraScript.minCameraSize;
				break;
			case 3:
				cameraScript.maxCameraSize = 8;
				break;
			case 4:
				cameraScript.maxCameraSize = 9;
				break;
			case 5:
				cameraScript.maxCameraSize = 10;
				break;
			default:
				cameraScript.maxCameraSize = 11;
				break;
			}
			cameraScript.inOrbit = true;
		}
	}


	void EnterBlackHole(){
		if (inOrbit) {
			inOrbit = false;
			aud.Stop ();
			Destroy (createdTrailingObject);
			Destroy (centreEmptyObject);
		}
		GetComponent<CircleCollider2D> ().enabled = false;
		this.gameObject.transform.GetChild (0).transform.gameObject.SetActive (false);
		GetComponent<TrailRenderer> ().enabled = false;
		lineRend.enabled = false;
		levelController.EnterBlackHole ();
	}

	public void EnterBlackHoleRegion(){
		bkTextureScript.scroll = true;
		bkTextureScript.scrollSpeed = bkTextureScript.scrollSpeed / 2;
		speed = lowestSpeed;
		Vector3 pos = transform.position;
		pos.x = 0;
		transform.position = pos;
		Vector3 rot = transform.eulerAngles;
		rot = new Vector3 (0,0,0);
		transform.eulerAngles = rot;
		GetComponent<CircleCollider2D> ().enabled = true;
		this.gameObject.transform.GetChild (0).transform.gameObject.SetActive (true);
		GetComponent<TrailRenderer> ().enabled = true;
		accControlEnabled = true;
	}

    void EnterWhiteHole(){
		inOrbit = false;
		accControlEnabled = false;
		GetComponent<CircleCollider2D> ().enabled = false;
		this.gameObject.transform.GetChild (0).transform.gameObject.SetActive (false);
		GetComponent<TrailRenderer> ().enabled = false;
		lineRend.enabled = false;
		levelController.EnterWhiteHole ();
	}

	public void EnterWhiteHoleRegion(){
		bkTextureScript.scroll = true;
		bkTextureScript.scrollSpeed = bkTextureScript.scrollSpeed * 2;
		speed = lowestSpeed;
		Vector3 pos = transform.position;
		pos.x = 0;
		transform.position = pos;
		Vector3 rot = transform.eulerAngles;
		rot = new Vector3 (0,0,0);
		transform.eulerAngles = rot;
		GetComponent<CircleCollider2D> ().enabled = true;
		this.gameObject.transform.GetChild (0).transform.gameObject.SetActive (true);
		enteredBlackHole = false;
		GetComponent<TrailRenderer> ().enabled = true;
	}

	public void ForceConnect(){
		NearestObstical ();
		lineRend.enabled = true;
		bkTextureScript.scroll = false;
	}

	public void GameOver(){
		if (!gameOver) {
			gameOver = true;
			levelController.gameOver = true;
			bkTextureScript.scroll = false;
			speed = 0;
			maxRotateSpeed = 0;
			attractionDistanceThreshold = 0;
			GameObject exp;
			exp = Instantiate (explosion, transform.position, Quaternion.identity)as GameObject;
			if (PlayerPrefs.GetInt ("Sound") == 1) {
				exp.GetComponent<AudioSource> ().Play ();
			}
			Destroy(this.gameObject.transform.GetChild(0).transform.gameObject);
			lineRend.enabled = false;
			levelController.GameOver ();
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Enemy") {
			GameOver ();
		}
		if ((col.gameObject.tag == "BlackHole")&&(!enteredBlackHole)) {
			col.gameObject.GetComponent<CircleCollider2D> ().enabled = false;
			enteredBlackHole = true;
			EnterBlackHole ();
			Destroy (col.gameObject.transform.parent.transform.gameObject, 0.67f);
		}
		if ((col.gameObject.tag == "WhiteHole")&&(enteredBlackHole)) {
			col.gameObject.GetComponent<BoxCollider2D> ().enabled = false;
			EnterWhiteHole ();
			Destroy (col.gameObject.transform.parent.transform.gameObject, 0.5f);
		}
	}

	void OnTriggerStay2D(Collider2D col)
	{
		if ((col.gameObject.tag == "Boundry")&&(!inOrbit)) {
			GameOver ();
		}
	}

}