using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIBehaivors : MonoBehaviour
{
    [Range(0,100)]
    [SerializeField] private int shootValue;
    [Range(0, 100)]
    [SerializeField] private int moveValue;
    [Range(0, 100)]
    [SerializeField] private int dashValue;
    [Range(0, 100)]
    [SerializeField] private int meleeValue;

    public int GetShootValue() { return shootValue; }
    public int GetMoveValue() { return moveValue; }
    public int GetDashValue() { return dashValue; }
    public int GetMeleeValue() { return meleeValue; }
}
