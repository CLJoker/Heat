using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA.UI
{
    public class ReplaceStringVariable : MonoBehaviour
    {
        public StringVariable variableTo;

        public UI_UpdateText textUpdater;

        public void Replace()
        {
            textUpdater.stringVariable = variableTo;
            textUpdater.UpdateTextFromStringVariable();
        }

    }
}
