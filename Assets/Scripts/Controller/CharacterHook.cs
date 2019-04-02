using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class CharacterHook : MonoBehaviour
    {
        public BodyPart body;

        public void Init(ClothItem c)
        {
            body.meshRenderer.sharedMesh = c.mesh;
            body.meshRenderer.sharedMaterial = c.material;
        }

    }

    [System.Serializable]
    public class BodyPart
    {
        public SkinnedMeshRenderer meshRenderer;
    }
}
