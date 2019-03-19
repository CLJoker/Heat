using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class ActionHook : MonoBehaviour
    {
        public Action[] startActions;
        public Action[] fixedUpdateActions;
		public Action[] updateActions;      

        private void Start()
        {
            if (startActions == null)
                return;

            for(int i = 0; i < startActions.Length; i++)
            {
                startActions[i].Execute();
            }
        }

        void FixedUpdate()
		{
			if (fixedUpdateActions == null)
				return;

			for (int i = 0; i < fixedUpdateActions.Length; i++)
			{
				fixedUpdateActions[i].Execute();
			}
		}

		void Update()
        {
			if (updateActions == null)
				return;

            for (int i = 0; i < updateActions.Length; i++)
            {
                updateActions[i].Execute();
            }
        }
    }
}
