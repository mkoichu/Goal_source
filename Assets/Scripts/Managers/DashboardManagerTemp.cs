using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.IO;
using System.Net;

using Tamarin.FirebaseX;
//using Firebase.Firestore;
using Newtonsoft.Json;
using MainDefinitions;

public class DashboardManagerTemp : MonoBehaviour
{
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject tournamentPanel;
    [SerializeField] TMP_InputField storeLinkInput;
    [SerializeField] TMP_InputField clipLinkInput;
    [SerializeField] TMP_InputField timeOfGoalInput;
    [SerializeField] TMP_InputField logoPathInput;
    [SerializeField] TMP_InputField bgPathInput;
    [SerializeField] TMP_InputField celebBGPathInput;
     [SerializeField] TMP_InputField prizeImageInput;

    [SerializeField] TMP_InputField yearInput;
    [SerializeField] TMP_InputField monthInput;
    [SerializeField] TMP_InputField dayInput;
    [SerializeField] TMP_InputField hourInput;
    [SerializeField] TMP_InputField minuteInput;


    [SerializeField] GameObject LogoFileDoesNotExistPopup;
    [SerializeField] GameObject BGFileDoesNotExistPopup;
    [SerializeField] GameObject StoreLinkDoesNotExistPopup;
    [SerializeField] GameObject PrizeImageDoesNotExistPopup;

    public static event Action OnAdminSettingsUpdateEvent;

    FirebaseAPI firebase;
    private int MIN_CLIP_TIME;

    [SerializeField] string DEFAULT_STORE_LINK = "https://www.fanatics.com/soccer-gear/x-555?_ref=p-HP:m-TS:i-r0c6:po-81";
    [SerializeField] string DEFAULT_CLIP_LINK = "https://youtu.be/DZZu2W1z7JE";
    [SerializeField] string DEFAULT_TIME_OF_GOAL = "11";
    [SerializeField] string DEFAULT_LOGO_PATH = "";
    [SerializeField] string DEFAULT_BG_PATH = "";
    [SerializeField] string DEFAULT_CELEB_BG_PATH = "";

    [SerializeField] string DEFAULT_END_OF_TOUR_YEAR = "2021";
    [SerializeField] string DEFAULT_END_OF_TOUR_MONTH = "6";
    [SerializeField] string DEFAULT_END_OF_TOUR_DAY = "27";
    [SerializeField] string DEFAULT_END_OF_TOUR_HOUR = "0";
    [SerializeField] string DEFAULT_END_OF_TOUR_MINUTE = "0";

    private string storeLink = "";
    private string clipLink = "";
    private float timeOfGoal = 0;
    /*private float clipStartTime = 0;
    private float clipEndTime = 0;
    private string logoImagePath = "";
    private string bgImagePath = "";
    private string celebBGImagePath = "";
    private string prizeImagePath = "";*/
    private DateTime currTournamentTargetDate;
    //private DateTime nextTournamentTargetDate;


    void OnEnable()
    {
        Debug.Log("FIREBASE START");
        firebase = GameManager.Instance.firebase; //FirebaseAPI.Instance;

        /* firebase.onInit.AddListener(async () =>
         {
             Debug.Log("FIREBASE INITIALIZED EVENT");*/

        LoadGameSettings();
        InitInputFields();

        //});

    }

    private void InitInputFields()
    {
        logoPathInput.text = PlayerPrefs.GetString("logoImagePath");
        bgPathInput.text = PlayerPrefs.GetString("bgImagePath");
        celebBGPathInput.text = PlayerPrefs.GetString("celebBGImagePath");
        prizeImageInput.text = PlayerPrefs.GetString("prizeImagePath");

    }

     private string RetrunIfNotEmpty(string str, string defaultStr)
     {
        if(str == "")
        {
            return defaultStr;
        }
        return str;
     }

    private string RetrunIfNotZero(float num, string defaultNum)
    {
        if (num <=0)
        {
            return defaultNum;
        }
        return num.ToString();
    }



    async public void UpdateStoreLink()
    {
        if(storeLinkInput.text != "")
        {
            /*if (RemoteFileExists(storeLinkInput.text) == false && !String.IsNullOrEmpty(storeLinkInput.text))
            {
                StoreLinkDoesNotExistPopup.SetActive(true);
                return;
            }*/
            storeLink = storeLinkInput.text;
            firebase.database.SetAsync($"{Definitions.DB_ROOT_NAME}/{Definitions.clientName}/{Definitions.STORE_LINK_DB_NAME}/", storeLink);
            //GameManager.Instance.storeLink = storeLinkInput.text;
        } 
    }

    // async public void UpdateClipLink()
    // { 
    //     if (clipLinkInput.text != "")
    //     {
    //         //TODO: check validity of link
    //         clipLink = clipLinkInput.text;
    //         firebase.database.SetAsync($"{Definitions.DB_ROOT_NAME}/{Definitions.clientName}/{Definitions.CLIP_LINK_DB_NAME}/", clipLink);
    //         GameManager.Instance.clipLink = clipLinkInput.text;
    //     }
    // }         //UNREMOVED FOR DEBUG (youtubePlayer)

    async public void UpdateTimeOfGoal()
    {
        
        if (timeOfGoalInput.text != "")
        {
            timeOfGoal = Int32.Parse(timeOfGoalInput.text);
            //TODO: check validity of link
            firebase.database.SetAsync($"{Definitions.DB_ROOT_NAME}/{Definitions.clientName}/{Definitions.TIME_OF_GOAL_DB_NAME}/", timeOfGoal);
            GameManager.Instance.timeOfGoal = timeOfGoal;
        }
    }

    async public void UpdateCurrTournamentTargetDate()
    {
        int year = Int32.Parse(yearInput.text);
        int month = Int32.Parse(monthInput.text);
        int day = Int32.Parse(dayInput.text);
        int hour = Int32.Parse(hourInput.text);
        int minute = Int32.Parse(minuteInput.text);
        Date dbTargetDate = new Date(year, month, day, hour, minute, 0);
        firebase.database.SetAsync($"{Definitions.DB_ROOT_NAME}/{Definitions.clientName}/{Definitions.CURR_TOURNAMENT_TARGET_DATE}/", dbTargetDate);
    }

    private bool RemoteFileExists(string url)
    {
        try
        {
            //Creating the HttpWebRequest
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            //Setting the Request method HEAD, you can also use GET too.
            request.Method = "HEAD";
            //Getting the Web Response.
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //Returns TRUE if the Status code == 200
            response.Close();
            return (response.StatusCode == HttpStatusCode.OK);
        }
        catch
        {
            //Any exception will returns false.
            return false;
        }
    }
    public void OnApplyClick()
    {
        UpdateStoreLink();
        // UpdateClipLink();          //UNREMOVED FOR DEBUG (youtubePlayer)
        UpdateTimeOfGoal();
        UpdateCurrTournamentTargetDate();

        if (!File.Exists(logoPathInput.text) && !String.IsNullOrEmpty(logoPathInput.text))
        {
            Debug.Log("Logo Path doesn't exitst");
            LogoFileDoesNotExistPopup.SetActive(true);
            return;
        }
        else
        {
            //GameManager.Instance.logoImagePath = logoPathInput.text;
            PlayerPrefs.SetString("logoImagePath", logoPathInput.text);
        }
        

        if (!File.Exists(bgPathInput.text) && !String.IsNullOrEmpty(bgPathInput.text))
        {
            Debug.Log("BG Path doesn't exitst");
            BGFileDoesNotExistPopup.SetActive(true);
            return;
        }
        else
        {
            //GameManager.Instance.bgImagePath = bgPathInput.text;
            PlayerPrefs.SetString("bgImagePath", bgPathInput.text);
        }

        if (!File.Exists(celebBGPathInput.text) && !String.IsNullOrEmpty(celebBGPathInput.text))
        {
            Debug.Log("Celeb BG Path doesn't exitst");
            BGFileDoesNotExistPopup.SetActive(true);
            return;
        }
        else
        {
           // GameManager.Instance.celebBGImagePath = celebBGPathInput.text;
            PlayerPrefs.SetString("celebBGImagePath", celebBGPathInput.text);
        }

         if (!File.Exists(prizeImageInput.text) && !String.IsNullOrEmpty(prizeImageInput.text))
        {
            Debug.Log("Prize Path doesn't exitst");
            PrizeImageDoesNotExistPopup.SetActive(true);
            return;
        }
        else
        {
            //GameManager.Instance.prizeImagePath = prizeImageInput.text;
             PlayerPrefs.SetString("prizeImagePath", prizeImageInput.text);
        }


      
        if (OnAdminSettingsUpdateEvent != null)
        {
            OnAdminSettingsUpdateEvent();
        }

        menuPanel.SetActive(true);
        tournamentPanel.SetActive(true);
        this.gameObject.SetActive(false);
    }

    async private void LoadGameSettings()
    {
        LoadStoreLink();
        LoadClipLink();
        LoadTimeOfGoal();
        LoadCurrTournamentTargetDate();
    }

    async private void LoadStoreLink()
    {
        var storeLinkPath = $"/{Definitions.DB_ROOT_NAME}/{Definitions.clientName}/{Definitions.STORE_LINK_DB_NAME}/";
        var storeLinkData = await firebase.database.QueryAsync(storeLinkPath);
        if (!storeLinkData.ToString().Contains("ERROR"))
        {
            storeLink = JsonConvert.DeserializeObject<string>(storeLinkData);
        }
        storeLinkInput.text = RetrunIfNotEmpty(storeLink, DEFAULT_STORE_LINK);
       

    }


    async private void LoadClipLink()
    {
        var clipLinkPath = $"/{Definitions.DB_ROOT_NAME}/{Definitions.clientName}/{Definitions.CLIP_LINK_DB_NAME}/";
        var clipLinkData = await firebase.database.QueryAsync(clipLinkPath);
        if (!clipLinkData.ToString().Contains("ERROR"))
        {
            clipLink = JsonConvert.DeserializeObject<string>(clipLinkData);
        }
        clipLinkInput.text = RetrunIfNotEmpty(clipLink, DEFAULT_CLIP_LINK); 
    }

    async private void LoadTimeOfGoal()
    {
        var timeOfGoalPath = $"/{Definitions.DB_ROOT_NAME}/{Definitions.clientName}/{Definitions.TIME_OF_GOAL_DB_NAME}/";
        var timeOfGoalData = await firebase.database.QueryAsync(timeOfGoalPath);
        if (!timeOfGoalData.ToString().Contains("ERROR"))
        {
            timeOfGoal = JsonConvert.DeserializeObject<float>(timeOfGoalData);
        }
        timeOfGoalInput.text = RetrunIfNotZero(timeOfGoal, DEFAULT_TIME_OF_GOAL);
    }


    async public void LoadCurrTournamentTargetDate()
    {
        var path = $"/{Definitions.DB_ROOT_NAME}/{Definitions.clientName}/{Definitions.CURR_TOURNAMENT_TARGET_DATE}/";
        var data = await firebase.database.QueryAsync(path);
        if (!data.ToString().Contains("ERROR"))
        {
            Date dbTargetDate = JsonConvert.DeserializeObject<Date>(data);
            DateTime dtTargetDate;
            if (dbTargetDate != null)
            {
                dtTargetDate = new DateTime(dbTargetDate.year, dbTargetDate.month, dbTargetDate.day, dbTargetDate.hour, dbTargetDate.minute, dbTargetDate.second);
                currTournamentTargetDate = dtTargetDate;

                yearInput.text = currTournamentTargetDate.Year.ToString();
                monthInput.text = currTournamentTargetDate.Month.ToString();
                dayInput.text = currTournamentTargetDate.Day.ToString();
                hourInput.text = currTournamentTargetDate.Hour.ToString();
                minuteInput.text = currTournamentTargetDate.Minute.ToString();

                return;
            } 
        }

        yearInput.text = DEFAULT_END_OF_TOUR_YEAR;
        monthInput.text = DEFAULT_END_OF_TOUR_MONTH;
        dayInput.text = DEFAULT_END_OF_TOUR_DAY;
        hourInput.text = DEFAULT_END_OF_TOUR_HOUR;
        minuteInput.text = DEFAULT_END_OF_TOUR_MINUTE;
    }

    private bool CheckInput()
    {
        if (!CheckGoalTime())
        {
            Debug.Log("Goal Time is invalid!");
            return false;
        }
        return true;
    }

    private bool CheckGoalTime()
    {
        if (timeOfGoal < MIN_CLIP_TIME)
        {
            return false;
        }
        return true;
    }

    /*public void QuitGame()
    {
        Application.Quit();
    }

    private bool CheckClipTimes()
    {
        if (clipStartTime < 0 || clipEndTime < 0)
        {
            Debug.Log("Invalid clip start time!");
            return false;
        }

        if (clipEndTime <= clipStartTime)
        {
            Debug.Log("Invalid clip end time!");
            return false;
        }
        if ((clipEndTime - clipStartTime) < MIN_CLIP_TIME + 2)
        {
            Debug.Log("Clip must be at least " + MIN_CLIP_TIME + 2 + " seconds long!");
            return false;
        }
        return true;
    }*/

}
