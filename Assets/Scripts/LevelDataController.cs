using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelDataController : MonoBehaviour {

	public GameObject canvas, cam, player, background, blackHole, whiteHole,acclorometerIcon,msgTextBox, countdownTextBox,entryParticles,blackHoleCollectables,ftuePanel;
	public List<GameObject> obstical;
	public List<GameObject> collectables;
	public List<GameObject> astroids;
	public float minX, maxX, minY, maxY, obsticalStartDistance,updateScoreDistance;  //(y values is the range of y coodinate of next obstical from previous obstical,,, x value is indipendent)
	public int scoreUpdate = 1,appriciateScore = 15,collectableRandomSeed = 5,blackHoleWorldSize = 50,astroidBeltSize = 200;
	public bool panCamera = false;
	public Material whiteWorldMaterial,blackWorldMaterial;
	public Text scoreText, appriciationText,ftueText;
	public List<string> appriciationMsg;
	public Sprite soundOn,soundOff;
	public Button sound;

	[HideInInspector]
	public List<GameObject> obsticalList,collectableList;
	[HideInInspector]
	public int score = 0;
	[HideInInspector]
	public bool gameOver = false,inBlackHole = false,inSpace = true,inAstroidBelt = false;

	private Text msgText,countdownText;
	private bool firstCollectableCreated = false,playingFirstTime = false,planetFtuShown = false;
	private float scoreDistance, minObsticalYCoodinate, maxObsticalYCoodinate, minCollectableYCoodinate = 0, lastObsticalXCood, astroidBeltStartingDistance;
	private int lastCreatedPlanetIndex;
	private GameObject cn;

	AudioSource aud;
	Animator canvasAnim;
	CameraFollow cameraScript;
	Player playerScript;

	GameObject valueSetterObject;

	void Awake(){     // Setting up the environment
		PlayerPrefs.DeleteAll();
		if (PlayerPrefs.GetInt ("TimesPlayed") == 0) {
			playingFirstTime = true;
			PlayerPrefs.SetInt ("TimesPlayed", (PlayerPrefs.GetInt ("TimesPlayed") + 1));
		}
		else {
			playingFirstTime = false;
		}


		aud = GetComponent<AudioSource> ();
		playerScript = player.GetComponent<Player> ();
		cameraScript = cam.GetComponent<CameraFollow> ();

		// Create first Obstical
		lastObsticalXCood = -2;
		Vector3 pos = new Vector3 (lastObsticalXCood,obsticalStartDistance,0);
		GameObject firstObstical;
		lastCreatedPlanetIndex = Random.Range (0, obstical.Count);
		firstObstical = Instantiate(obstical[lastCreatedPlanetIndex],pos,Quaternion.identity)as GameObject;
		firstObstical.GetComponent<asteroid_rotate> ().ossilationSpeed = 0;
		minObsticalYCoodinate = obsticalStartDistance;
		maxObsticalYCoodinate = obsticalStartDistance;
		obsticalList.Add (firstObstical);
		CreateObstical (5);                   // Create 5 obsticals in starting
		if (PlayerPrefs.GetInt ("Sound") == 0) {
			sound.image.sprite = soundOff;
			aud.Stop ();   // Stop background Music
		}
		else {
			sound.image.sprite = soundOn;
			aud.Play ();    // Play background Music
		}
		acclorometerIcon.SetActive(false);
	}

	void Start(){
		canvasAnim = canvas.GetComponent<Animator> ();
		scoreText.text = score.ToString ();
		scoreDistance = updateScoreDistance;
		msgText = msgTextBox.GetComponent<Text> ();
		msgTextBox.SetActive (false);
		countdownText = countdownTextBox.GetComponent<Text> ();
		countdownTextBox.SetActive (false);
	}

	void Update(){
		if (!gameOver) {
			
			float playerDistance = player.transform.position.y;

			if ((playerDistance > obsticalStartDistance - 3f) && (playingFirstTime)&&(!planetFtuShown)) {
				PlanetOrbitFTU ("Tap and hold near a planet to orbit");
			}

			if (playerDistance > scoreDistance) {
				scoreDistance = scoreDistance + updateScoreDistance;
				ScoreIncrease (scoreUpdate);
			}

			if ((!inBlackHole)&&(inSpace)) {
				if (maxObsticalYCoodinate < playerDistance + 20) {
					CreateObstical ();
				}

				if ((minObsticalYCoodinate + 20 < playerDistance) && (!playerScript.inOrbit)) {   
					if (obsticalList.Count > 1) {
						minObsticalYCoodinate = obsticalList [1].transform.position.y;
						Destroy (obsticalList [0]);
						obsticalList.RemoveAt (0);
					}
				}
				if (minCollectableYCoodinate + 15 < playerDistance) { 
					if ((collectableList.Count > 1)&&(minCollectableYCoodinate > 0)) {
						minCollectableYCoodinate = collectableList [1].transform.position.y;
						Destroy(collectableList[0]);
						collectableList.RemoveAt (0);
					}
				}
				if (((int)playerDistance+1) % 200 == 0) {
					EnterAstroidBelt (playerDistance);
				}
			}
			if (inAstroidBelt) {
				if (playerDistance > astroidBeltStartingDistance + astroidBeltSize) {
					EndAstroidBelt ();
				}
			}
	/*		if (inBlackHole) {   // In Black Hole
				
			}-*/
		}
	}


	void PlanetOrbitFTU(string ftuText){
		planetFtuShown = true;
		ftuePanel.SetActive (true);
		ftueText.text = ftuText;
		playerScript.playerPaused = true;
		StartCoroutine (TouchDetectFTUE ());
	}

	public void CreateObstical(int times = 1){
		for(int i = 0 ; i < times ; i++){
			float x;
			do{
				x = Random.Range (minX, maxX);
			}while(Mathf.Abs(x-lastObsticalXCood)<2);
			lastObsticalXCood = x;
			float y = Random.Range(minY,maxY);
			y = y + obsticalList [obsticalList.Count - 1].gameObject.transform.position.y;
			Vector3 pos = new Vector3(x,y,0);
			GameObject newObstical;
			int ind;
			do{
				ind = Random.Range(0,obstical.Count);	
			}while(ind == lastCreatedPlanetIndex);
			newObstical = Instantiate(obstical[ind],pos,Quaternion.identity)as GameObject;
			lastCreatedPlanetIndex = ind;
			CreateCollectables (x, y);
			maxObsticalYCoodinate = y;
			obsticalList.Add (newObstical);
		}
	}

	void CreateCollectables(float xco ,float yCoodinate){
		int num = Random.Range (1, collectableRandomSeed);
		if (num == 1) {
			float x;
			if (Mathf.Abs (xco) > 1.5f) {
				if (xco > 0) {
					x = 2.25f;
				}
				else {
					x = -2.25f;
				}

			}
			else {
				x = 0;
			}
			yCoodinate = yCoodinate + 2.5f;
			if (!firstCollectableCreated) {
				minCollectableYCoodinate = yCoodinate;
				firstCollectableCreated = true;
			}
			Vector3 pos = new Vector3(x,yCoodinate,0);
			GameObject coll;
			num = Random.Range (1, 15);
			if (num == 1) {
				coll = Instantiate (blackHole, pos, Quaternion.identity)as GameObject;
				Vector3 rot = coll.transform.eulerAngles;
				rot.x = 60;
				coll.transform.eulerAngles = rot;
			}
			else {
				coll = Instantiate (collectables[Random.Range(0,collectables.Count)], pos, Quaternion.identity)as GameObject;
			}
			collectableList.Add (coll);
		}
	}


	public void EnterAstroidBelt(float playerDistance){
		inAstroidBelt = true;
		cameraScript.player = player;
		cameraScript.inOrbit = false;
		playerScript.enteredAstroidBelt = true;
		astroidBeltStartingDistance = playerDistance;
		inSpace = false;
		for (int i = 0; i < obsticalList.Count; i++) {
			Destroy (obsticalList [i]);
		}
		obsticalList.Clear ();
		for (int i = 0; i < collectableList.Count; i++) {
			Destroy (collectableList [i]);
		}
		collectableList.Clear ();
		StartCoroutine (CreateAstroidBelt ());
		msgTextBox.SetActive (false);
		msgText.text = "ENTEING ASTROID BELT";
		msgTextBox.SetActive (true);
		StartCoroutine (CountDown (3));
	}

	IEnumerator CountDown(int timer){
		countdownTextBox.SetActive (true);
		for (int i = timer; i > 0; i--) {
			countdownText.text = i.ToString ();
			yield return new WaitForSeconds (1);
		}
		countdownTextBox.SetActive (false);
		msgTextBox.SetActive (false);
		msgText.text = "TILT TO STEER";
		msgTextBox.SetActive (true);
		playerScript.accControlEnabled = true;
		yield return new WaitForSeconds (1f);
		msgTextBox.SetActive (false);
	}

	IEnumerator CreateAstroidBelt(){
		float playerY = player.transform.position.y;
		int x = 0,lastX = 0;
		for(int i = 0;i<astroidBeltSize;i+=2){
			if (i < 40) {
				continue;
			}
			else {
				if (i % 10 != 0) {
					Vector3 pos;
					pos.y = (int)playerY + i;
					pos.z = 0;
					pos.x = lastX + Random.Range(-0.5f,0.5f);
					GameObject obs;
					obs = Instantiate (astroids[Random.Range(0,astroids.Count)], pos, Quaternion.identity)as GameObject;
					obsticalList.Add (obs);
				}
				else {
					do{
						x= (int)Random.Range (minX, maxX);
					}while(x == lastX);
					lastX = x;
				}
			}
		}
		acclorometerIcon.SetActive (true);
		yield return new WaitForSeconds (3f);
		acclorometerIcon.SetActive (false);
	}

	public void EndAstroidBelt(){
		inAstroidBelt = false;
		inSpace = true;
		for (int i = 0; i < obsticalList.Count; i++) {
			Destroy (obsticalList [i]);
		}
		obsticalList.Clear ();
		RecreateObsticals (player.transform.position.y + 10);
		StartCoroutine (ExitAstroidBelt ());
	}

	IEnumerator ExitAstroidBelt(){
		msgTextBox.SetActive (false);
		msgText.text = "ASTROID BELT ENDED";
		msgTextBox.SetActive (true);
		yield return new WaitForSeconds (0.75f);
		playerScript.accControlEnabled = false;
		playerScript.enteredAstroidBelt = false;
		msgTextBox.SetActive (false);
		msgText.text = "MANUAL STEERING OFF";
		msgTextBox.SetActive (true);
		yield return new WaitForSeconds (1);
		msgTextBox.SetActive (false);
	}

	public void EnterBlackHole(){
		inBlackHole = true;
		inSpace = false;
		cameraScript.inOrbit = false;
		playerScript.enabled = false;
		for (int i = 0; i < obsticalList.Count; i++) {
			Destroy (obsticalList [i],0.5f);
		}
		obsticalList.Clear ();
		for (int i = 0; i < collectableList.Count; i++) {
			Destroy (collectableList [i],0.5f);
		}
		collectableList.Clear ();
		canvasAnim.SetTrigger ("EnterBlackHole");
		StartCoroutine (EnterBlackHoleRegion());
	}
	IEnumerator Warp(){
		entryParticles.SetActive (true);
		yield return new WaitForSeconds (1f);
		entryParticles.SetActive (false);
	}

	IEnumerator EnterBlackHoleRegion(){
		StartCoroutine (Warp());
		cameraScript.player = player;
		msgTextBox.SetActive (false);
		msgText.text = "ENTERED BLACK HOLE ! TILT TO STEER";
		msgTextBox.SetActive (true);
		yield return new WaitForSeconds (0.9f);
	//	CreateBlackHoleCoins ();
		float playerY = player.transform.position.y;
		Vector3 post;
		post.y = (int)playerY;
		post.z = 0;
		post.x = 0;
		cn = Instantiate (blackHoleCollectables, post, Quaternion.identity)as GameObject;

		background.GetComponent<MeshRenderer> ().material = blackWorldMaterial;
		inSpace = true;
		playerScript.enabled = true;
		playerScript.EnterBlackHoleRegion ();
		Vector3 pos = new Vector3 (0, 0, 0);
		pos.y = player.transform.position.y + blackHoleWorldSize;
		GameObject wh;
		wh = Instantiate (whiteHole, pos, Quaternion.identity)as GameObject;
		Vector3 rot = wh.transform.eulerAngles;
		rot.x = 50;
		wh.transform.eulerAngles = rot;
		acclorometerIcon.SetActive (true);
		yield return new WaitForSeconds (1.5f);
		msgTextBox.SetActive (false);
		yield return new WaitForSeconds (1f);
		acclorometerIcon.SetActive (false);
	}
		
	public void EnterWhiteHole(){
		inSpace = false;
		inBlackHole = false;
		playerScript.enabled = false;
		cameraScript.player = player;
		acclorometerIcon.SetActive (false);
	/*	for (int i = 0; i < collectableList.Count; i++) {
			Destroy (collectableList [i],0.5f);
		}*/
		Destroy (cn);
	//	collectableList.Clear ();
		canvasAnim.SetTrigger ("EnterBlackHole");
		StartCoroutine (EnterWhiteHoleRegion());
	}

	IEnumerator EnterWhiteHoleRegion(){
		msgTextBox.SetActive (false);
		entryParticles.SetActive (true);
		msgText.text = "EXITED BLACK HOLE";
		msgTextBox.SetActive (true);
		yield return new WaitForSeconds (0.9f);
		entryParticles.SetActive (false);
		RecreateObsticals ();
		background.GetComponent<MeshRenderer> ().material = whiteWorldMaterial;
		inSpace = true;
		playerScript.enabled = true;
		playerScript.EnterWhiteHoleRegion ();
		yield return new WaitForSeconds (1f);
		msgTextBox.SetActive (false);
	}

/*	void CreateBlackHoleCoins(){
		float playerY = player.transform.position.y;
		for(int i = 0;i<blackHoleWorldSize;i++){
			if ((i < 15) || (i > blackHoleWorldSize - 15)) {
				continue;
			}
			else {
				Vector3 pos;
				pos.y = (int)playerY + i;
				pos.z = 0;
				pos.x = (int)Random.Range (-3.1f, 3.1f);
				GameObject cn;
				cn = Instantiate (collectables[Random.Range(0,(collectables.Count-1))], pos, Quaternion.identity)as GameObject;
				collectableList.Add (cn);
			}
		}
	}*/

	void RecreateObsticals(float position = -100){
		Vector3 pos;
		if (position < 0) {
			pos = new Vector3 (Random.Range (minX, maxX), obsticalStartDistance + player.transform.position.y, 0);

		}
		else {
			pos = new Vector3 (Random.Range (minX, maxX), obsticalStartDistance + position, 0);
		}
		GameObject firstObstical;
		firstObstical = Instantiate(obstical[Random.Range(0,obstical.Count)],pos,Quaternion.identity)as GameObject;
		minObsticalYCoodinate = obsticalStartDistance;
		maxObsticalYCoodinate = obsticalStartDistance;
		obsticalList.Add (firstObstical);
		firstCollectableCreated = false;
		CreateObstical (5); 
	}


	public void GameOver(){
		cameraScript.ShakeCamera ();
		cameraScript.enabled = false;
		playerScript.enabled = false;
		canvasAnim.SetTrigger ("GameOver");
	}
		
	public void ScoreIncrease(int scoreIncrease){
		score = score + scoreIncrease;
		scoreText.text = score.ToString ();
		if ((score > 0) && (score % appriciateScore == 0)) {
			appriciationText.text = appriciationMsg [Random.Range ((int)0, (int)appriciationMsg.Count)];
			canvasAnim.SetTrigger ("Appriciate");
		}
	}
		
	public void RestartBtn(){
		Time.timeScale = 1;
		SceneManager.LoadScene ("Game_Scene");
	}

	public void PauseBtn(){
		StartCoroutine (Pause ());
	}
	IEnumerator Pause(){
		canvasAnim.SetTrigger ("Pause");
		yield return new WaitForSeconds (0.04f);
		Time.timeScale = 0;
	}

	public void ResumeBtn(){
		StartCoroutine (Resume ());
	}
	IEnumerator Resume(){
		yield return null;
		Time.timeScale = 1;
		canvasAnim.SetTrigger ("Resume");
	}

	public void Sound(){
		if (PlayerPrefs.GetInt ("Sound") == 0) {
			PlayerPrefs.SetInt ("Sound", 1);
			sound.image.sprite = soundOn;
			aud.Play ();    // Play Background Music
		}
		else {
			PlayerPrefs.SetInt ("Sound", 0);
			sound.image.sprite = soundOff;
			aud.Stop ();    // Stop background Music
		}
	}

	IEnumerator TouchDetectFTUE ()
	{
		while (true) {
			if (Input.GetMouseButton (0)) {
				playerScript.ForceConnect ();
				playerScript.playerPaused = false;
				break;
			}
			yield return null;
		}
		ftuePanel.SetActive (false);
		StartCoroutine (ReleseFTUE ());
	}

	IEnumerator ReleseFTUE(){
		yield return new WaitForSeconds (2.5f);
		if (playerScript.inOrbit) {
			ftuePanel.SetActive (true);
			ftueText.text = "Release your hold to follow trajectory";
			while (true) {
				if (Input.GetMouseButtonUp (0)) {
					ftuePanel.SetActive (false);
					break;
				}
				yield return null;
			}
		}
	}
}