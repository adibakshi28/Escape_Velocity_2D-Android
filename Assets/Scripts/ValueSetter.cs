using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ValueSetter : MonoBehaviour {

	public InputField playerLinerSpeedField,maxRotSpdField,obsAttDistField,scoreUpdateDistanceField,scoreUpdateField,collScoreIncrementField,collRandomSeedField,obsStartDistanceField,obsMinYField,obsMaxYField,cameraFollowSpeedField,minObsticalSizeField,maxObsticalSizeField,massConstantField,distanceDegreeField,acclerationField;
	public Toggle panCamera;
	[HideInInspector]
	public float playerLinerSpeed,maxRotSpd,obsAttDist,scoreUpdateDistance,obsStartDistance,obsMinY,obsMaxY,cameraFollowSpeed,minObsSize,maxObsSize,massConstant,distanceDegree,accleration;
	[HideInInspector]
	public int scoreUpdate,collScoreIncrement,collRandomSeed;
	[HideInInspector]
	public bool panCam;

	void Start () {
		SetVariables ();
		UpdateInputField ();
		DontDestroyOnLoad(this.gameObject);
	}


	void SetVariables(){
		playerLinerSpeed = PlayerPrefs.GetFloat ("LineraSpeed");
		maxRotSpd = PlayerPrefs.GetFloat ("MaxRotSpeed");
		obsAttDist = PlayerPrefs.GetFloat ("ObsAttThrDist");
		scoreUpdateDistance = PlayerPrefs.GetFloat ("ScoreUpdateDist");
		obsStartDistance = PlayerPrefs.GetFloat ("ObsStartDist");
		obsMinY = PlayerPrefs.GetFloat ("ObsMinY");
		obsMaxY = PlayerPrefs.GetFloat ("ObsMaxY");
		cameraFollowSpeed = PlayerPrefs.GetFloat ("CamFollowSpeed");
		minObsSize = PlayerPrefs.GetFloat ("MinObsSize");
		maxObsSize = PlayerPrefs.GetFloat ("MaxObsSize");
		massConstant = PlayerPrefs.GetFloat ("MassConstant");
		distanceDegree = PlayerPrefs.GetFloat ("DistanceDegree");
		accleration = PlayerPrefs.GetFloat ("Acc");
		scoreUpdate = PlayerPrefs.GetInt ("ScoreUpdate");
		collScoreIncrement = PlayerPrefs.GetInt ("CollScoreIncrement");
		collRandomSeed = PlayerPrefs.GetInt ("CollRandomSeed");
		if (PlayerPrefs.GetInt ("CamPan") == 1) {
			panCam = true;
		}
		else {
			panCam = false;
		}
	}


	void UpdateInputField(){
		playerLinerSpeedField.text = playerLinerSpeed.ToString ();
		maxRotSpdField.text = maxRotSpd.ToString ();
		obsAttDistField.text = obsAttDist.ToString ();
		scoreUpdateDistanceField.text = scoreUpdateDistance.ToString ();
		obsStartDistanceField.text = obsStartDistance.ToString ();
		obsMinYField.text = obsMinY.ToString ();
		obsMaxYField.text = obsMaxY.ToString ();
		cameraFollowSpeedField.text = cameraFollowSpeed.ToString ();
		minObsticalSizeField.text = minObsSize.ToString ();
		maxObsticalSizeField.text = maxObsSize.ToString ();
		massConstantField.text = massConstant.ToString ();
		distanceDegreeField.text = distanceDegree.ToString ();
		acclerationField.text = accleration.ToString ();
		scoreUpdateField.text = scoreUpdate.ToString ();
		collScoreIncrementField.text = collScoreIncrement.ToString ();
		collRandomSeedField.text = collRandomSeed.ToString ();
		panCamera.isOn = panCam;
	}

	public void StartGame(){
		PlayerPrefs.SetFloat ("LineraSpeed",float.Parse(playerLinerSpeedField.text));
		PlayerPrefs.SetFloat ("MaxRotSpeed",float.Parse(maxRotSpdField.text));
		PlayerPrefs.SetFloat ("ObsAttThrDist",float.Parse(obsAttDistField.text));
		PlayerPrefs.SetFloat ("ScoreUpdateDist",float.Parse(scoreUpdateDistanceField.text));
		PlayerPrefs.SetFloat ("ObsStartDist",float.Parse(obsStartDistanceField.text));
		PlayerPrefs.SetFloat ("ObsMinY",float.Parse(obsMinYField.text));
		PlayerPrefs.SetFloat ("ObsMaxY",float.Parse(obsMaxYField.text));
		PlayerPrefs.SetFloat ("CamFollowSpeed",float.Parse(cameraFollowSpeedField.text));
		PlayerPrefs.SetFloat ("MinObsSize",float.Parse(minObsticalSizeField.text));
		PlayerPrefs.SetFloat ("MaxObsSize", float.Parse (maxObsticalSizeField.text));
		PlayerPrefs.SetFloat ("MassConstant",float.Parse(massConstantField.text));
		PlayerPrefs.SetFloat ("DistanceDegree",float.Parse(distanceDegreeField.text));
		PlayerPrefs.SetFloat ("Acc",float.Parse(acclerationField.text));
		PlayerPrefs.SetInt ("ScoreUpdate",int.Parse(scoreUpdateField.text));
		PlayerPrefs.SetInt ("CollScoreIncrement",int.Parse(collScoreIncrementField.text));
		PlayerPrefs.SetInt ("CollRandomSeed",int.Parse(collRandomSeedField.text));
		if (panCamera.isOn) {
			PlayerPrefs.SetInt ("CamPan", 1);
		}
		else{
			PlayerPrefs.SetInt ("CamPan", 0);
		}
		SetVariables ();
		SceneManager.LoadScene ("Game_Scene");
	}

	public void ResetBtn(){
		playerLinerSpeedField.text = "9";
		maxRotSpdField.text = "1250";
		obsAttDistField.text = "6";
		scoreUpdateDistanceField.text = "7";
		scoreUpdateField.text = "1";
		collScoreIncrementField.text = "2";
		collRandomSeedField.text = "3";
		obsStartDistanceField.text = "10";
		obsMinYField.text = "4";
		obsMaxYField.text = "7";
		cameraFollowSpeedField.text = "3";
		minObsticalSizeField.text = "1";
		maxObsticalSizeField.text = "1.3";
		massConstantField.text = "0.1";
		distanceDegreeField.text = "1.8";
		acclerationField.text = "0";
		panCamera.isOn = true;
	}
}
