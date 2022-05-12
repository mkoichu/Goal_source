
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using FrostweepGames.Plugins.Native;
using Tamarin.Common;
using MainDefinitions;
using System;
// namespace UnityWebGLMicrophone
// {
public class AudioManager : MonoBehaviour
{
    //Public AudioSource variables:
    [SerializeField] private AudioSource objectButttonSound;
    [SerializeField] private AudioSource menuButtonSound;
    [SerializeField] private AudioSource cashRegisterSound;
    [SerializeField] private AudioMixer soundsAudioMixer;

    //Microphone variables:
    [SerializeField] private AudioSource micAudioSource;
    public bool mic_is_on = false;
    private string microphone;
    private List<string> options = new List<string>();
    private FFTWindow fftWindow;
    private int numberOfSamples = 8192;
    private float minThreshold = 0;
    //private float frequency = 0.0f;

    //General variables:
    public static bool sound_on = true;
    private float DEFAULT_VOLUME = 0f;
    private float NO_VOLUME = -80f;

        public static AudioManager Instance { get; private set; }


    //new variables:    
    public float averageVoiceLevel = 0f;

    public double voiceDetectionTreshold = 0.02d;
    private AudioClip _workingClip;
    
    private int frequency = 44100;

    private int recordingTime = 120;

    private string selectedMicrophone;




    void Awake(){
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Debug.LogError("There is more than one instance!");
            Destroy(gameObject);
        }
    }

     private void Start()
    {
        //StartCoroutine(InitMicrophone());
    }

    public void InitMic(System.Action onInitMicOver)
    {
        Debug.Log("InitMic()");
        StartCoroutine(InitMicrophone(onInitMicOver));
    }

    private IEnumerator InitMicrophone(System.Action onInitMicOver)
    {
        Debug.Log("InitMicrophone()   FLAG 1");
        // yield return Waiter.Until(() => GameFlowStates.state >= PossibleGameFlowStates.MicrophoneLoaded);

        CustomMicrophone.RecordStreamDataEvent += RecordStreamDataEventHandler;
        CustomMicrophone.PermissionStateChangedEvent += PermissionStateChangedEventHandler;
        CustomMicrophone.RecordStartedEvent += RecordStartedEventHandler;
        CustomMicrophone.RecordEndedEvent += RecordEndedEventHandler;
         Debug.Log("InitMicrophone()   FLAG 2");
        selectedMicrophone = string.Empty;
        while (!CustomMicrophone.HasConnectedMicrophoneDevices())
        {
            Debug.Log("InitMicrophone()   FLAG 2.111");
            RefreshMicrophoneDevicesButtonOnclickHandler();
            yield return new WaitForSeconds(0.2f);
        }
         Debug.Log("InitMicrophone()   FLAG 3");
        RefreshMicrophoneDevicesButtonOnclickHandler();
         Debug.Log("InitMicrophone()   FLAG 4");
        yield return new WaitForSeconds(0.2f);
         Debug.Log("InitMicrophone()   FLAG 5");
        RequestPermission();
         Debug.Log("InitMicrophone()   FLAG 6");
        yield return null;
         Debug.Log("InitMicrophone()   FLAG 7");
        Debug.Log($"MICROPHONE: {selectedMicrophone}  permission: '{ (CustomMicrophone.HasMicrophonePermission() ? "<color=green>granted</color>" : "<color=red>denined</color>") }'");

        yield return null;
         Debug.Log("InitMicrophone()   FLAG 8");
        if(CustomMicrophone.HasMicrophonePermission())
        {
             Debug.Log("InitMicrophone()   FLAG 9");
            onInitMicOver?.Invoke();
        }
    }


    private void OnDestroy()
    {
        CustomMicrophone.RecordStreamDataEvent -= RecordStreamDataEventHandler;
        CustomMicrophone.PermissionStateChangedEvent -= PermissionStateChangedEventHandler;
        CustomMicrophone.RecordStartedEvent -= RecordStartedEventHandler;
        CustomMicrophone.RecordEndedEvent -= RecordEndedEventHandler;
    }

    public void PlayObjectButtonSound()
    {
        objectButttonSound.Play();
    }

    public void PlayMenuButtonSound()
    {
        menuButtonSound.Play();
        //StartCoroutine(PlayMenuButtonCoroutine());
    }

    /*IEnumerator PlayMenuButtonCoroutine()
    {
        menuButtonSound.Play();
        yield return null;
    }*/

    public void PlayCashRegisterSound()
    {
        cashRegisterSound.Play();
    }

    /*
     * 
     * DateTime now = DateTime.Now;
        Debug.Log("================ now = " + now.ToString() + "===========");
        TimeSpan sec2 = new TimeSpan(0, 0, 2);
        await Waiter.Until(() => GameFlowStates.state >= PossibleGameFlowStates.MicrophoneLoaded);
        await Waiter.Until(() => DateTime.Now > now + sec2);

        Debug.Log("================ now = " + now.ToString() + "===========");

        Debug.Log("===================== AUDIO MANAGER LOADED =====================");

        CustomMicrophone.RecordStreamDataEvent += RecordStreamDataEventHandler;
        CustomMicrophone.PermissionStateChangedEvent += PermissionStateChangedEventHandler;
        CustomMicrophone.RecordStartedEvent += RecordStartedEventHandler;
        CustomMicrophone.RecordEndedEvent += RecordEndedEventHandler;

        StartCoroutine(InitMicrophone());
     * 
     * */


    public void ToggleSound()
    {
        Debug.Log("SOUND ON: "+ sound_on);
        if (sound_on)
        {
            SoundOFF();
        }
        else
        {
            SoundON();
        }
    }

    public void SoundOFF()
    {
        sound_on = false;
        soundsAudioMixer.SetFloat("ui_sound_effects_volume", NO_VOLUME);
    }

    public void SoundON()
    {
        sound_on = true;
        soundsAudioMixer.SetFloat("ui_sound_effects_volume", DEFAULT_VOLUME);
    }

    
    public void SetLinearVolume(float sliderValue)
    {
        soundsAudioMixer.SetFloat("ui_sound_effects_volume", Mathf.Log10(sliderValue) * 20);
    }



    //================================================   Microphone Functions:   ================================================



    public void MicON()
    {
        Debug.Log("MIC IS ON!!");
        mic_is_on = true;
        micAudioSource.mute = false;
    }

    public void MicOFF()
    {
        Debug.Log("MIC IS OFF!!");
        mic_is_on = false;
        micAudioSource.mute = true;
    }

    public bool MicIsOn(){
        return mic_is_on;
    }

    public float GetMicVolume(){
        if (CustomMicrophone.HasConnectedMicrophoneDevices() && mic_is_on && CustomMicrophone.IsRecording(selectedMicrophone))
		{
            CustomMicrophone.IsVoiceDetected(selectedMicrophone, _workingClip, ref averageVoiceLevel, voiceDetectionTreshold);
            var volume = 20 * Mathf.Log10(Mathf.Abs(averageVoiceLevel)) + 80f + 30f;
            if(volume>0)
            {
                return volume;
            }
        }
        return 0f;
    }

     /// <summary>
        /// Works only in WebGL
        /// </summary>
        /// <param name="samples"></param>
        private void RecordStreamDataEventHandler(float[] samples)
        {
            // handle streaming recording data
        }

        /// <summary>
        /// Works only in WebGL
        /// </summary>
        /// <param name="permissionGranted"></param>
        private void PermissionStateChangedEventHandler(bool permissionGranted)
        {
            // handle current permission status

            Debug.Log($"Permission state changed on: {permissionGranted}");
        }

        private void RecordEndedEventHandler()
        {
            // handle record ended event

            Debug.Log("Record ended");
        }

        private void RecordStartedEventHandler()
        {
            // handle record started event

            Debug.Log("Record started");
        }

        private void RefreshMicrophoneDevicesButtonOnclickHandler()
		{
            CustomMicrophone.RefreshMicrophoneDevices();
        Debug.Log("===================== AAAAAAAAA =====================");

        if (!CustomMicrophone.HasConnectedMicrophoneDevices())
                return;
        Debug.Log("===================== BBBBBBBBB =====================");
        DevicesDropdownValueChangedHandler(0);
        }

        private void RequestPermission()
        {
            Debug.Log("AudioManager: RequestPermission()");

            CustomMicrophone.RequestMicrophonePermission();
        }

        public void StartRecordingAudio()
        {
            Debug.Log("AudioManager: StartRecordingAudio()");
            MicON();
            if (!CustomMicrophone.HasConnectedMicrophoneDevices())
            {
                Debug.Log("No connected devices found. Refreshing...");

                return;
            }

            _workingClip = CustomMicrophone.Start(selectedMicrophone, false, recordingTime, frequency);
        }

        public void StopRecordingAudio()
        {
            Debug.Log("AudioManager: StopRecordingAudio()");
            MicOFF();
            if (!CustomMicrophone.IsRecording(selectedMicrophone))
                return;

            // End recording is an async operation, so you have to provide callback or subscribe on RecordEndedEvent event
            CustomMicrophone.End(selectedMicrophone, () =>
            {
                // if (makeCopy)
                // {
                //     recordedClips.Add(CustomMicrophone.MakeCopy($"copy{recordedClips.Count}", recordingTime, frequency, _workingClip));
                //     micAudioSource.clip = recordedClips.Last();
                // }
                // else
                // {
                    micAudioSource.clip = _workingClip;
                // }

                // micAudioSource.Play();
            });
        }


        private void PlayRecordedAudio()
        {
            if (_workingClip == null)
                return;

            micAudioSource.clip = _workingClip;
            micAudioSource.Play();
        }

        private void DevicesDropdownValueChangedHandler(int index)
		{
            Debug.Log($"DevicesDropdownValueChangedHandler FLAG 1:  index: {index}  < CustomMicrophone.devices.Length: {CustomMicrophone.devices.Length}");
            if (index < CustomMicrophone.devices.Length)
            {
                Debug.Log($"DevicesDropdownValueChangedHandler FLAG 2:  CustomMicrophone.devices[index]: {CustomMicrophone.devices[index]}");
                selectedMicrophone = CustomMicrophone.devices[index];
                Debug.Log($"DevicesDropdownValueChangedHandler FLAG 3:  selectedMicrophone: {selectedMicrophone}");
            }
        }

//      private void RefreshMicrophoneDevicesButtonOnclickHandler()
// 		{
//             CustomMicrophone.RefreshMicrophoneDevices();

//             if (!CustomMicrophone.HasConnectedMicrophoneDevices())
//                 return;
//         }


//     public void UpdateMicrophone()
//     {
// // #if UNITY_EDITOR
//         micAudioSource.Stop();
//         //Start recording to audioclip from the mic
//         AudioConfiguration audioConfiguration = AudioSettings.GetConfiguration();
//         // micAudioSource.clip = Microphone.Start(microphone, true, 10, audioConfiguration.sampleRate);  // REMOVED FOR DEBUG (microphone)

//          if (!CustomMicrophone.HasConnectedMicrophoneDevices())
//             {
//                 Debug.Log("No connected devices found. Refreshing...");

//                 return;
//             }

//         micAudioSource.clip = CustomMicrophone.Start(microphone, false, 10, 44100);

//         //micAudioSource.clip = CustomMicrophone.Start(microphone, true, 10, audioConfiguration.sampleRate); // REMOVED FOR DEBUG (microphone)
//         micAudioSource.loop = true;
//         // Mute the sound with an Audio Mixer group becuase we don't want the player to hear it
//         // Debug.Log(Microphone.IsRecording(microphone).ToString());  // REMOVED FOR DEBUG (microphone)
//         Debug.Log(CustomMicrophone.IsRecording(microphone).ToString());

//         if (CustomMicrophone.IsRecording(microphone))
//         // if (Microphone.IsRecording(microphone))  // REMOVED FOR DEBUG (microphone)
//         { //check that the mic is recording, otherwise you'll get stuck in an infinite loop waiting for it to start
//             // while (!(Microphone.GetPosition(microphone) > 0))  // REMOVED FOR DEBUG (microphone)
//             while (!(CustomMicrophone.GetPosition(microphone) > 0))
//             {
//             } // Wait until the recording has started. 
//              Debug.Log("recording started with " + microphone);

//             // Start playing the audio source
//             micAudioSource.Play();
//         }
//         else
//         {
//             //microphone doesn't work for some reason
//             Debug.Log(microphone + " doesn't work!");
//         }
// // #endif
//     }

// // #if UNITY_WEBGL && !UNITY_EDITOR
// //     public float GetMicrophoneVolume()
// //     {
// //         return Microphone.GetMicrophoneVolume(0);
// //         // return volumes[0];
// //     }
// // #endif // REMOVED FOR DEBUG (microphone)

//     public float GetAveragedVolume()
//     {
//         float[] data = new float[256];
//         float a = 0;
//         micAudioSource.GetOutputData(data, 0);
//         foreach (float s in data)
//         {
//             a += Mathf.Abs(s);
//         }
//         Debug.Log($"AVG_VOL {a}");
//         return a / 256;
//     }

//     private float GetFundamentalFrequency()
//     {
//         float fundamentalFrequency = 0.0f;
//         float[] data = new float[numberOfSamples];
//         micAudioSource.GetSpectrumData(data, 0, fftWindow);
//         float s = 0.0f;
//         int i = 0;
//         for (int j = 1; j < numberOfSamples; j++)
//         {
//             if (data[j] > minThreshold) // volumn must meet minimum threshold
//             {
//                 if (s < data[j])
//                 {
//                     s = data[j];
//                     i = j;
//                 }
//             }
//         }
//         fundamentalFrequency = i * AudioSettings.outputSampleRate / numberOfSamples;
//         frequency = fundamentalFrequency;
//         return fundamentalFrequency;
//     }
}
// }