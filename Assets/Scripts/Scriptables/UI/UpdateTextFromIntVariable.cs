using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO.UI;
using UnityEngine.UI;

namespace SO
{
    public class UpdateTextFromIntVariable : UIPropertyUpdater
    {
        public IntVariable targetVariable;
        public Text targetText;

        public override void Raise()
        {
            targetText.text = targetVariable.value.ToString();
        }

        public void Raise(string target)
        {
            targetText.text = target;
        }
    }
}
