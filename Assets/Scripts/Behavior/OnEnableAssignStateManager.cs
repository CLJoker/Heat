using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class OnEnableAssignStateManager : MonoBehaviour
    {
        public StatesVariable targetVariable;

        private void OnEnable()
        {
            targetVariable.value = GetComponent<StateManager>();
            Destroy(this);
        }
    }
}
