using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Single Instances/Resources")]
    public class ResourcesManager : ScriptableObject
    {
        public RuntimeReferences runtime;
        public Weapon[] all_weapons;
        Dictionary<string, int> w_dict = new Dictionary<string, int>();

        public MeshContainer[] meshContainers;
        Dictionary<string, int> m_dict = new Dictionary<string, int>();

        public CharObject[] charObjects;
        Dictionary<string, int> c_dict = new Dictionary<string, int>();

        public Mask[] masks;
        Dictionary<string, int> mask_dict = new Dictionary<string, int>();

        public void Init()
        {
            InitWeapons();
            InitMeshContainers();
            InitMaskDictionaries();
        }

        void InitWeapons()
        {
            w_dict.Clear();
            for (int i = 0; i < all_weapons.Length; i++)
            {
                if (w_dict.ContainsKey(all_weapons[i].id))
                {

                }
                else
                {
                    w_dict.Add(all_weapons[i].id, i);
                }
            }
        }

        void InitMeshContainers()
        {
            m_dict.Clear();
            for (int i = 0; i < meshContainers.Length; i++)
            {
                if (m_dict.ContainsKey(meshContainers[i].id))
                {

                }
                else
                {
                    m_dict.Add(meshContainers[i].id, i);
                }
            }
        }

        void InitCharacterObjects()
        {
            c_dict.Clear();
            for (int i = 0; i < charObjects.Length; i++)
            {
                if (c_dict.ContainsKey(charObjects[i].id))
                {

                }
                else
                {
                    c_dict.Add(charObjects[i].id, i);
                }
            }
        }

        void InitMaskDictionaries()
        {
            mask_dict.Clear();
            for (int i = 0; i < masks.Length - 1; i++)
            {
                if (mask_dict.ContainsKey(masks[i].obj.id))
                {

                }
                else
                {
                    mask_dict.Add(masks[i].obj.id, i);
                }
            }
        }

        public Mask GetMask(string id)
        {
            Mask retVal = null;
            int index = -1;

            if (mask_dict.TryGetValue(id, out index))
            {
                retVal = masks[index];
            }

            return retVal;
        }

        public MeshContainer GetMesh(string id)
        {
            MeshContainer retVal = null;
            int index = -1;

            if (m_dict.TryGetValue(id, out index))
            {
                retVal = meshContainers[index];
            }

            return retVal;
        }

        public Weapon GetWeapon(string id)
        {
            Weapon retVal = null;
            int index = -1;

            if(w_dict.TryGetValue(id, out index))
            {
                retVal = all_weapons[index];
            }

            return retVal;
        }
    }



    public enum MyBones
    {
        head,
        chest,
        eyebrows,
        rightHand,
        leftHand,
        rightUpperLeg,
        hips
    }
}
