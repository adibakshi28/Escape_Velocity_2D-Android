using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public GameObject background;
	public float inOrbirFollowSpeed, panSpeed;
	[HideInInspector]
	public float minCameraSize;
	[HideInInspector]
	public GameObject player;
	[HideInInspector]
	public bool inOrbit = false;
	[HideInInspector]
	public float maxCameraSize;

	private bool startFollow = false;
	private Vector3 offset;

	Camera cam;
	Animator anim;
	LevelDataController levelController;

	ValueSetter valSet;

	void Awake(){
		cam = GetComponent<Camera> ();
		if ((Screen.width == 1080) && (Screen.height == 2160)) {
			minCameraSize = 7.5f;
			cam.orthographicSize = minCameraSize;
		}
		else {
			minCameraSize = 6.5f;
		}

		levelController = GameObject.FindGameObjectWithTag ("LevelDataController").GetComponent<LevelDataController> ();
	}

	void Start(){
		anim = this.gameObject.transform.parent.transform.gameObject.GetComponent<Animator> ();
	}

	void FixedUpdate () 
	{
		if (startFollow) {
			if (!inOrbit) {
				//	transform.position = player.transform.position + offset;
		//		Vector3 followPos = new Vector3 (player.transform.position.x, player.transform.position.y, this.gameObject.transform.position.z);
				transform.position = Vector3.Lerp(transform.position, player.transform.position + offset, inOrbirFollowSpeed*Time.deltaTime);
				if (levelController.panCamera) {
					if (cam.orthographicSize > minCameraSize) {
						cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, minCameraSize, panSpeed*Time.deltaTime);
					}
					else {
						cam.orthographicSize = minCameraSize;
					}
				}
			}
			else {
				Vector3 followPos = new Vector3 (player.transform.position.x, player.transform.position.y, this.gameObject.transform.position.z);
				transform.position = Vector3.Lerp(transform.position, followPos, inOrbirFollowSpeed*Time.deltaTime);
				if (levelController.panCamera) {
					if (cam.orthographicSize < maxCameraSize) {
						cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, maxCameraSize, panSpeed*Time.deltaTime);
					}
					else {
						cam.orthographicSize = maxCameraSize;
					}
				}
			}
		}
		Vector3 pos = background.transform.position;
		pos.y = this.gameObject.transform.position.y;
		background.transform.position = pos;
	}
		
	public void ShakeCamera(){
		anim.SetTrigger ("Shake");
		CameraReposition ();
	}

	void CameraReposition(){
		Vector3 pos = transform.position;
		pos.x = 0;
		transform.position = pos;
		cam.orthographicSize = minCameraSize;
	}

	public void StartFollow(){
		startFollow = true;
		offset = transform.position - player.transform.position;
	}
}