using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class Character : MonoBehaviour
    {
        public string outfitID;

        public bool isFemale;
        public SkinnedMeshRenderer bodyRenderer;

        public void LoadCharacter(ResourcesManager r)
        {
            MeshContainer m = r.GetMesh(outfitID);
            LoadMeshContainer(m);
        }

        public void LoadMeshContainer(MeshContainer m)
        {
            if(!isFemale)
            {
                bodyRenderer.sharedMesh = m.m_mesh;
            }
            else
            {
                bodyRenderer.sharedMesh = m.f_mesh;
            }

            bodyRenderer.sharedMaterial = m.meterial;
        }

    }
}
