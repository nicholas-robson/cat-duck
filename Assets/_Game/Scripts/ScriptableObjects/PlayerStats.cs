using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BodyState
{
    Brain,
    Head,
    Torso,
    Arms,
    Legs
}

[System.Serializable]
public class BodyStats
{
    [SerializeField] string name;
    public BodyState bodyState;
    public float jumpForce;
    public float forwardForce;
    public float jumpCooldown;
    public float rotationSpeed;
   
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
