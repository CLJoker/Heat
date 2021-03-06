﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName ="Single Instances/Player Refs")]
    public class PlayerReferences : ScriptableObject
    {
        [Header("HUD")]
        public IntVariable health;
        public IntVariable curAmmo;
        public IntVariable curCarrying;

        [Header("States")]
        public BoolVariable isAiming;
        public BoolVariable isLeftPivot;
        public BoolVariable isCrouching;

        [Header("UI")]
        public FloatVariable targetSpread;
        public GameEvent e_UpdateUI;

        public void Init()
        {
            curAmmo.value = 0;
            curCarrying.value = 0;
            health.value = 100;
            isAiming.value = false;
            isLeftPivot.value = false;
            isCrouching.value = false;
            targetSpread.value = 30;
        }

    }
}
