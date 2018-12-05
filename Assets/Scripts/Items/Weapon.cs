using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;

namespace SA
{
    [CreateAssetMenu(menuName ="Items/Weapon")]
    public class Weapon : Item
    {
        public int currentBullets = 30;
        public int megazineBullets = 30;
        public float fireRate = 0.2f;

        public Vector3Variable rightHandPosition;
        public Vector3Variable rightHandEuler;
        public GameObject modelPrefab;
        public RuntimeWeapon runTime;

        public void Init()
        {
            runTime = new RuntimeWeapon();
            runTime.modelInstance = Instantiate(modelPrefab) as GameObject;
            runTime.weaponHook = runTime.modelInstance.GetComponent<WeaponHook>();
            runTime.weaponHook.Init();
        }

        public class RuntimeWeapon
        {
            public GameObject modelInstance;
            public WeaponHook weaponHook;
        }
    }
}
