using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class Character : MonoBehaviour
    {
        public Animator anim;

        public string outfitID;

        public CharObject hairObj;
        public CharObject eyebrowsObjs;
        public CharObject other;
        public Mask maskObj;

        GameObject hair;
        GameObject eyebrows;
        GameObject mask;

        public bool isFemale;
        public SkinnedMeshRenderer bodyRenderer;

        public Transform eyebrowsBone;

        List<GameObject> instanceObjs = new List<GameObject>();

        ResourcesManager r_manager;

        public void Init(StatesManager st)
        {
            anim = st.anim;
            r_manager = st.r_manager;
            LoadCharacter();
            hair = LoadCharacterObject(hairObj);
            eyebrows = LoadCharacterObject(eyebrowsObjs);
            mask = LoadMask(maskObj);
            LoadCharacterObject(other);
        }

        public void LoadCharacter()
        {
            MeshContainer m = r_manager.GetMesh(outfitID);
            LoadMeshContainer(m);
        }

        public void LoadMeshContainer(MeshContainer m)
        {
            if (!isFemale)
            {
                bodyRenderer.sharedMesh = m.m_mesh;
            }
            else
            {
                bodyRenderer.sharedMesh = m.f_mesh;
            }

            bodyRenderer.sharedMaterial = m.meterial;
        }

        public GameObject LoadMask(Mask m)
        {
            hair.SetActive(m.enableHair);
            eyebrows.SetActive(m.enableEyebrow);

            return LoadCharacterObject(m.obj);
        }

        public GameObject LoadCharacterObject(CharObject o)
        {
            if (o == null)
                return null;

            Transform b = GetBone(o.parentBone);
            GameObject prefab = o.f_prefab;
            if (prefab == null || !isFemale)
                prefab = o.m_prefab;
            GameObject go = Instantiate(prefab);
            go.transform.parent = b;
            go.transform.localPosition = Vector3.zero;
            go.transform.localEulerAngles = Vector3.zero;
            go.transform.localScale = Vector3.one * 100;
            instanceObjs.Add(go);

            return go;
        }

        public Transform GetBone(MyBones b)
        {
            switch (b)
            {
                case MyBones.head:
                    return anim.GetBoneTransform(HumanBodyBones.Head);
                case MyBones.chest:
                    return anim.GetBoneTransform(HumanBodyBones.Chest);
                case MyBones.eyebrows:
                    return eyebrowsBone;
                case MyBones.rightHand:
                    return anim.GetBoneTransform(HumanBodyBones.RightHand);
                case MyBones.leftHand:
                    return anim.GetBoneTransform(HumanBodyBones.LeftHand);
                case MyBones.rightUpperLeg:
                    return anim.GetBoneTransform(HumanBodyBones.RightUpperLeg);
                case MyBones.hips:
                    return anim.GetBoneTransform(HumanBodyBones.Hips);
                default:
                    return null;
            }
        }

    }
}
