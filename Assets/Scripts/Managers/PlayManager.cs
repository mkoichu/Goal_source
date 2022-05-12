// using OpenCVForUnitySample;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
//using Assets.Scripts.General;
using MainDefinitions;
using UnityEngine.Video;
//using LightShaft.Scripts;
using MarksAssets.VideoPlayerWebGL;


public class PlayManager : MonoBehaviour
{
	[SerializeField] private GameObject comentatorPanel;      //UNREMOVED FOR DEBUG (COMENTATOR PANEL)
	[SerializeField] GameObject getReadyPanel;


	[SerializeField] private GameSessionManager gameSessionManager;
	[SerializeField] private VolumeMeter volumeMeter;
	[SerializeField] private TextMeshProUGUI uiScore;
	[SerializeField] private TextMeshProUGUI uiTimer;
	[SerializeField] private GameObject celebrationPanel;
	//[SerializeField] private VideoRecorder videoRecorder;    //REMOVED FOR DEBUG

	[SerializeField] private GameObject countDown;
	// [SerializeField] private YoutubeSimplified youtubeSimplified;      //UNREMOVED FOR DEBUG (youtubePlayer)
	// [SerializeField] private VideoPlayer videoPlayer;      //UNREMOVED FOR DEBUG (youtubePlayer)
	// [SerializeField] private YoutubePlayer youtubeSettings;      //UNREMOVED FOR DEBUG (youtubePlayer)
	// [SerializeField] private AudioSource videoSound;

	// private VideoPlayerWebGL videoPlayerWebGL;


	private float VOLLUME_THRESHOLD_0_START = 40f; //begin to record if above this threshold
	private float VOLLUME_THRESHOLD_0_END = 25f; //end recording if below this threshold
	private float TIME_THRESHOLD_1 = 3;
	private float TIME_THRESHOLD_2 = 6;
	private float TIME_THRESHOLD_3 = 9;
	private float TIME_THRESHOLD_4 = 12;

	private float VOL_THRESHOLD_1 = 30; //begin to record
	private float VOL_THRESHOLD_2 = 40;
	private float VOL_THRESHOLD_3 = 50;


	private double elapsedTime = 0;
	private float maxVolume = 0;
	private double score = 0;
	//private double finalScore = 0;
	private double totalVolumeScore = 0;

	private bool begin = false;
	private bool end = false;
	//private bool stop = false;
	private bool esc_pressed = false;

	private float startTime;
	private float currTime;

	/*private ARVolumeEffectType currSteamType = ARVolumeEffectType.None;
	private ARTimeEffectType currTimeEffectType = ARTimeEffectType.None;*/      //REMOVED FOR DEBUG (AR + unity+)
	private int safetyDelay = 5;

	private bool played_countdown = false;
	float avgVol;
	float db = 0;
	

	private void Start()
    {
		//Bus.OnGameOver.Subscribe(this, () => StartCoroutine(GameOver(Definitions.GAME_OVER_DELAY)));
		// videoPlayerWebGL = UIManager.Instance.videoPlayerWebGL;
	}


	public bool BeganShouting()
    {
		return begin;
    }



	private void OnEnable()
	{
		InitVariables();
		// UIManager.Instance.videoPlayerWebGL.Volume(1f);
		// AudioManager.Instance.MicOFF();
		// AudioManager.Instance.StartRecordingAudio();   //TODO: remove after adding video and goal time
		

		// youtubeSettings.Play();      //UNREMOVED FOR DEBUG (youtubePlayer)
		// youtubeSimplified.Play();      //UNREMOVED FOR DEBUG (youtubePlayer)
		//videoRecorder.OnRecButtonClick(); //Remove comments   //REMOVED FOR DEBUG
		//videoRecorder.StartRecording(GameManager.Instance.video_save_path);   //REMOVED FOR DEBUG
	}

	private void InitVariables()
    {
		begin = false; //TODO: uncomment
		// begin = true;
		//stop = false;
		end = false;
		esc_pressed = false;

		db = 0;
		score = 0;
		//finalScore = 0;
		totalVolumeScore = 0;
		elapsedTime = 0;
		maxVolume = 0;
		uiTimer.text = "00:00";
		uiScore.text = "0";
		safetyDelay = Definitions.SAFETY_DELAY_FRAMES;

		// youtubeSimplified.url = GameManager.Instance.clipLink;      //UNREMOVED FOR DEBUG (youtubePlayer)
		played_countdown = false;

		/*currSteamType = ARVolumeEffectType.None;
		currTimeEffectType = ARTimeEffectType.None;*/     //REMOVED FOR DEBUG (AR + unity+)
	}

	void OnDisable()
	{
		 
		// youtubeSettings.EnableThumbnailObject();      //UNREMOVED FOR DEBUG (youtubePlayer)
		// videoSound.volume = 1;      //UNREMOVED FOR DEBUG (youtubePlayer)
		//videoPlayer.Stop();      //UNREMOVED FOR DEBUG (youtubePlayer)
		// played_countdown = false;  //UNREMOVED FOR DEBUG (youtubePlayer)
		// countDown.SetActive(false);  //UNREMOVED FOR DEBUG (youtubePlayer)
		// AudioManager.Instance.MicOFF();  //UNREMOVED FOR DEBUG (youtubePlayer)
	}



	private void Update()
	{
		var videoSound = UIManager.Instance.videoPlayerWebGL.Volume();
		var videoTime = UIManager.Instance.videoPlayerWebGL.Time();
		//Turn on mic 1 second before the goal:
		Debug.Log($"PLAY 1: videoTime: {videoTime} timeOfGoal: {GameManager.Instance.timeOfGoal}  timeOfGoal-TURN_ON_MIC: {GameManager.Instance.timeOfGoal - Definitions.TURN_ON_MIC_BEFORE_GOAL}   .mic_is_on: {AudioManager.Instance.mic_is_on}");
		if (videoTime > GameManager.Instance.timeOfGoal - Definitions.TURN_ON_MIC_BEFORE_GOAL && AudioManager.Instance.mic_is_on == false)
		{
			Debug.Log("PLAY 2");
			AudioManager.Instance.StartRecordingAudio();
		}
		Debug.Log($"PLAY 3:  videoTime: {videoTime}  videoSound: {videoSound}");
		//Gradually turn down the volume of the soccer game footage:
		if (videoTime > GameManager.Instance.timeOfGoal && videoSound > 0)
		{
			Debug.Log("PLAY 4");
			UIManager.Instance.videoPlayerWebGL.Volume(videoSound - Definitions.VOLUME_DECREASE_RATE);
		}
		Debug.Log($"PLAY 5:  timeOfGoal-ANIMATION_LENGTH: {GameManager.Instance.timeOfGoal - Definitions.COUNTDOWN_ANIMATION_LENGTH}   played_countdown: {played_countdown}");
		//Play countdown once 5 seconds before the goal:
		if (videoTime > GameManager.Instance.timeOfGoal - Definitions.COUNTDOWN_ANIMATION_LENGTH && played_countdown == false)
		{
			Debug.Log("PLAY 6");
			Debug.Log("COUNT DOWN BEGINS");
			played_countdown = true;
			countDown.SetActive(true);
		}
		Debug.Log($"PLAY 7: begin: {begin}");
		//End game if player didn't begin to shout within 3 second after the goal:
		if (videoTime > GameManager.Instance.timeOfGoal + Definitions.END_GAME_IF_SILENT_AFTER && begin == false)
		{
			Debug.Log("PLAY 8");
			Debug.Log("PlayManager game over because not shout");
			StartCoroutine(GameOver(0));
			// Bus.OnGameOver.Publish();
		}

		db = AudioManager.Instance.GetMicVolume();
		Debug.Log($"PLAY 9:  db: {db}");
		if (Input.GetKeyDown(KeyCode.Escape) && esc_pressed == false)
		{ 
			Debug.Log("PLAY 10");
			esc_pressed = true;
			StartCoroutine(GameOver(Definitions.GAME_OVER_DELAY));
			//UIManager.Instance.OnPlayEscape();
			//Bus.OnGameOver.Publish();
		}
		Debug.Log("PLAY 11");
        if (Input.GetKey(KeyCode.Q))
        {
            db = 15;
        }
        if (Input.GetKey(KeyCode.W))
        {
            db = 35;
        }
        if (Input.GetKey(KeyCode.E))
        {
            db = 45;
        }
        if (Input.GetKey(KeyCode.R))
        {
            db = 55;
        }
        // Debug.Log("DB: " + db);

        if (end == false)
		{
            if (begin == false)
            {
                if (db >= VOLLUME_THRESHOLD_0_START)
                {
                    if (safetyDelay < 0)
                    {
                        begin = true;
                        score = 0;
                        totalVolumeScore = 0;
                        elapsedTime = 0;
                        startTime = Time.time;
                    }
                    safetyDelay--;
                }
                else
                {
                    if (safetyDelay < Definitions.SAFETY_DELAY_FRAMES)
                    {
                        safetyDelay++;
                    }
                }
            }


            if (begin == true)
			{
				volumeMeter.SetVolume(db);
				if (db > maxVolume)
				{
					maxVolume = db;
				}

				if (db >= VOLLUME_THRESHOLD_0_END)
				{
					// UIManager.Instance.MaximizeComentatorPanel();

					totalVolumeScore += db;
					currTime = Time.time - startTime;
					score = currTime * 1000; // db * (currTime);
					UpdateTimer(currTime);


					//Threshold events:
					if (currTime > TIME_THRESHOLD_1)
					{
						//Volume threshold events:
						// if (db < VOL_THRESHOLD_1 && currSteamType != ARVolumeEffectType.None)
						// {
						// 	currSteamType = ARVolumeEffectType.None;
						// 	Bus.StartARVolumeEffect.Publish(ARVolumeEffectType.None);
						// }
						// if (VOL_THRESHOLD_1 <= db && db < VOL_THRESHOLD_2 && currSteamType != ARVolumeEffectType.SmallSteam)
						// {
						// 	currSteamType = ARVolumeEffectType.SmallSteam;
						// 	Bus.StartARVolumeEffect.Publish(ARVolumeEffectType.SmallSteam);
						// }
						// if (VOL_THRESHOLD_2 <= db && db < VOL_THRESHOLD_3 && currSteamType != ARVolumeEffectType.MediumSteam)
						// {
						// 	currSteamType = ARVolumeEffectType.MediumSteam;
						// 	Bus.StartARVolumeEffect.Publish(ARVolumeEffectType.MediumSteam);
						// }
						// if (db > VOL_THRESHOLD_3 && currSteamType != ARVolumeEffectType.LargeSteam)
						// {
						// 	currSteamType = ARVolumeEffectType.LargeSteam;
						// 	Bus.StartARVolumeEffect.Publish(ARVolumeEffectType.LargeSteam);
						// }
					}

					// Time threshold events:
					if (currTime > TIME_THRESHOLD_1)// && currTimeEffectType == ARTimeEffectType.None)
                    {
						UIManager.Instance.ActivateTimeElementByIndex(0);
						// currTimeEffectType = ARTimeEffectType.VeinPop;
					}						
					if (currTime > TIME_THRESHOLD_2)// && currTimeEffectType == ARTimeEffectType.VeinPop)
					{
						UIManager.Instance.ActivateTimeElementByIndex(1);
						// currTimeEffectType = ARTimeEffectType.FireEyes;
					}
					if (currTime > TIME_THRESHOLD_3)// && currTimeEffectType == ARTimeEffectType.FireEyes)
					{
						UIManager.Instance.ActivateTimeElementByIndex(2);
						// currTimeEffectType = ARTimeEffectType.Lightning;
					}
					if (currTime > TIME_THRESHOLD_4)// && currTimeEffectType == ARTimeEffectType.Lightning)
					{
						UIManager.Instance.ActivateTimeElementByIndex(3);
						// currTimeEffectType = ARTimeEffectType.SuperSaiyan;
					}
				}

				//Game Over:
				if (db < VOLLUME_THRESHOLD_0_END)
				{
					Debug.Log($"PlayManager: game over because db < VOLLUME_THRESHOLD_0_END");
					StartCoroutine(GameOver(Definitions.GAME_OVER_DELAY));
					//Bus.OnGameOver.Publish();
				}
			}

			uiScore.text = ((int)score).ToString();
		}

		

	}

	public IEnumerator GameOver(int delay)
	{
		elapsedTime = (startTime > 0) ? (Time.time - startTime) * 1000 : 0;
		uiScore.text = ((int)score).ToString();
		
		gameSessionManager.UpdateStats((float)score, (float)maxVolume, (float)elapsedTime);   //UNREMOVED FOR DEBUG (298 temp)
		yield return new WaitForSeconds(delay);
		//videoRecorder.StopRecording();   //REMOVED FOR DEBUG
		UIManager.Instance.OnGameOver();
	}

	private void UpdateTimer(float currTime)
    {
		string minutes = ((int)(currTime / 60)).ToString();
		string seconds = ((int)currTime % 60).ToString("f0");
		int miliseconds = ((int)(currTime * 100) % 100);

		if (miliseconds < 10)
		{
			uiTimer.text = minutes + ":" + seconds + ":0" + miliseconds.ToString();
		}
		else
		{
			uiTimer.text = minutes + ":" + seconds + ":" + miliseconds.ToString();
		}
	}



}
