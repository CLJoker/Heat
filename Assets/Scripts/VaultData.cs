using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [System.Serializable]
    public class VaultData
    {
        public Vector3 startPosition;
        public Vector3 endingPosition;
        public float vaultSpeed = 2;

        public float animLength;
        public float vaultT;
        public bool isInit;
    }
}
