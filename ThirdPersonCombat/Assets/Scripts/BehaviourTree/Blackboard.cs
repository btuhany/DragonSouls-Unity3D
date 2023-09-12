using UnityEngine;

[System.Serializable]
public class Blackboard
{
    public float attackTimeCounter = 0f;
    public bool isHit = false;
    public bool playerOnLeft = false; // false means player is on the right
}
