using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class CountdownScript : MonoBehaviour
{


    [SerializeField] private TextMeshProUGUI uiTimer;
    [SerializeField] private ScoreBoardScript scoreboardScript;
    private DateTime targetDate;
    private bool targetDate_isSet = false;
    private bool calculating = false;


    //[SerializeField] private float startTime;
    private float currTime;
    private bool enable_tour_panel = false;

    // Start is called before the first frame update
    private void Start()
    {
        //UIManager.Instance.DisableTournament();
        /*DateTime date = DateTime.Now;
        var dayOfWeek = date.DayOfWeek;
        var seconds = date.Second.ToString();
        var minutes = (date.Minute+3).ToString();
        if (int.Parse(minutes) < 10) minutes = "0" + minutes;

        var hours = date.Hour.ToString();

        var day = date.Day.ToString();
        var month = date.Month.ToString();
        var year = date.Year.ToString();*/

    }

    // Update is called once per frame
    void Update()
    {
        var tourMode = GameManager.Instance.tourMode;
        if (tourMode != null && tourMode != MainDefinitions.TourModes.None)
        {
            TimeSpan timeToStart = GameManager.Instance.currTournamentStartDate - DateTime.Now;
            bool tour_began = ((float)(timeToStart).TotalSeconds < 0);
            //Debug.Log("COUNT DOWN - Not null/None");
            if (tour_began)
            {
                //Debug.Log("COUNT DOWN - Tour began");
                if (enable_tour_panel == false)
                {
                    //Debug.Log("COUNT DOWN - enable tour panel");
                    UIManager.Instance.EnableTournament();
                    enable_tour_panel = true;
                }
                TimeSpan timeToEnd = GameManager.Instance.currTournamentEndDate - DateTime.Now;
                bool tour_ended = ((float)(timeToEnd).TotalSeconds <= 0);
                
                if (tour_ended)
                {
                   // Debug.Log("COUNT DOWN - tour ended");
                    UIManager.Instance.DisableTournament();
                    enable_tour_panel = false;
                    //GameManager.Instance.UpdatePreviousTournamentScoreboard();
                    if (calculating == false)
                    {
                        //Debug.Log("COUNT DOWN - load new tournament");
                        GameManager.Instance.LoadNewTournament();
                        calculating = true;
                    }
                }
                else
                {
                    //Debug.Log("COUNT DOWN - show countdown");
                    calculating = false;

                    var daysLeft = Mathf.Floor((float)timeToEnd.TotalDays);
                    if (daysLeft < 0) { daysLeft = 0; }

                    var hoursleft = Mathf.Floor((float)timeToEnd.TotalHours - (24 * daysLeft));
                    if (hoursleft < 0) { hoursleft = 0; }

                    var minutesLeft = Mathf.Floor((float)timeToEnd.TotalMinutes - (24 * 60 * daysLeft) - (60 * hoursleft));
                    if (minutesLeft < 0) { minutesLeft = 0; }

                    var secondsLeft = Mathf.Floor((float)timeToEnd.TotalSeconds - (24 * 60 * 60 * daysLeft) - (60 * 60 * hoursleft) - (60 * minutesLeft));
                    if (secondsLeft < 0) { secondsLeft = 0; }

                    uiTimer.text = daysLeft + "d " + hoursleft + "h " + minutesLeft + "m " + secondsLeft + "s";
                }
            }
        }
    }


    private string TimeSpanToString(TimeSpan time)
    {
        string t = "Days: " + Mathf.Floor((float)time.TotalDays) +
                    " Hours: " + Mathf.Floor((float)time.TotalHours) +
                    " Minutes: " + Mathf.Floor((float)time.TotalMinutes) +
                    " Seconds: " + Mathf.Floor((float)time.TotalSeconds);
        return t;
    }
}
