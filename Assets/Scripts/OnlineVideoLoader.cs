using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;
 
public class OnlineVideoLoader : MonoBehaviour
{
     
    public VideoPlayer videoPlayer;
    public string videoUrl = "yourvideourl";
    public TMP_InputField videoInput;
     
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.url = videoUrl;
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.EnableAudioTrack (0, true);
        videoPlayer.Prepare ();
    }

    public void OnPlayButtonClick(){
        videoPlayer.url = videoInput.text;
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.EnableAudioTrack (0, true);
        videoPlayer.Prepare ();
        videoPlayer.Play();
    }
 
    // Update is called once per frame
    void Update()
    {
         
    }
}