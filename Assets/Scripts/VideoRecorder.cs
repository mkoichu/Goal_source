// using UnityEngine;
// using UnityEngine.SceneManagement;
// using UnityEngine.UI;
// using OpenCVForUnity.CoreModule;
// using OpenCVForUnity.VideoioModule;
// using OpenCVForUnity.ImgprocModule;
// using OpenCVForUnity.UnityUtils;
// using UnityEngine.Networking;
// using System.Collections;

// namespace OpenCVForUnitySample
// {
//     /// <summary>
//     /// VideoWriter Example
//     /// An example of saving a video file using the VideoWriter class.
//     /// http://docs.opencv.org/3.2.0/dd/d43/tutorial_py_video_display.html
//     /// </summary>
//     public class VideoRecorder : MonoBehaviour
//     {

//         /// <summary>
//         /// The preview panel.
//         /// </summary>
//         public RawImage previewPanel;

//         /// <summary>
//         /// The max frame count.
//         /// </summary>
//         const int maxframeCount = 300;

//         /// <summary>
//         /// The frame count.
//         /// </summary>
//         int frameCount;

//         /// <summary>
//         /// The videowriter.
//         /// </summary>
//         VideoWriter writer;

//         /// <summary>
//         /// The videocapture.
//         /// </summary>
//         VideoCapture capture;

//         /// <summary>
//         /// The screen capture.
//         /// </summary>
//         Texture2D screenCapture;

//         /// <summary>
//         /// The recording frame rgb mat.
//         /// </summary>
//         Mat recordingFrameRgbMat;

//         /// <summary>
//         /// The preview rgb mat.
//         /// </summary>
//         Mat previewRgbMat;

//         /// <summary>
//         /// The preview texture.
//         /// </summary>
//         Texture2D previrwTexture;

//         /// <summary>
//         /// Indicates whether videowriter is recording.
//         /// </summary>
//         bool isRecording;

//         /// <summary>
//         /// Indicates whether videocapture is playing.
//         /// </summary>
//         bool isPlaying;

//         /// <summary>
//         /// The save path.
//         /// </summary>
//        // string savePath;

//         // Use this for initialization
//         void Start()
//         {
//             //previewPanel.gameObject.SetActive(false);

//             Initialize();
//         }


//         private void Initialize()
//         {
//             Texture2D imgTexture = Resources.Load("face") as Texture2D;

//             Mat imgMat = new Mat(imgTexture.height, imgTexture.width, CvType.CV_8UC4);

//             Utils.texture2DToMat(imgTexture, imgMat);

//             Texture2D texture = new Texture2D(imgMat.cols(), imgMat.rows(), TextureFormat.RGBA32, false);

//             Utils.matToTexture2D(imgMat, texture);

//         }

//         // Update is called once per frame
//         void Update()
//         {

//             if (isPlaying)
//             {
//                 //Loop play
//                 if (capture.get(Videoio.CAP_PROP_POS_FRAMES) >= capture.get(Videoio.CAP_PROP_FRAME_COUNT))
//                     capture.set(Videoio.CAP_PROP_POS_FRAMES, 0);

//                 if (capture.grab())
//                 {
//                     capture.retrieve(previewRgbMat, 0);

//                     Imgproc.rectangle(previewRgbMat, new Point(0, 0), new Point(previewRgbMat.cols(), previewRgbMat.rows()), new Scalar(0, 0, 255), 3);

//                     Imgproc.cvtColor(previewRgbMat, previewRgbMat, Imgproc.COLOR_BGR2RGB);
//                     Utils.fastMatToTexture2D(previewRgbMat, previrwTexture);
//                 }
//             }
//         }

//         void OnPostRender()
//         {
//             if (isRecording)
//             {
//                 if (frameCount >= maxframeCount ||
//                     recordingFrameRgbMat.width() != Screen.width || recordingFrameRgbMat.height() != Screen.height)
//                 {
//                     OnRecButtonClick();
//                     return;
//                 }

//                 frameCount++;

//                 // Take screen shot.
//                 screenCapture.ReadPixels(new UnityEngine.Rect(0, 0, Screen.width, Screen.height), 0, 0);
//                 screenCapture.Apply();

//                 Utils.texture2DToMat(screenCapture, recordingFrameRgbMat);
//                 Imgproc.cvtColor(recordingFrameRgbMat, recordingFrameRgbMat, Imgproc.COLOR_RGB2BGR);

//                /* Imgproc.putText(recordingFrameRgbMat, frameCount.ToString(), new Point(recordingFrameRgbMat.cols() - 70, 30), Imgproc.FONT_HERSHEY_SIMPLEX, 1.0, new Scalar(255, 255, 255), 2, Imgproc.LINE_AA, false);
//                 Imgproc.putText(recordingFrameRgbMat, "SavePath:", new Point(5, recordingFrameRgbMat.rows() - 30), Imgproc.FONT_HERSHEY_SIMPLEX, 0.8, new Scalar(0, 0, 255), 2, Imgproc.LINE_AA, false);
//                 Imgproc.putText(recordingFrameRgbMat, savePath, new Point(5, recordingFrameRgbMat.rows() - 8), Imgproc.FONT_HERSHEY_SIMPLEX, 0.5, new Scalar(255, 255, 255), 0, Imgproc.LINE_AA, false);*/

//                 writer.write(recordingFrameRgbMat);
//             }
//         }

//         public void StartRecording(string savePath)
//         {
//             if (isRecording || isPlaying)
//                 return;

//            // this.savePath = savePath;
//             Debug.Log("REC PATH: " + savePath);

//             writer = new VideoWriter();
// #if !UNITY_IOS
//             writer.open(savePath, VideoWriter.fourcc('M', 'J', 'P', 'G'), 30, new Size(Screen.width, Screen.height));
// #else
//             writer.open(savePath, VideoWriter.fourcc('D', 'V', 'I', 'X'), 30, new Size(Screen.width, Screen.height));
// #endif

//             if (!writer.isOpened())
//             {
//                 Debug.LogError("writer.isOpened() false");
//                 writer.release();
//                 return;
//             }

//             screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
//             recordingFrameRgbMat = new Mat(Screen.height, Screen.width, CvType.CV_8UC3);
//             frameCount = 0;

//             isRecording = true;
//         }

//         public void StopRecording()
//         {
//             if (!isRecording || isPlaying)
//                 return;

//             if (writer != null && !writer.IsDisposed)
//                 writer.release();

//             if (recordingFrameRgbMat != null && !recordingFrameRgbMat.IsDisposed)
//                 recordingFrameRgbMat.Dispose();

//             isRecording = false;
//         }

//         public void ShareVideo()
//         {
//             //StartCoroutine(ShareVideoRequest());
//         }

//         IEnumerator ShareVideoRequest()
//         {
//             string videoPath = Application.persistentDataPath + "/test_video.mp4";
//             string videoURL = System.IO.Path.Combine(Application.streamingAssetsPath, videoPath);
//             using (UnityWebRequest request = new UnityWebRequest(videoURL))
//             {
//                 request.downloadHandler = new DownloadHandlerBuffer();
//                 yield return request.SendWebRequest();
//                // MarksAssets.ShareNSaveWebGL.ShareNSaveWebGL.Share(request.downloadHandler.data, request.downloadHandler.data.Length, "test_video.mp4", "video/mp4", "https://haha.com", "The Title", "this is the content");
//             }
//         } 

//         private void PlayVideo(string filePath)
//         {
//             if (isPlaying || isRecording)
//                 return;

//             capture = new VideoCapture();
//             capture.open(filePath);

//             if (!capture.isOpened())
//             {
//                 Debug.LogError("capture.isOpened() is false. ");
//                 capture.release();
//                 return;
//             }

//             Debug.Log("CAP_PROP_FORMAT: " + capture.get(Videoio.CAP_PROP_FORMAT));
//             Debug.Log("CAP_PROP_POS_MSEC: " + capture.get(Videoio.CAP_PROP_POS_MSEC));
//             Debug.Log("CAP_PROP_POS_FRAMES: " + capture.get(Videoio.CAP_PROP_POS_FRAMES));
//             Debug.Log("CAP_PROP_POS_AVI_RATIO: " + capture.get(Videoio.CAP_PROP_POS_AVI_RATIO));
//             Debug.Log("CAP_PROP_FRAME_COUNT: " + capture.get(Videoio.CAP_PROP_FRAME_COUNT));
//             Debug.Log("CAP_PROP_FPS: " + capture.get(Videoio.CAP_PROP_FPS));
//             Debug.Log("CAP_PROP_FRAME_WIDTH: " + capture.get(Videoio.CAP_PROP_FRAME_WIDTH));
//             Debug.Log("CAP_PROP_FRAME_HEIGHT: " + capture.get(Videoio.CAP_PROP_FRAME_HEIGHT));
//             double ext = capture.get(Videoio.CAP_PROP_FOURCC);
//             Debug.Log("CAP_PROP_FOURCC: " + (char)((int)ext & 0XFF) + (char)(((int)ext & 0XFF00) >> 8) + (char)(((int)ext & 0XFF0000) >> 16) + (char)(((int)ext & 0XFF000000) >> 24));


//             previewRgbMat = new Mat();
//             capture.grab();

//             capture.retrieve(previewRgbMat, 0);

//             int frameWidth = previewRgbMat.cols();
//             int frameHeight = previewRgbMat.rows();
//             previrwTexture = new Texture2D(frameWidth, frameHeight, TextureFormat.RGB24, false);

//             capture.set(Videoio.CAP_PROP_POS_FRAMES, 0);

//             previewPanel.texture = previrwTexture;

//             isPlaying = true;
//         }

//         private void StopVideo()
//         {
//             if (!isPlaying || isRecording)
//                 return;

//             if (capture != null && !capture.IsDisposed)
//                 capture.release();

//             if (previewRgbMat != null && !previewRgbMat.IsDisposed)
//                 previewRgbMat.Dispose();

//             isPlaying = false;
//         }

//         /// <summary>
//         /// Raises the destroy event.
//         /// </summary>
//         void OnDestroy()
//         {
//             StopRecording();
//             StopVideo();
//         }



//         /// <summary>
//         /// Raises the rec button click event.
//         /// </summary>
//       public void OnRecButtonClick()
//         {
//             if (isRecording)
//             {
//                 StopRecording();

//                 //previewPanel.gameObject.SetActive(false);
//             }
//             else
//             {
// #if !UNITY_IOS
//                 StartRecording(Application.persistentDataPath + "/Recording.avi");
//                // StartRecording(Application.persistentDataPath + "/VideoWriterExample_output.mp4");
// #else
//                 StartRecording(Application.persistentDataPath + "/VideoWriterExample_output.m4v");
// #endif

//             }
//         }

//         /// <summary>
//         /// Raises the play button click event.
//         /// </summary>
//         public void OnPlayButtonClick()
//         {
//             if (isPlaying)
//             {
//                 StopVideo();
//                 //previewPanel.gameObject.SetActive(false);
//                 previewPanel.color = Color.black;
//             }
//             else
//             {
//                 if (string.IsNullOrEmpty(GameManager.Instance.video_save_path))
//                 {
//                     Debug.Log("Path is null: " + GameManager.Instance.video_save_path);
//                     return;
//                 }

//                 PlayVideo(GameManager.Instance.video_save_path);
//                 //previewPanel.gameObject.SetActive(true);
//                 previewPanel.color = Color.white;
//             }
//         }
//     }
    
// }