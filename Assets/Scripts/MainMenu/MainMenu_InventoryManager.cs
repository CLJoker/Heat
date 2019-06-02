using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class MainMenu_InventoryManager : MonoBehaviour
    {
        public Transform[] cameraPosition;
        public int camIndex;

        public new Transform camera;
        float t;
        public float speed = 5;
        Vector3 startPos;
        Vector3 endPos;
        Quaternion startRot;
        Quaternion endRot;
        bool isInit;
        bool isLerping;
        [HideInInspector]
        public CategorySelection currentCategory = CategorySelection.None;

        List<ClothItem> cloths;
        int curCloth;
        List<Weapon> weapons;
        int curWeapon;
        int curSubWeapon;

        public StateManager states;

        public Transform weaponParent;
        GameObject weaponObj;
        public Transform subWeaponParent;
        GameObject subWeaponObj;

        public enum CategorySelection
        {
            None, Cloth, Weapon, SubWeapon
        }

        private void OnEnable()
        {
            camera.position = cameraPosition[0].position;
            camera.rotation = cameraPosition[0].rotation;
            currentCategory = CategorySelection.None;

            cloths = GameManagers.GetResourcesManager().GetAllCloth();
            weapons = GameManagers.GetResourcesManager().GetAllWeapon();

            InitItemsIndex();
            if (weaponObj != null)
            {
                Destroy(weaponObj);
            }
            weaponObj = CreateWeapon(weapons[curWeapon], weaponParent);
            if (subWeaponObj != null)
            {
                Destroy(subWeaponObj);                
            }
            subWeaponObj = CreateWeapon(weapons[curSubWeapon], subWeaponParent);
        }

        private void Update()
        {
            MoveCameraToPosition();
        }

        public void AssignCameraPosition(int index)
        {
            switch (index)
            {
                case 0:
                    currentCategory = CategorySelection.None;
                    break;
                case 1:
                    currentCategory = CategorySelection.Weapon;
                    break;
                case 2:
                    currentCategory = CategorySelection.SubWeapon;
                    break;
                case 3:
                    currentCategory = CategorySelection.Cloth;
                    break;
                default:
                    break;
            }

            camIndex = index;
            isLerping = true;
            isInit = false;
        }

        void MoveCameraToPosition()
        {
            if (!isLerping)
                return;

            if (!isInit)
            {
                startPos = camera.position;
                endPos = cameraPosition[camIndex].position;
                startRot = camera.rotation;
                endRot = cameraPosition[camIndex].rotation;
                isInit = true;
                t = 0;
            }

            t += Time.deltaTime * speed;
            if(t > 1)
            {
                t = 1;
                isInit = false;
                isLerping = false;

            }

            Vector3 tp = Vector3.Lerp(startPos, endPos, t);
            Quaternion tr = Quaternion.Slerp(startRot, endRot, t);

            camera.position = tp;
            camera.rotation = tr;
        }

        public void PreviousItem()
        {
            switch (currentCategory)
            {
                case CategorySelection.Cloth:
                    curCloth -= 1;
                    if(curCloth < 0)
                    {
                        curCloth = cloths.Count - 1;
                    }
                    GameManagers.GetPlayerProfile().modelId = cloths[curCloth].name;
                    states.LoadCharacterModel(cloths[curCloth].name);
                    break;
                case CategorySelection.Weapon:
                    curWeapon -= 1;
                    if(curWeapon < 0)
                    {
                        curWeapon = weapons.Count - 1;
                    }
                    GameManagers.GetPlayerProfile().itemIds[0] = weapons[curWeapon].name;
                    if(weaponObj != null)
                    {
                        Destroy(weaponObj);
                        weaponObj = CreateWeapon(weapons[curWeapon], weaponParent);
                    }
                    break;
                case CategorySelection.SubWeapon:
                    curSubWeapon -= 1;
                    if (curSubWeapon < 0)
                    {
                        curSubWeapon = weapons.Count - 1;
                    }
                    GameManagers.GetPlayerProfile().itemIds[1] = weapons[curSubWeapon].name;
                    if (subWeaponObj != null)
                    {
                        Destroy(subWeaponObj);
                        subWeaponObj = CreateWeapon(weapons[curSubWeapon], subWeaponParent);
                    }
                    break;
                default:
                    break;
            }
        }

        public void NextItem()
        {
            switch (currentCategory)
            {
                case CategorySelection.Cloth:
                    curCloth += 1;
                    if (curCloth > cloths.Count - 1)
                    {
                        curCloth = 0;
                    }
                    GameManagers.GetPlayerProfile().modelId = cloths[curCloth].name;
                    states.LoadCharacterModel(cloths[curCloth].name);
                    break;
                case CategorySelection.Weapon:
                    curWeapon += 1;
                    if (curWeapon > weapons.Count - 1)
                    {
                        curWeapon = 0;
                    }
                    GameManagers.GetPlayerProfile().itemIds[0] = weapons[curWeapon].name;
                    if (weaponObj != null)
                    {
                        Destroy(weaponObj);
                        weaponObj = CreateWeapon(weapons[curWeapon], weaponParent);
                    }
                    break;
                case CategorySelection.SubWeapon:
                    curSubWeapon += 1;
                    if (curSubWeapon > weapons.Count - 1)
                    {
                        curSubWeapon = 0;
                    }
                    GameManagers.GetPlayerProfile().itemIds[1] = weapons[curSubWeapon].name;
                    if (subWeaponObj != null)
                    {
                        Destroy(subWeaponObj);
                        subWeaponObj = CreateWeapon(weapons[curSubWeapon], subWeaponParent);
                    }
                    break;
                default:
                    break;
            }
        }

        #region Init Item Index
        private void InitItemsIndex()
        {
            //Load data from local, if exist
            PlayerDataBin playerData = DataManager.LoadLocalPlayer();

            if(playerData != null)
            {
                PlayerProfile playerProfile = GameManagers.GetPlayerProfile();
                playerProfile.playerName = playerData.playerName;
                playerProfile.modelId = playerData.modelId;
                playerProfile.itemIds[0] = playerData.mainWeapon;
                playerProfile.itemIds[1] = playerData.subWeapon;
                playerProfile.avatarData = playerData.avatar;
                Debug.Log("Load successfull");
            }
            else
            {
                Debug.Log("No save file to load");
            }

            InitClothIndex();
            InitWeaponIndex();
            InitSubWeaponIndex();
        }

        private void InitClothIndex()
        {
            string modelId = GameManagers.GetPlayerProfile().modelId;

            for(int i = 0; i < cloths.Count; i++)
            {
                if(string.Equals(modelId, cloths[i].name))
                {
                    curCloth = i;
                    break;
                }
            }
        }

        private void InitWeaponIndex()
        {
            string weapon = GameManagers.GetPlayerProfile().itemIds[0];

            for (int i = 0; i < weapons.Count; i++)
            {
                if (string.Equals(weapon, weapons[i].name))
                {
                    curWeapon = i;
                    break;
                }
            }
        }

        private void InitSubWeaponIndex()
        {
            string subWeapon = GameManagers.GetPlayerProfile().itemIds[1];

            for (int i = 0; i < weapons.Count; i++)
            {
                if (string.Equals(subWeapon, weapons[i].name))
                {
                    curSubWeapon = i;
                    break;
                }
            }
        }
        #endregion

        GameObject CreateWeapon(Weapon w, Transform p)
        {
            GameObject go = Instantiate(w.modelPrefab, p.position, p.rotation, p);
            return go;
        }

        public void SavePlayerProfile()
        {
            PlayerDataBin playerData = new PlayerDataBin();
            PlayerProfile playerProfile = GameManagers.GetPlayerProfile();
            playerData.playerName = playerProfile.playerName;
            playerData.modelId = playerProfile.modelId;
            playerData.mainWeapon = playerProfile.itemIds[0];
            playerData.subWeapon = playerProfile.itemIds[1];
            DataManager.SavePlayer(playerData);
            Debug.Log("Save Player!!!");
        }

    }
}
