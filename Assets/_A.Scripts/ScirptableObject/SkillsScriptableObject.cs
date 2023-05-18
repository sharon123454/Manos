using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SkillsScriptableObject", menuName = "Actions/BaseAction")]
public class SkillsScriptableObject : ScriptableObject
{
  public float moveSpeed = 4, rotateSpeed = 7.5f;
  public int maxMoveDistance = 3;
}
