using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Weapons/ Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        public string id;

        public IKPositions m_h_ik;
        public GameObject modelPrefab;

        public float fireRate = 0.1f;
        public int megazineAmmo = 30;
        public int maxAmmo = 160;
        public bool onIdleDisableOh;
        public float WeaponType;

        public AnimationCurve recoilY;
        public AnimationCurve recoilZ;
    }
}
