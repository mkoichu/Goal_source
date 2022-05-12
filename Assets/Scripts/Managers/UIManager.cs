using MainDefinitions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using MarksAssets.VideoPlayerWebGL;

public class UIManager : MonoBehaviour
{
    [Header("UI From DB")]
    [SerializeField] private RawImage mainBGImage;
    [SerializeField] private RawImage celebBGImage;
     [SerializeField] private RawImage logo1Image;
     [SerializeField] private RawImage logo2Image;
     [SerializeField] private RawImage logo3Image;       //UNREMOVED FOR DEBUG (COMENTATOR PANEL)

    [SerializeField] private TextMeshProUGUI toWinText;
    [SerializeField] private RawImage prizeImage;
    [SerializeField] private TextMeshProUGUI prizeDescText;

    [SerializeField] Texture2D DEFAULT_MAIN_TEXTURE;
    [SerializeField] Texture2D DEFAULT_CELEB_TEXTURE;

    [Header("Loading Panels:")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Button loadingScreenButton;
    [SerializeField] private GameObject lblLoading;
    [SerializeField] private GameObject lblClickToStart;

    [Header("Main Menu/Settings Panels")]
    [SerializeField] private GameObject menuScreen;
    [SerializeField] private GameObject registrationScreen;
    [SerializeField] private GameObject helpScreen;
    [SerializeField] private GameObject settingsScreen;
    [SerializeField] private GameObject dashboardScreen;

    [SerializeField] private GameObject welcomePanel;
    [SerializeField] private GameObject tournamentPanel;
    [SerializeField] private GameObject scorePanel;


    [Header("Scoreboard Gameobjects:")]
    [SerializeField] private GameObject scoreboardScreen;
    [SerializeField] private GameObject PreviousScoreboard;
    [SerializeField] private GameObject CurrentScoreboard;
    [SerializeField] private GameObject AlltimeScoreboard;
    [SerializeField] private GameObject PreviousScoreboardButton;
    [SerializeField] private GameObject CurrentScoreboardButton;
    [SerializeField] private GameObject AlltimeScoreboardButton;


    [Header("Game Panels")]
    [SerializeField] private GameObject gameSessionScreen;
    [SerializeField] private GameObject getReadyScreen;
    [SerializeField] private GameObject arStoreScreen;
    [SerializeField] private GameObject playScreen;
    [SerializeField] private GameObject celebrationScreen;

    [SerializeField] private GameObject comentatorPanel;      //UNREMOVED FOR DEBUG (COMENTATOR PANEL)
    // [SerializeField] private GameObject youtubeFootagePanel;      //UNREMOVED FOR DEBUG (youtubePlayer)
    // [SerializeField] private GameObject thumbnailObject;      //UNREMOVED FOR DEBUG (youtubePlayer)
    // [SerializeField] private GameObject gameScorePanel;
    // [SerializeField] private GameObject countDownPanel;
    [SerializeField] public VideoPlayerWebGL videoPlayerWebGL;

    [Header("Face AR Objects")]
    [SerializeField] private GameObject zapparCamera;
    [SerializeField] private GameObject zapparFaceTracker;
    [SerializeField] private Transform timeElements;

    //Private Fields:
    private Vector3 comentatorPanelOriginalPos;
    private Vector3 comentatorPanelOriginalScale;

    private int INVISIBLE_LAYER = 6;
    private int DEFAULT_LAYER = 0;
    // private Animator comentatorPanelAnimation;

    private bool no_tournament = false;


    public static UIManager Instance { get; private set; }
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
         comentatorPanelOriginalPos = new Vector3(comentatorPanel.transform.position.x, comentatorPanel.transform.position.y, comentatorPanel.transform.position.z);
         comentatorPanelOriginalScale =  new Vector3(comentatorPanel.transform.localScale.x, comentatorPanel.transform.localScale.y, comentatorPanel.transform.localScale.z);
        //  comentatorPanelAnimation = comentatorPanel.GetComponent<Animator>();      //UNREMOVED FOR DEBUG (COMENTATOR PANEL)
    }

    //===============================================   Loading Screen Funcions:   ===============================================
    public void OnFinishedLoading()
    {
        lblLoading.SetActive(false);
        lblClickToStart.SetActive(true);
        loadingScreenButton.interactable = true;
        AudioManager.Instance.MicOFF();
    }

    public IEnumerator LoadGetReadyScreenAssets()
    {
        // OnPlayClick();
        OnGetReadyReturnClick();
        yield return null;
    }

    public void OnScreenClick()
    {
        //Enable:
        menuScreen.SetActive(true);
        ActivateTourPanel(true);
        scorePanel.SetActive(true);

        //Disable:
        loadingScreen.SetActive(false);
        gameSessionScreen.SetActive(false);
    }

    void ActivateTourPanel(bool t_f)
    {
        if(t_f == false || no_tournament == true)
        {
            tournamentPanel.SetActive(false);
        }
        else
        {
            tournamentPanel.SetActive(true);
        }
        
    }

    //=================================================   UI From Firebase DB:   =================================================

    public IEnumerator LoadMainBGImage(DBImage dbImage)
    {
        if (dbImage == null || dbImage.url == null || dbImage.url == "")
        {
            Debug.Log("LoadDefaultMainImage()");
            LoadDefaultMainImage();
        }
        else
        {
            WWW wwwLoader = new WWW(dbImage.url);
            yield return wwwLoader;

            if (wwwLoader.texture)
            {
                mainBGImage.texture = wwwLoader.texture;
            }
            else
            {
                LoadDefaultMainImage();
            }
        }
    }

    public void LoadDefaultMainImage()
    {
        mainBGImage.texture = DEFAULT_MAIN_TEXTURE;
    }

    public IEnumerator LoadCelebBGImage(DBImage dbImage)
    {
        if (dbImage == null || dbImage.url == null || dbImage.url == "")
        {
            Debug.Log("LoadDefaultCelebImage()");
            LoadDefaultCelebImage();
        }
        else
        {
            WWW wwwLoader = new WWW(dbImage.url);
            yield return wwwLoader;

            if (wwwLoader.texture)
            {
                celebBGImage.texture = wwwLoader.texture;
            }
            else
            {
                LoadDefaultCelebImage();
            }
        }
    }

    public void LoadDefaultCelebImage()
    {
        celebBGImage.texture = DEFAULT_CELEB_TEXTURE;
    }

     public IEnumerator LoadLogo1mage(DBImage dbImage)
     {
         if (dbImage == null || dbImage.url == null || dbImage.url == "")
         {
             Debug.Log("DisableLogo1Image()");
             DisableLogo1Image();
         }
         else
         {
             WWW wwwLoader = new WWW(dbImage.url);
             yield return wwwLoader;

             if (logo1Image.texture)
             {
                 logo1Image.texture = wwwLoader.texture;
             }
             else
             {
                 DisableLogo1Image();
             }
         }    
     }

     public void DisableLogo1Image()
     {
         logo1Image.gameObject.SetActive(false);
     }

     public IEnumerator LoadLogo2mage(DBImage dbImage)
     {
         if (dbImage == null || dbImage.url == null || dbImage.url == "")
         {
             Debug.Log("DisableLogo2Image()");
             DisableLogo2Image();
         }
         else
         {
             WWW wwwLoader = new WWW(dbImage.url);
             yield return wwwLoader;

             if (wwwLoader.texture)
             {
                 logo2Image.texture = wwwLoader.texture;
             }
             else
             {
                 DisableLogo2Image();
             }
         }
     }
     public void DisableLogo2Image()
     {
         logo2Image.gameObject.SetActive(false);
     }

     public IEnumerator LoadLogo3mage(DBImage dbImage)
     {
         if (dbImage == null || dbImage.url == null || dbImage.url == "")
         {
             Debug.Log("DisableLogo3Image()");
             DisableLogo3Image();
         }
         else
         {
             WWW wwwLoader = new WWW(dbImage.url);
             yield return wwwLoader;

             if (wwwLoader.texture)
             {
                 logo3Image.texture = wwwLoader.texture;
             }
             else
             {
                 DisableLogo3Image();
             }
         }
     }
     public void DisableLogo3Image()
     {
         logo3Image.gameObject.SetActive(false);
     }      //UNREMOVED FOR DEBUG (COMENTATOR PANEL)



    //===============================================   Main Menu Screen Functions:   ===============================================
    public void OnPlayClick()
    {
        //GameManager.Instance.PreloadYoutubeClip();

        // SetLayerRecursively(youtubeFootagePanel, INVISIBLE_LAYER);      //UNREMOVED FOR DEBUG (youtubePlayer)

        //Enable
        // youtubeFootagePanel.SetActive(true);      //UNREMOVED FOR DEBUG (youtubePlayer)
        gameSessionScreen.SetActive(true);
        comentatorPanel.SetActive(true);      //UNREMOVED FOR DEBUG (COMENTATOR PANEL)
        getReadyScreen.SetActive(true);
        zapparCamera.SetActive(true);
        zapparFaceTracker.SetActive(true);

        //Play video:
        videoPlayerWebGL.Source(MarksAssets.VideoPlayerWebGL.VideoPlayerWebGL.srcs.External, GameManager.Instance.clipURL);
        videoPlayerWebGL.CreateVideoWrapper();
        
        //Disable:
        ActivateTourPanel(false);
        menuScreen.SetActive(false);
    }

    public void OnPlayVideoClip(){
        Debug.Log("OnPlayVideoClip()");
        videoPlayerWebGL.Volume(1f);
        videoPlayerWebGL.Play();
    }

    public void OnHelpClick()
    {
        //Enable:
        helpScreen.SetActive(true);

        //Disable:
        ActivateTourPanel(false);
        menuScreen.SetActive(false);
    }

    public void OnScoreboardClick()
    {
        //Enable:
        scoreboardScreen.SetActive(true);

        //Disable:
        menuScreen.SetActive(false);
    }

    public void OnSettingsClick()
    {
        //Enable:
        settingsScreen.SetActive(true);

        //Disable:
        ActivateTourPanel(false);
        menuScreen.SetActive(false);
    }

    public void OnRegisterClick()
    {
        //Enable:
        registrationScreen.SetActive(true);

        //Disable:
        scoreboardScreen.SetActive(false);
        ActivateTourPanel(false);
        menuScreen.SetActive(false);
        getReadyScreen.SetActive(false);
        comentatorPanel.SetActive(false);      //UNREMOVED FOR DEBUG (COMENTATOR PANEL)
        zapparCamera.SetActive(false);
        zapparFaceTracker.SetActive(false);
    }
    public void DisableTournament()
    {
       // Debug.Log("TOUR CLOSED");
        no_tournament = true;
        ActivateTourPanel(false);
    }

    public void EnableTournament()
    {
       // Debug.Log("TOUR OPEN");
        no_tournament = false;
        ActivateTourPanel(true);
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }

    public void DisablePrizeImage()
    {
        if (prizeDescText.gameObject.active == false)
        {
            toWinText.gameObject.SetActive(false);
        }
        toWinText.gameObject.SetActive(false);
        prizeImage.gameObject.SetActive(false);
    }

    public IEnumerator LoadPrizeImage(DBImage dbImage)
    {
        if (dbImage == null || dbImage.url == null || dbImage.url == "")
        {
            DisablePrizeImage();
        }
        else
        {
            WWW wwwLoader = new WWW(dbImage.url);
            yield return wwwLoader;

            toWinText.gameObject.SetActive(true);
            prizeImage.gameObject.SetActive(true);
            if (wwwLoader.texture != null)
            {
                prizeImage.texture = wwwLoader.texture;
            }
            else
            {
                DisablePrizeImage();
            }
        }
        
    }

    public void DisablePrizeDesc()
    {
        if(prizeImage.gameObject.active == false)
        {
            toWinText.gameObject.SetActive(false);
        }
        prizeDescText.gameObject.SetActive(false);
    }

    public void LoadPrizeDesc(string text)
    {
        if (text != null && text != "")
        {
            toWinText.gameObject.SetActive(true);
            prizeDescText.gameObject.SetActive(true);
            prizeDescText.text = text;
        }
        else
        {
            DisablePrizeDesc();
        }
        
    }

    //===============================================   Registration Screen Functions:   ===============================================
    public void OnRegistrationExit()
    {
        //Enable:
        ActivateTourPanel(true);
        menuScreen.SetActive(true);
        welcomePanel.SetActive(true);

        //Disable:
        registrationScreen.SetActive(false);
        gameSessionScreen.SetActive(false);
    }


    //===============================================   Help Screen Functions:   ===============================================
    public void OnHelpExit()
    {
        //Enable:
        ActivateTourPanel(true);
        menuScreen.SetActive(true);

        //Disable:
        helpScreen.SetActive(false);
        gameSessionScreen.SetActive(false);
    }


    //===============================================   Settings Screen Functions:   ===============================================
    public void OnSettingsOKClick()
    {
        //Enable:
        ActivateTourPanel(true);
        menuScreen.SetActive(true);

        //Disable:
        settingsScreen.SetActive(false);
        gameSessionScreen.SetActive(false);
    }

    public void OnDashboardClick()
    {
        //Enable:
        dashboardScreen.SetActive(true);

        //Disable:
        settingsScreen.SetActive(false);
    }


    //===============================================   Scoreboard Screen Functions:   ===============================================
    public void OnScoreboardReturnClick()
    {
        //Enable:
        menuScreen.SetActive(true);

        //Disable:
        scoreboardScreen.SetActive(false);
        gameSessionScreen.SetActive(false);
    }


    public void InitializePreviousScoreboardUI()
    {
        PreviousScoreboard.SetActive(true);
        CurrentScoreboard.SetActive(false);
        AlltimeScoreboard.SetActive(false);

        PreviousScoreboardButton.GetComponent<Animator>().SetBool("isChosen", true);
        CurrentScoreboardButton.GetComponent<Animator>().SetBool("isChosen", false);
        AlltimeScoreboardButton.GetComponent<Animator>().SetBool("isChosen", false);

        PreviousScoreboardButton.GetComponent<Canvas>().sortingOrder = 3;
        CurrentScoreboardButton.GetComponent<Canvas>().sortingOrder = 2;
        AlltimeScoreboardButton.GetComponent<Canvas>().sortingOrder = 1;
    }

    public void InitializeCurrentScoreboardUI()
    {
        PreviousScoreboard.SetActive(false);
        CurrentScoreboard.SetActive(true);
        AlltimeScoreboard.SetActive(false);

        PreviousScoreboardButton.GetComponent<Animator>().SetBool("isChosen", false);
        CurrentScoreboardButton.GetComponent<Animator>().SetBool("isChosen", true);
        AlltimeScoreboardButton.GetComponent<Animator>().SetBool("isChosen", false);

        PreviousScoreboardButton.GetComponent<Canvas>().sortingOrder = 3;
        CurrentScoreboardButton.GetComponent<Canvas>().sortingOrder = 4;
        AlltimeScoreboardButton.GetComponent<Canvas>().sortingOrder = 1;
    }

    public void InitializeAlltimeUI()
    {
        PreviousScoreboard.SetActive(false);
        CurrentScoreboard.SetActive(false);
        AlltimeScoreboard.SetActive(true);

        PreviousScoreboardButton.GetComponent<Animator>().SetBool("isChosen", false);
        CurrentScoreboardButton.GetComponent<Animator>().SetBool("isChosen", false);
        AlltimeScoreboardButton.GetComponent<Animator>().SetBool("isChosen", true);

        PreviousScoreboardButton.GetComponent<Canvas>().sortingOrder = 3;
        CurrentScoreboardButton.GetComponent<Canvas>().sortingOrder = 2;
        AlltimeScoreboardButton.GetComponent<Canvas>().sortingOrder = 4;
    }


    //===============================================   GetReadyScreen Functions:   ===============================================
   
    public void OnStartClick()
    {
        Debug.Log("OnStartClick()");
        AudioManager.Instance.InitMic(OnGotMicrophonePermission);
    }

    private void OnGotMicrophonePermission()
    {
        Debug.Log("OnGotMicrophonePermission()");
        SetLayerRecursively(videoPlayerWebGL.gameObject, DEFAULT_LAYER);      //videoPlayerWebGL
        //  SetLayerRecursively(youtubeFootagePanel, DEFAULT_LAYER);      //UNREMOVED FOR DEBUG (youtubePlayer)

        //Enable:
        // youtubeFootagePanel.SetActive(true);      //UNREMOVED FOR DEBUG (youtubePlayer)
        // thumbnailObject.gameObject.SetActive(true);      //UNREMOVED FOR DEBUG (youtubePlayer)
        playScreen.SetActive(true);

        //Disable:
        getReadyScreen.SetActive(false);
        scorePanel.SetActive(false);
        OnPlayVideoClip();

        MinimizeComentatorPanel();
    }
   
    public void OnGetReadyReturnClick()
    {
        //Enable:
        ActivateTourPanel(true);
        menuScreen.SetActive(true);

        //Disable:
        getReadyScreen.SetActive(false);
        comentatorPanel.SetActive(false);      //UNREMOVED FOR DEBUG (COMENTATOR PANEL)
        gameSessionScreen.SetActive(false);
        zapparCamera.SetActive(false);
        zapparFaceTracker.SetActive(false);
    }

    public void OnARStoreClick()
    {
        //Enable:
        arStoreScreen.SetActive(true);

        //Disable:
        getReadyScreen.SetActive(false);
        comentatorPanel.SetActive(false);      //UNREMOVED FOR DEBUG (COMENTATOR PANEL)
        zapparCamera.SetActive(false);
        zapparFaceTracker.SetActive(false);
    }


    public void OnGameOver()
    {
        // Reset position and scale:
        comentatorPanel.transform.position = new Vector3(comentatorPanelOriginalPos.x, comentatorPanelOriginalPos.y, comentatorPanelOriginalPos.z);
        comentatorPanel.transform.localScale = new Vector3(comentatorPanelOriginalScale.x, comentatorPanelOriginalScale.y, comentatorPanelOriginalScale.z);      //UNREMOVED FOR DEBUG (COMENTATOR PANEL)
        videoPlayerWebGL.Stop();
        SetLayerRecursively(videoPlayerWebGL.gameObject, INVISIBLE_LAYER);      //videoPlayerWebGL
        // SetLayerRecursively(youtubeFootagePanel, INVISIBLE_LAYER);      //UNREMOVED FOR DEBUG (youtubePlayer)
        
        // comentatorPanelAnimation.enabled = false;      //UNREMOVED FOR DEBUG (COMENTATOR PANEL)
        //countDown.SetActive(false);
        //Enable:
        celebrationScreen.SetActive(true);

        //Diable:
        comentatorPanel.SetActive(false);      //UNREMOVED FOR DEBUG (COMENTATOR PANEL)
        zapparCamera.SetActive(false);
        zapparFaceTracker.SetActive(false);
        //youtubeFootagePanel.SetActive(false);
        playScreen.SetActive(false);
        AudioManager.Instance.StopRecordingAudio();
        DeactivateAllTimeElementsx();
    }

    public void ActivateTimeElementByIndex(int index){
        timeElements.GetChild(index).gameObject.SetActive(true);
    }

    public void DeactivateAllTimeElementsx(){
        foreach(Transform child in timeElements){
            child.gameObject.SetActive(false);
        }
    }

    
    //================================================   ARStoreScreen Functions:   ================================================
    public void OnARStoreOKClick()
    {
        GameManager.Instance.SaveGame();

        //Enable:
        getReadyScreen.SetActive(true);
        comentatorPanel.SetActive(true);      //UNREMOVED FOR DEBUG (COMENTATOR PANEL)
        zapparCamera.SetActive(true);
        zapparFaceTracker.SetActive(true);

        //Disable:
        arStoreScreen.SetActive(false);
    }

    //=================================================   Play Screen Functions:   =================================================
    public void MinimizeComentatorPanel()
    {
        // comentatorPanelAnimation.enabled = true;
        // comentatorPanelAnimation.Play("MinimizeCommentaryPanel");      //UNREMOVED FOR DEBUG (COMENTATOR PANEL)
    }

    public void MaximizeComentatorPanel()
    {
        // comentatorPanelAnimation.enabled = true;
        // comentatorPanelAnimation.Play("MaximizeCommentaryPanel");      //UNREMOVED FOR DEBUG (COMENTATOR PANEL)
    }


    //===============================================   CelebrationScreen Functions:   ===============================================
    public void OnShareClick()
    {

    }
    public void OnStoreClick()
    {

    }
    public void OnHomeClick()
    {
        //Enable
        ActivateTourPanel(true);
        menuScreen.SetActive(true);

        //Disable:
        gameSessionScreen.SetActive(false);
        celebrationScreen.SetActive(false);
    }


    //===============================================   Auxiliary Functions:   ===============================================

    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}
