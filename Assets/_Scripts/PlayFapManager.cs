using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class PlayFapManager : MonoBehaviour
{
    [Header("Leaderboard")]
    public GameObject rowPrefap;
    public Transform rowsParent;

    [Header("Error Handling")]
    public GameObject errorPopUp;
    public TextMeshProUGUI errorMessage;

    [Header("")]
    public TextMeshProUGUI loginInfoText;

    public int playerStatisticValue = 0;

	private const string titleID = "CCE98";

	private void Start()
    {
        errorPopUp.SetActive(false);
        GetLeaderBoard();
        GetAccountInfo();
        GetLeaderboardValueOfPlayer();
	}

	void OnError(PlayFabError error)
	{
		Debug.Log("OnError Triggers: "+error.GenerateErrorReport());
        errorPopUp.SetActive(true);
        errorMessage.text = "Error: "+error.GenerateErrorReport();
	}

    public void HideError()
    {
        errorPopUp.SetActive(false);
    }

	void GetAccountInfo()
	{
		GetAccountInfoRequest request = new GetAccountInfoRequest();
		PlayFabClientAPI.GetAccountInfo(request, Successs, OnError);
	}

	void Successs(GetAccountInfoResult result)
	{
		loginInfoText.text = "Welcome "+  result.AccountInfo.Username;
	}

	public void SendLeaderBoard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest { 
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "Scoreboard",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderBoardUpdate, OnError);
    }

    void OnLeaderBoardUpdate(UpdatePlayerStatisticsResult result)
    {
        Invoke(nameof(GetLeaderBoard), 2f);
    }

    public void GetLeaderBoard() {
        var request = new GetLeaderboardRequest { 
            StatisticName = "Scoreboard",
            StartPosition = 0,
            MaxResultsCount= 5,
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderBoardGet, OnError);
    }

	void OnLeaderBoardGet(GetLeaderboardResult result)
    {
        foreach (Transform item in rowsParent)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in result.Leaderboard)
        {
			GameObject newGo = Instantiate(rowPrefap, rowsParent);
			TextMeshProUGUI[] texts = newGo.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = (item.Position +1).ToString()+ ".";
			texts[1].text = item.DisplayName;
			texts[2].text = item.StatValue.ToString();
		}
	}

	void GetLeaderboardValueOfPlayer()
	{
		List<string> list = new List<string>();
		list.Add("Scoreboard");
		var request = new GetPlayerStatisticsRequest
		{
			StatisticNames = list,
		};
		PlayFabClientAPI.GetPlayerStatistics(request, OnGetPlayerStatisticsSuccess, OnError);
	}

	void OnGetPlayerStatisticsSuccess(GetPlayerStatisticsResult result)
	{
		foreach (var res in result.Statistics)
		{
            playerStatisticValue = res.Value;
            Debug.Log(playerStatisticValue);
		}
	}


}
