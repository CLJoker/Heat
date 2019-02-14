using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Variables/Map Variable")]
    public class MapVariable : ScriptableObject
    {
        public Map value;

        public void SetMap(Map m)
        {
            value = m;
        }
    }
}
