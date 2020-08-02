using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager _instance;
    public Text message_text;
    public GameObject message;  /// message shown when the game has finished
    public GameObject playButton;
    public GameObject pauseButton;
    private List<TeamMate> players = new List<TeamMate>();      /// list of all players
    private List<TeamIdentifier> teams = new List<TeamIdentifier>();    /// list of all teams
    private float playTime;

    void Awake()
    {
        _instance = this;
        _instance.message.SetActive(false);
        _instance.playButton.SetActive(true);
        PauseGame();
        players.AddRange(FindObjectsOfType<TeamMate>());
    }

    #endregion

    void Update()
    {
        playTime += Time.deltaTime;
        players.RemoveAll(item => item == null);
        SearchForTeams();
        ForceTeamEnemy();
        CheckWinningTeam();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        _instance.playButton.SetActive(true);
        _instance.pauseButton.SetActive(false);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        _instance.playButton.SetActive(false);
        _instance.pauseButton.SetActive(true);
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
    
    public void QuitGame()
    {
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    /// <summary>
    /// Find any player of one team and apply a target to its team from any opposing team
    /// </summary>
    public void ForceTeamEnemy()
    {
        foreach(TeamIdentifier team in teams)
        {
            var oppose_players = players.FindAll(player => player.team != team);
            if(oppose_players.Count != 0 && oppose_players != null)
                players.Find(player => player.team == team).SetTeamTarget(oppose_players[Random.Range(0, oppose_players.Count)].transform);
        }
    }

    /// <summary>
    /// Search for all teams amongst players
    /// </summary>
    static void SearchForTeams()
    {
        TeamIdentifier current_team = null;
        _instance.teams.Clear();
        foreach(TeamMate tm in _instance.players)
        {
            current_team = tm.team;
            if(_instance.teams.Find(item => item == current_team) == null)
            {
                _instance.teams.Add(tm.team);
            }
        }
    }

    /// <summary>
    /// Check if only one team left, pause game and show message
    /// </summary>
    static void CheckWinningTeam()
    {
        if(_instance.teams.Count <= 1)
        {
            Time.timeScale = 0;
            if(_instance.teams.Count == 1)
                _instance.message_text.text = "Team " + _instance.teams[0].teamTag + " WON!\r\nPlay Time: " + _instance.playTime + " seconds";
            else
                _instance.message_text.text = "No winning team!\r\nPlay Time: " + _instance.playTime + " seconds";
            _instance.message.SetActive(true);
            _instance.playButton.SetActive(false);
            _instance.pauseButton.SetActive(false);
        }
    }
}
