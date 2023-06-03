using Unity.Services.Leaderboards;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;

public class ScoreManager : MonoBehaviour
{
    public Color firstPlace;
    public Color secondPlace;
    public Color thirdPlace;

    public GameObject scorePrefab;
    public Transform boardContent;
    public Transform currentPlayerScore;

    private const string leaderboardId = "ProjectOR";

    private async void Awake()
    {
        if (UnityServices.State == ServicesInitializationState.Uninitialized)
        {
            await UnityServices.InitializeAsync();
        }

        try
        {
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
        }
        catch (AuthenticationException e)
        {
            Debug.LogWarning(e);
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        ShowLeaderboard();
    }

    public static async void UploadScore(int score)
    {
        if (UnityServices.State == ServicesInitializationState.Uninitialized)
        {
            await UnityServices.InitializeAsync();
        }

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardId, score);
    }


    private async void ShowLeaderboard()
    {
        // Get player score
        Unity.Services.Leaderboards.Models.LeaderboardEntry playerScore = await LeaderboardsService.Instance
            .GetPlayerScoreAsync(leaderboardId);

        // Get all scores
        Unity.Services.Leaderboards.Models.LeaderboardScoresPage scores = await LeaderboardsService.Instance
            .GetScoresAsync(leaderboardId);

        // ScoreUI has 2 children: name and value, each with one TMP_Text component
        TMPro.TMP_Text[] scoreTexts;

        for (int i = 0; i < scores.Total; i++)
        {
            scoreTexts = Instantiate(scorePrefab, boardContent).GetComponentsInChildren<TMPro.TMP_Text>();

            scoreTexts[0].text = scores.Results[i].Rank + 1 + ".   " + scores.Results[i].PlayerName;
            scoreTexts[1].text = scores.Results[i].Score.ToString();

            // First place color and bold
            if (i == 0)
            {
                scoreTexts[0].color = firstPlace;
                scoreTexts[1].color = firstPlace;
                scoreTexts[0].fontStyle = TMPro.FontStyles.Bold;
                scoreTexts[1].fontStyle = TMPro.FontStyles.Bold;
            }
            // Second place color
            else if (i == 1){
                scoreTexts[0].color = secondPlace;
                scoreTexts[1].color = secondPlace;
            }
            // Third place
            else if (i == 2)
            {
                scoreTexts[0].color = thirdPlace;
                scoreTexts[1].color = thirdPlace;
            }
        }

        scoreTexts = currentPlayerScore.GetComponentsInChildren<TMPro.TMP_Text>();
        if (playerScore != null)
        {
            scoreTexts[0].text = playerScore.Rank + 1 + ".  " + playerScore.PlayerName;
            scoreTexts[1].text = playerScore.Score.ToString();
        }
    }
}