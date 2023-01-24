using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIBehaivors : MonoBehaviour
{
    [Range(0,100)]
    [SerializeField]private int shootValue;
    [Range(0, 10)]
    [SerializeField] private int walkValue;
    [Range(0, 200)]
    [SerializeField] private int meleeValue;

    public int GetShootValue() { return shootValue; }
    public int GetWalkValue() { return walkValue; }
    public int GetMeleeValue() { return meleeValue; }
}
