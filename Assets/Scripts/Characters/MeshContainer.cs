﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Characters/Mesh Container")]
    public class MeshContainer : ScriptableObject
    {
        public string id;

        public Mesh m_mesh;
        public Mesh f_mesh;
        public Material meterial;

    }
}