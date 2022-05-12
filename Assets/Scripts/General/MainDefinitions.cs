using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MainDefinitions
{
    public enum PossibleGameFlowStates {
        None = 0,
        UnityLoaded = 1,
        FirebaseLoaded = 2,
        MicrophoneLoaded = 3,
        UIManagerLoaded = 4,
        GameLoaded = 5
    }

    public class GameFlowStates{
        static public PossibleGameFlowStates state = PossibleGameFlowStates.None;
    }

    public class Definitions
    {
        //Firebase paths:
        static public string DB_ROOT_NAME = "Clients";
        static public string clientName = "WEBGL_test";
        //static public string clientName = "MICHAEL_2PLpjsu8VjNOYOGB92zIxULHCi92";
        static public string PREV_SCOREBOARD_DB_NAME = "Scoreboard_previous";
        static public string CURR_SCOREBOARD_DB_NAME = "Scoreboard_current";
        static public string ALL_SCOREBOARD_DB_NAME = "Scoreboard_alltime";
        static public string CURR_TOURNAMENT_TARGET_DATE = "currTourEnd";
        static public string STORE_LINK_DB_NAME = "storeURL";
        static public string CLIP_LINK_DB_NAME = "clipURL";
        static public string TIME_OF_GOAL_DB_NAME = "goalTime";
        static public string MAIN_BG_IMAGE = "mainBGImage";
        static public string CELEB_BG_IMAGE = "celebBGImage";
        static public string LOGO_1_IMAGE = "logo1Image";
        static public string LOGO_2_IMAGE = "logo2Image";
        static public string LOGO_3_IMAGE = "logo3Image";
        static public string PRIZE_IMAGE = "prizeImage";
        static public string PRIZE_DESC = "prizeDesc";



        //General defintions:
        static public int GAME_OVER_DELAY = 2;          //Seconds
        static public int END_GAME_IF_SILENT_AFTER = 7; //Seconds
        static public int TURN_ON_MIC_BEFORE_GOAL = 2;  //Seconds
        static public int COUNTDOWN_ANIMATION_LENGTH = 5; //Seconds
        static public int INIT_COIN_AMOUNT = 9000;      //Coins
        static public int SAFETY_DELAY_FRAMES = 5;      //Franes
        static public int SCORE_COEFICIENT = 1000;
        static public float VOLUME_DECREASE_RATE = 0.01f;


        //Player definitions:
        static public string DEFAULT_NAME = "GUEST";
        static public string DEFAULT_EMAIL = "default@gmail.com";

        //Dashboard definitions:
        static public string DEFAULT_FOOTAGE_LINK = "https://youtu.be/DZZu2W1z7JE";

        //Scoreboard definitions
        static public int PREVIOUS_SCOREBOARD_MAX_SIZE = 10;  //Rows
        static public int CURRENT_SCOREBOARD_MAX_SIZE = 10;   //Rows
        static public int ALLTIME_SCOREBOARD_MAX_SIZE = 10;   //Rows
    }

    public class Content
    {
        public Content(string clipURL, float goalTime, 
                       DBImage mainBGImage, DBImage celebBGImage, DBImage logo1Image, DBImage logo2Image, DBImage logo3Image,
                       DBImage prizeImage, string prizeDesc, TourModes tourMode, Date tourStart, Date tourEnd, string storeURL)
        {
            //Footage:
            this.clipURL = clipURL;
            this.goalTime = goalTime;

            //UI:
            this.mainBGImage = mainBGImage;
            this.celebBGImage = celebBGImage;
            this.logo1Image = logo1Image;
            this.logo2Image = logo2Image;
            this.logo3Image = logo3Image;

            //Tournaments:
            this.prizeImage = prizeImage;
            this.prizeDesc = prizeDesc;
            this.tourMode = tourMode;
            this.tourStart = tourStart;
            this.tourEnd = tourEnd;

            //Sales:
            this.storeURL = storeURL;
        }
        public string clipURL { get; private set; }
        public float goalTime { get; private set; }
        public DBImage mainBGImage { get;  set; }
        public DBImage celebBGImage { get;  set; }
        public DBImage logo1Image { get;  set; }
        public DBImage logo2Image { get;  set; }
        public DBImage logo3Image { get;  set; }
        public DBImage prizeImage { get; private set; }
        public string prizeDesc { get; set; }
        public TourModes tourMode { get; private set; }
        public Date tourStart { get; private set; }
        public Date tourEnd { get; private set; }
        public string storeURL { get; private set; }
    }

    public class DBImage
    {
        public DBImage(string name, string url)
        {
            this.name = name;
            this.url = url;
        }

        public string name { get; set; }
        public string url { get; set; }

    }

    public class Player
    {
        //private string name;
        //private int score;

        public Player(string newName, int newScore, string newEmail = "-")
        {
            this.name = newName;
            this.score = newScore;
            this.email = newEmail;
        }

        public string name { get; set; }
        public int score { get; set; }
        public string email { get; set; }
    }

    public class Date
    {
        public Date()
        {
        }
        public Date(int year, int month, int day, int hour, int minute, int second)
        {
            this.year = year;
            this.month = month;
            this.day = day;
            this.hour = hour;
            this.minute = minute;
            this.second = second;
        }

        public int year { get; set; }
        public int month { get; set; }
        public int day { get; set; }
        public int hour { get; set; }
        public int minute { get; set; }
        public int second { get; set; }
    }

    public enum ARTimeEffectType
    {
        None = 0,
        VeinPop = 1,
        FireEyes = 2,
        Lightning = 3,
        SuperSaiyan = 4
    }

    public enum ARVolumeEffectType
    {
        None = 0,
        SmallSteam = 1,
        MediumSteam = 2,
        LargeSteam = 3
    }

    public enum TourModes
    {
        None = 0,
        EveryWeek = 1,
        EveryMonth = 2,
        SchedTour = 3
    }
}
