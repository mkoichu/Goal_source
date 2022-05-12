using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

using Tamarin.FirebaseX;
using Tamarin.Common;
//using Firebase.Firestore;
using Newtonsoft.Json;
using TMPro;
using MainDefinitions;
using System.Threading.Tasks;
// using LightShaft.Scripts;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    // [SerializeField] private YoutubePlayer youtubePlayer;      //UNREMOVED FOR DEBUG (youtubePlayer)
    public StoreManager storeManager;

    public SortedList<int, Player> previousScoreboard;
    public SortedList<int, Player> currentScoreboard;
    public SortedList<int, Player> alltimeScoreboard;

    
    //Foorage variables:
    // public string clipLink;   //UNREMOVED FOR DEBUG (youtubePlayer)
    public float timeOfGoal = 5f;
    public float clipStartTime = 0f;
    public float clipEndTime = 0;

    //Tournament variables:
    public TourModes tourMode = TourModes.None;
    public Date tourStart;
    public Date tourEnd;

    //Merch Sales variables:
    private string storeURL;
    public string clipURL;

    public DateTime currTournamentEndDate;
    public DateTime currTournamentStartDate;
    public DateTime nextTournamentTargetDate;

    public GameObject AccountIsInactivePanel;

    public FirebaseAPI firebase;
    public GameObject _completed;


    
    static bool is_active = false;
    public int totalScore=0;
    public TextMeshProUGUI totalScoreLabel;



    public TMP_FontAsset hebrewFont;

    public bool firebase_ready = false;
    public string playersName = Definitions.DEFAULT_NAME;
    public string playersEmail = Definitions.DEFAULT_EMAIL;
    private Content content;



    public static GameManager Instance { get; private set; }

    public string video_save_path;

    //==========================================================   Main Functions:   ==========================================================

    void Awake(){
        GameFlowStates.state = PossibleGameFlowStates.UnityLoaded;
        Debug.Log("===================== UNITY LOADED =====================");
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Debug.LogError("There is more than one instance!");
            Destroy(gameObject);
        }
    }


    async void Start()
    {
        previousScoreboard = new SortedList<int, Player>(new DescComparer<int>());
        currentScoreboard = new SortedList<int, Player>(new DescComparer<int>());
        alltimeScoreboard = new SortedList<int, Player>(new DescComparer<int>());
        video_save_path = Application.persistentDataPath + "/Recording.avi";


        firebase = FirebaseAPI.Instance;
        FirebaseUser user;

        await Waiter.Until(() => FirebaseAPI.Instance.ready == true);
        GameFlowStates.state = PossibleGameFlowStates.FirebaseLoaded;
        Debug.Log("===================== FIREBASE LOADED =====================");

        firebase_ready = true;
        Debug.Log("FIREBASE INITIALIZED EVENT");
        StartCoroutine(LoadGame());

        
    }

    private IEnumerator LoadGame()
    {
        var loadTask = LoadContent();
        yield return new WaitUntil(() => loadTask.IsCompleted);

        loadTask = LoadScoreboards();
        yield return new WaitUntil(() => loadTask.IsCompleted);
        
        //LoadARStoreElements();
        ModifyTotalScoreBy(9000);

        yield return StartCoroutine(UIManager.Instance.LoadGetReadyScreenAssets());
        UIManager.Instance.OnFinishedLoading();

        GameFlowStates.state = PossibleGameFlowStates.GameLoaded;
        Debug.Log("===================== GAME LOADED =====================");
    }

    public void SaveGame()
    {
        DataManager.SaveOptions(this);
    }

    public void QuitGame()
    {
        SaveGame();
        Application.Quit();
    }

    public void OpenOnlineStore()
    {
        if (content.storeURL != null && content.storeURL != "")
        {
            Application.OpenURL(storeURL);
        }
    }


    //==========================================================   Register Functions:   ==========================================================
    async public void GoogleSignIn()
    {
       /* string authCode = null;
        string idToken = null;

        if (Application.platform != RuntimePlatform.WebGLPlayer)
            authCode = "Google auth token from facebook SDK";
        if (Application.platform == RuntimePlatform.WebGLPlayer)
            authCode = null;

        await Waiter.Until(() => firebase.ready == true);
        var auth = await firebase.SignInGoogle(idToken, authCode);
        var deserializedAuth = JsonConvert.SerializeObject(auth);
        Debug.Log($"Google: { deserializedAuth }");*/
    }

    async public void FacebookSignIn()
    {
       /* string accessToken = null;
        if (Application.platform != RuntimePlatform.WebGLPlayer)
            accessToken = "Facebook auth token from facebook SDK";
        if (Application.platform == RuntimePlatform.WebGLPlayer)
            accessToken = null;

        await Waiter.Until(() => firebase.ready == true);
        var auth = await firebase.SignInFacebook(accessToken);
        var deserializedAuth = JsonConvert.SerializeObject(auth);
        Debug.Log($"Facebook: { deserializedAuth }");*/
    }

    //===================================================   Load Dashboard Elements Functions:   ===================================================
    private string prizeDesc;
    async private Task<bool> LoadContent()
    {
        var path = $"/{Definitions.DB_ROOT_NAME}/{Definitions.clientName}/";
        var data = await firebase.database.QueryAsync(path);
        if (!data.ToString().Contains("ERROR") )
        {
            content = JsonConvert.DeserializeObject<Content>(data);
            if (content != null)
            {
                Debug.Log($"================= CONTENT: =================");
               /* Debug.Log($"DATA: {data}");
                Debug.Log($"CLIP URL: {content.clipURL}");
                Debug.Log($"GOAL TIME: {content.goalTime}");
                Debug.Log($"MAIN BG: {content.mainBGImage.url}");
                Debug.Log($"CELEB BG: {content.celebBGImage.url}");
                Debug.Log($"LOGO 1: {content.logo1Image.url}");
                Debug.Log($"LOGO 2: {content.logo2Image.url}");
                Debug.Log($"LOGO 3: {content.logo3Image.url}");
                Debug.Log($"TOUR MODE: {content.tourMode}");
                Debug.Log($"TOUR START: {content.tourStart}:  {content.tourStart.day}/{content.tourStart.month}/{content.tourStart.year}");
                Debug.Log($"TOUR END: {content.tourEnd}:   {content.tourEnd.day}/{content.tourEnd.month}/{content.tourEnd.year}");
                Debug.Log($"PRIZE: {content.prizeImage.url}");
                Debug.Log($"PRIZE DESC: {content.prizeDesc}");
                Debug.Log($"STORE URL: {content.storeURL}");*/
                Debug.Log($"============================================");


                //======== Backgrounds & Logos settings: ========
                //Load Main background:
                StartCoroutine(UIManager.Instance.LoadMainBGImage(content.mainBGImage));
                StartCoroutine(UIManager.Instance.LoadCelebBGImage(content.celebBGImage));
                StartCoroutine(UIManager.Instance.LoadLogo1mage(content.logo1Image));
                StartCoroutine(UIManager.Instance.LoadLogo2mage(content.logo2Image));
                StartCoroutine(UIManager.Instance.LoadLogo3mage(content.logo3Image));      //UNREMOVED FOR DEBUG (COMENTATOR PANEL)

                //Tournament settings;
                if (content.tourMode != null && content.tourMode != TourModes.None)
                {
                    tourMode = content.tourMode;
                    tourStart = content.tourStart;
                    tourEnd = content.tourEnd;
                }
                else
                {
                    tourMode = TourModes.None;
                    tourStart = null;
                    tourEnd = null;
                }
                
                StartCoroutine(UIManager.Instance.LoadPrizeImage(content.prizeImage));
                UIManager.Instance.LoadPrizeDesc(content.prizeDesc);

                //Merch Sales settings:
                storeURL = content.storeURL;

                //Footage settings:
                clipURL = content.clipURL;  //REMOVED FOR DEBUG (clipLink)
                Debug.Log($"GAME MANAGER LOADING CONTENT:   clipURL: {content.clipURL}");
                UIManager.Instance.videoPlayerWebGL.Source(MarksAssets.VideoPlayerWebGL.VideoPlayerWebGL.srcs.External, content.clipURL);
                // clipLink = "https://www.youtube.com/watch?v=6PHDBfocvSo";    //UNREMOVED FOR DEBUG (youtubePlayer)
                // StartCoroutine(PreloadYoutubeClip());     //UNREMOVED FOR DEBUG (youtubePlayer)
            }
        }
        return true;
    }

    // private IEnumerator PreloadYoutubeClip()
    // {
        //  youtubePlayer.transform.parent.gameObject.SetActive(true);      //UNREMOVED FOR DEBUG (youtubePlayer)
        //  youtubePlayer.PreLoadVideo(clipLink);      //UNREMOVED FOR DEBUG (youtubePlayer)
    //     yield return null;
    // }



    //=================================================== Load Tournament Functions:  ===================================================
    public int startMonth = 11;
    public int startDay = 2;
    public int startHour = 2;
    public int startMinute = 23;
    public int startSecond = 0;
    public int endYear = 2022;
    public int endMonth = 4;
    public int endDay = 25;
    public int endHour = 0;
    public int endMinute = 0;
    public int endSecond = 0;
    [SerializeField] private TextMeshProUGUI output;

    public void LoadNewTournament()
    {
        //If tourMode is None, null or empty asume no tournament:
        if (tourMode == null || tourMode == TourModes.None)
        {
            UIManager.Instance.DisableTournament();
        }
        else if(tourMode == TourModes.SchedTour && tourStart != null && tourEnd != null)
        {
            currTournamentStartDate = new DateTime(tourStart.year, startMonth, startDay, startHour, startMinute, startSecond);
            //currTournamentStartDate = new DateTime(tourStart.year, tourStart.month, tourStart.day, tourStart.hour, tourStart.minute, tourStart.second);
            TimeSpan timeToStart = currTournamentStartDate - DateTime.Now;
            bool tour_began = ((float)(timeToStart).TotalSeconds < 0);

            currTournamentEndDate = new DateTime(endYear, endMonth, endDay, endHour, endMinute, endSecond);
            //currTournamentEndDate = new DateTime(tourEnd.year, tourEnd.month, tourStart.day, tourEnd.hour, tourEnd.minute, tourEnd.second);
            TimeSpan timeToEnd = GameManager.Instance.currTournamentEndDate - DateTime.Now;
            bool tour_ended = ((float)(timeToEnd).TotalSeconds <= 0);

            output.text = ($"TOUR START: {startDay}/{startMonth}/2021   TOUR END: {endDay}/{endMonth}/2021   PRIZE DESC: {prizeDesc}");
            //output.text = ($"TOUR START: {tourStart.day}/{tourStart.month}/{tourStart.year}   TOUR END: {tourEnd.day}/{tourEnd.month}/{tourEnd.year}   PRIZE DESC: {prizeDesc}");
            if (tour_began && !tour_ended)
            {
                Debug.Log("LoadNewTournament() OPEN");
                UIManager.Instance.EnableTournament();
            }
            else
            {
                Debug.Log("LoadNewTournament() CLOSE");
                UIManager.Instance.DisableTournament();
            }
        }
    }

    /*async public void LoadCurrTournamentTargetDate()
    {
        var path = $"/{Definitions.DB_ROOT_NAME}/{Definitions.clientName}/{Definitions.CURR_TOURNAMENT_TARGET_DATE}/";
        var data = await firebase.RealQueryAsync(path);

        if (data.ToString().Contains("ERROR"))
        {
            currTournamentTargetDate = CalculateNextSundayDate(1);
            UpdateCurrTournamentTargetDate();
        }

        Date dbTargetDate = JsonConvert.DeserializeObject<Date>(data);
        DateTime dtTargetDate;
        if (dbTargetDate != null)
        {
            dtTargetDate = new DateTime(dbTargetDate.year, dbTargetDate.month, dbTargetDate.day, dbTargetDate.hour, dbTargetDate.minute, dbTargetDate.second);
            TimeSpan diff = dtTargetDate - DateTime.Now;
            if ((float)diff.TotalSeconds <= 0)
            {
                currTournamentTargetDate = CalculateNextSundayDate(1);
                UpdateCurrTournamentTargetDate();
            }
            else
            {
                currTournamentTargetDate = dtTargetDate;
            }
        }
        else
        {
            currTournamentTargetDate = CalculateNextSundayDate(1);
            UpdateCurrTournamentTargetDate();
        }

    }*/


    /*    async public void LoadNextTournamentTargetDate()
        {
            var path = $"/{Definitions.DB_ROOT_NAME}/{Definitions.clientName}/next_tournament_target_date/";
            var data = await firebase.RealQueryAsync(path);
            if (data == null)
            {
                nextTournamentTargetDate = CalculateNextSundayDate(8);
            }
            else
            {
                nextTournamentTargetDate = CalculateNextSundayDate(8);
            }
        }*/


    async public void UpdatePreviousTournamentScoreboard()
    {
        /*previousScoreboard = new SortedList<int, Player>(currentScoreboard);
        firebase.RealSetAsync($"{Definitions.DB_ROOT_NAME}/{Definitions.clientName}/{Definitions.PREV_SCOREBOARD_DB_NAME}/", previousScoreboard.Values);
        currentScoreboard.Clear();
        firebase.RealSetAsync($"{Definitions.DB_ROOT_NAME}/{Definitions.clientName}/{Definitions.CURR_SCOREBOARD_DB_NAME}/", currentScoreboard.Values);*/
    }

   /* async public void UpdateCurrTournamentTargetDate()
    {
        if (!firebase_ready) return;
        Date dbTargetDate = new Date(currTournamentTargetDate.Year, currTournamentTargetDate.Month, currTournamentTargetDate.Day, currTournamentTargetDate.Hour, currTournamentTargetDate.Minute, currTournamentTargetDate.Second);
        firebase.RealSetAsync($"{Definitions.DB_ROOT_NAME}/{Definitions.clientName}/{Definitions.CURR_TOURNAMENT_TARGET_DATE}/", dbTargetDate);
    }

    async public void UpdateCurrTournamentTargetDate_DEBUG(Date date)
    {
        if (!firebase_ready) return;
        firebase.RealSetAsync($"{Definitions.DB_ROOT_NAME}/{Definitions.clientName}/{Definitions.CURR_TOURNAMENT_TARGET_DATE}/", date);
    }*/


    private DateTime CalculateNextSundayDate(int startingDay)
    {
        DateTime date = DateTime.Now;
        int daysToAdd = startingDay;
        DateTime res = date.AddDays(daysToAdd);
        while (res.DayOfWeek.ToString() != "Sunday")
        {
            daysToAdd++;
            res = date.AddDays(daysToAdd);
        }

        return new DateTime(res.Year, res.Month, res.Day, 0, 0, 0);
    }

    //=================================================== Load AR Store Functions:  ===================================================
    private void LoadARStoreElements()
    {
        AppData data = DataManager.LoadOptions();

        //If not first load:
        if (data != null)
        {
            totalScore = data.score;
            ModifyTotalScoreBy(0);
            storeManager.LoadARStoreElements(data);
        }
        else
        {
            ModifyTotalScoreBy(9000);
            storeManager.ARStoreFirstLoad();
        }
 
    }

    // private void ARStoreFirstLoad()
    // {
    //     ModifyTotalScoreBy(9000);
    //     // storeManager.ARStoreFirstLoad();
    // }



    //=======================================================   Scoreboard Functions:  =======================================================  
    public void AddScoreToScoreboard(int newScore)
    {
        ModifyTotalScoreBy(newScore);
        var newPlayer = new Player(playersName, newScore);

        //If tournament has ended update the previous tournament scoreboard, and clear the current one:
        if(DateTime.Now > currTournamentEndDate)
        {
            UpdatePreviousTournamentScoreboard();
        }

        
        try
        {
            // Add score to current tournament scoreboard:
            currentScoreboard.Add(newScore, newPlayer);
            if (currentScoreboard.Count > Definitions.CURRENT_SCOREBOARD_MAX_SIZE)
            {
                currentScoreboard.RemoveAt(Definitions.CURRENT_SCOREBOARD_MAX_SIZE);
            }
            // Add score to the general alltime scoreboard:
            alltimeScoreboard.Add(newScore, newPlayer);
            if (alltimeScoreboard.Count > Definitions.ALLTIME_SCOREBOARD_MAX_SIZE)
            {
                alltimeScoreboard.RemoveAt(Definitions.ALLTIME_SCOREBOARD_MAX_SIZE);
            }

            firebase.database.SetAsync($"{Definitions.DB_ROOT_NAME}/{Definitions.clientName}/{Definitions.CURR_SCOREBOARD_DB_NAME}/", currentScoreboard.Values);
            firebase.database.SetAsync($"{Definitions.DB_ROOT_NAME}/{Definitions.clientName}/{Definitions.ALL_SCOREBOARD_DB_NAME}/", alltimeScoreboard.Values);
        }
        catch (Exception e)
        {
            return;
        }
       
    }


    async public Task<bool> LoadScoreboards()
    {
        await LoadScoreboard(this.previousScoreboard, Definitions.PREV_SCOREBOARD_DB_NAME);
        await LoadScoreboard(this.currentScoreboard, Definitions.CURR_SCOREBOARD_DB_NAME);
        await LoadScoreboard(this.alltimeScoreboard, Definitions.ALL_SCOREBOARD_DB_NAME);

        return true;
    }

    
    async private Task<bool> LoadScoreboard(SortedList<int, Player> scoreboard, string scoreboardName)
    {
        var scoreboardPath = $"/{Definitions.DB_ROOT_NAME}/{Definitions.clientName}/{scoreboardName}/";
        Debug.Log("scoreboardPath: " + scoreboardPath);
        var scoreboardData = await firebase.database.QueryAsync(scoreboardPath);

        if (scoreboardData == null || scoreboardData.ToString().Contains("ERROR"))
        {
            return true;
        }

        List<Player> DBScoreboard = JsonConvert.DeserializeObject<List<Player>>(scoreboardData);

        if (DBScoreboard.Count > 0)
        {
            for (int i = 0; i < DBScoreboard.Count; i++)
            {
                Player player = DBScoreboard[i];
                scoreboard.Add(player.score, player);
            }
        }
        else
        {
            Debug.Log(scoreboardName + " is empty!!!");
        }
        return true;
    }


    public void ModifyTotalScoreBy(int amount)
    {
        totalScore += amount;
        totalScoreLabel.text = totalScore.ToString();
        //StartCoroutine(UpdateScoreAnimation(totalScore - amount, totalScore));
        
    }
}


class DescComparer<T> : IComparer<T>
{
    public int Compare(T x, T y)
    {
        if (x == null) return -1;
        if (y == null) return 1;
        int result = Comparer<T>.Default.Compare(y, x);

        if (result == 0)
            return -1;
        else          
            return result;
    }
}


/*public class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable
{
    #region IComparer<TKey> Members

    public int Compare(TKey x, TKey y)
    {
        int result = x.CompareTo(y);

        if (result == 0)
            return 1; // Handle equality as being greater. Note: this will break Remove(key) or
        else          // IndexOfKey(key) since the comparer never returns 0 to signal key equality
            return result;
    }

    #endregion
}*/


