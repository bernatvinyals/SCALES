using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum GameplayFlags
{
    IGNORE_BULLETS = 1 << 0,
    GROUND_BULLETS = 1 << 1,
    TRANSITION_REQUIRED = 1 << 2,
}
public class GameplayObject : MonoBehaviour
{
    public GameplayFlags flags = GameplayFlags.IGNORE_BULLETS;
    //Check flags like:: (enumValue & (MyEnum.C | MyEnum.D | MyEnum.F)
}
