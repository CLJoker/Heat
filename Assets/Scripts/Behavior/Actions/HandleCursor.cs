using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName ="Actions/Mono Actions/Handle Cursor")]
    public class HandleCursor : Action
    {
        public CursorLockMode lockMode;
        public bool isVisible;

        public override void Execute()
        {
            Cursor.lockState = lockMode;
            Cursor.visible = isVisible;
        }

    }
}
