using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName ="Single Instances/Player Refs")]
    public class PlayerReferences : ScriptableObject
    {
        public FloatVariable targetSpread;
        public IntVariable health;
        public IntVariable curAmmo;
        public IntVariable curCarrying;
        public GameEvent e_UpdateUI;

    }
}
