using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TeamIdentifier")]
public class TeamIdentifier : ScriptableObject
{
    public List<TeamIdentifier> enemyTeams = new List<TeamIdentifier>();
    public string teamTag;
}
