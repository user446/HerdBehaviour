using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamMate : MonoBehaviour
{
    public TeamIdentifier team;
    public List<TeamMate> teamMates = new List<TeamMate>();

    void Start()
    {
        List<TeamMate> players = new List<TeamMate>();
        players.AddRange(FindObjectsOfType<TeamMate>());
        teamMates.AddRange(players.FindAll(x => x.team.Equals(this.team)));
    }

    void Update()
    {
        ClearMissingTeamMates();
    }

    /// <summary>
    /// Clear all missing team mates
    /// </summary>
    void ClearMissingTeamMates()
    {
        teamMates.RemoveAll(item => item == null);
    }

    /// <summary>
    /// Apply target for all team mates without target
    /// </summary>
    /// <param name="target">Target transform</param>
    public void SetTeamTarget(Transform target)
    {
        foreach(TeamMate tm in teamMates)
        {
            if(tm != null)
            {
                var tm_playerBase = tm.GetComponent<PlayerBase>();
                if(tm_playerBase != null)
                {
                    if(tm_playerBase.GetTarget() == null)
                        tm_playerBase.SetTarget(target);
                }
            }
        }
    }

    public void AssignTeam(TeamIdentifier teamToAssign)
    { 
        team = teamToAssign;
    }

}
