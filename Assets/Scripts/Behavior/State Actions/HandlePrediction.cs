using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/State Actions/Handle Prediction")]
    public class HandlePrediction : StateActions
    {
        public override void Execute(StateManager states)
        {
            states.multiplayerListener.Prediction();
        }

    }
}
