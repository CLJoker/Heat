using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SA
{
    public class UI_UpdateText : MonoBehaviour
    {
        public Text target;
        public FloatVariable floatVariable;
        public IntVariable intVariable;
        public StringVariable stringVariable;

        private void OnEnable()
        {
            if (target == null)
                target = GetComponentInChildren<Text>();
        }

        public void UpdateTextFromFloatVariable()
        {
            target.text = floatVariable.value.ToString();
        }

        public void UpdateTextFromFloat(float f)
        {
            target.text = f.ToString();
        }

        public void UpdateTextFromIntVariable()
        {
            target.text = intVariable.value.ToString();
        }

        public void UpdateTextFromInt(int i)
        {
            target.text = i.ToString();
        }

        public void UpdateTextFromStringVariable()
        {
            target.text = stringVariable.value;
        }
        
        public void UpdateTextFromString(string s)
        {
            target.text = s;
        }
    }
}
