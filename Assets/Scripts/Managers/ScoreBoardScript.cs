using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MainDefinitions;

public class ScoreBoardScript : MonoBehaviour
{
    [Header("Scroll Views Contents:")]
    [SerializeField] private GameObject PreviousScoreboardContent;
    [SerializeField] private GameObject CurrentScoreboardContent;
    [SerializeField] private GameObject AlltimeScoreboardContent;

    [Header("Entry Prefab:")]
    [SerializeField] private GameObject entryPrefab;


    void OnEnable()
    {
       // GameManager.Instance.LoadScoreboards();
        InitCurrentScoreboard();
    }


    public void InitPreviousScoreboard()
    {
        UIManager.Instance.InitializePreviousScoreboardUI();
        InitializeScores(PreviousScoreboardContent, GameManager.Instance.previousScoreboard);
    }


    public void InitCurrentScoreboard()
    {
        UIManager.Instance.InitializeCurrentScoreboardUI();
        InitializeScores(CurrentScoreboardContent, GameManager.Instance.currentScoreboard);


    }

    public void InitAlltimeScoreboard()
    {
        UIManager.Instance.InitializeAlltimeUI();
        InitializeScores(AlltimeScoreboardContent, GameManager.Instance.alltimeScoreboard);

    }

    
    private void InitializeScores(GameObject outputScoreboardContent, SortedList<int, Player> inputScoreboard)
    {
        ClearScoreboard(outputScoreboardContent);
        for (int i = 0; i < inputScoreboard.Count; i++)
        {
            Player player = inputScoreboard.Values[i];
            GameObject newEntry = Instantiate(entryPrefab, outputScoreboardContent.transform);

            newEntry.gameObject.transform.Find("Number").GetComponent<TextMeshProUGUI>().text = (i + 1).ToString() + ".";
            newEntry.gameObject.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = player.name.ToString();
            newEntry.gameObject.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = player.score.ToString();
            newEntry.SetActive(true);
        }
    }


    void ClearScoreboard(GameObject scoreboard)
    {
        foreach(Transform child in scoreboard.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
