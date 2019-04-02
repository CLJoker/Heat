using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Managers/Player Profile")]
    public class PlayerProfile : ScriptableObject
    {
        public string modelId;
        public string[] itemIds;

    }
}
