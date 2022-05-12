using UnityEngine;
using UnityEngine.UI; //for accessing Sliders and Dropdown
using System.Collections.Generic; // So we can use List<>
using MainDefinitions;
using TMPro;
using System;
using System.Collections;
using Tamarin.Common;
// using MarksAssets.VideoPlayerWebGL;
// using LightShaft.Scripts;

[RequireComponent(typeof(AudioSource))]
public class GameSessionManager : MonoBehaviour
{
	// [SerializeField] YoutubePlayer youtubePlayer;      //UNREMOVED FOR DEBUG (youtubePlayer)

	//private string playerName;
	//private string playerEmail;
	private float elapsedTime;
	private float maxVolume;
	private float score;
	// private VideoPlayerWebGL videoPlayerWebGL;



    async private void Start()
    {
		// videoPlayerWebGL = UIManager.Instance.videoPlayerWebGL;
		//playerName = Definitions.DEFAULT_NAME;
		//playerEmail = null;
		//youtubePlayer.transform.parent.gameObject.SetActive(true);      //UNREMOVED FOR DEBUG (youtubePlayer)
		//await Waiter.Until(() => GameManager.Instance.firebase.ready == true);
		//GameManager.Instance.LoadClipLink();
		//youtubePlayer.PreLoadVideo(GameManager.Instance.clipLink);

	}

	// public void OnClickPlay(){
    //     videoPlayerWebGL.Source(MarksAssets.VideoPlayerWebGL.VideoPlayerWebGL.srcs.External, GameManager.Instance.clipURL);
    //     videoPlayerWebGL.CreateVideoWrapper();
    //     videoPlayerWebGL.Play();
    // }


	public float getScore()
	{
		return (float)score;
	}

	public float getMaxVolume()
	{
		return (float)maxVolume;
	}

	public float getElapsedTime()
	{
		return (float)elapsedTime / 1000;
	}

	public void UpdateStats(float score, float maxVolume, float elapsedTime)
    {
		this.score = score;
		this.maxVolume = maxVolume;
		this.elapsedTime = elapsedTime;
		GameManager.Instance.AddScoreToScoreboard((int)score);
	}

	

}