using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Variables/Integer")]
    public class IntVariable : ScriptableObject
    {
        public int value;

        public void Apply(IntVariable v)
        {
            value += v.value;
        }

        public void Apply(int v)
        {
            value += v;
        }
    }
}
