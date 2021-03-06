﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName ="Variables/String")]
    public class StringVariable : ScriptableObject
    {
        public string value;

        public void Apply(StringVariable s)
        {
            value = s.value;
        }

        public void Apply(string s)
        {
            value = s;
        }

    }
}
