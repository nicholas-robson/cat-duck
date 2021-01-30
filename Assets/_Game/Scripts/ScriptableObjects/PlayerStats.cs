using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BodyState
{
    Brain,
    Body
}

[System.Serializable]
public class BodyStats
{
    [SerializeField] string name;
    public BodyState bodyState;
    public float forwardForce;
    public float soundCooldown;
   
}

[CreateAssetMenu(fileName = "New Player Stats", menuName = "Player/Stats")]
public class PlayerStats : ScriptableObject
{
    public BodyState state;
    public List<BodyStats> statsList;

    public BodyStats GetCurrentStats()
    {
        return statsList.Find(element => element.bodyState == state);
    }


    void OnEnable()
    {
        state = BodyState.Brain;
    }

}
