using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class StateManager : MonoBehaviour
    {
        public MovementValue movementValues;

        [System.Serializable]
        public class MovementValue
        {
            public float horizontal;
            public float vertical;
            public float moveAmount;
            public Vector3 moveDirection;
        }
        
        public State currentState;


        [HideInInspector]
        public float delta;
        [HideInInspector]
        public Transform mTransform;
        [HideInInspector]
        public Rigidbody rigid;

        private void Start()
        {
            mTransform = this.transform;
            rigid = GetComponent<Rigidbody>();
            rigid.drag = 4;
            rigid.angularDrag = 999;
            rigid.constraints = RigidbodyConstraints.FreezeRotation;
        }

        private void FixedUpdate()
        {
            delta = Time.deltaTime;
            if (currentState != null)
            {
                currentState.FixedTick(this);
            }
        }

        private void Update()
        {
            delta = Time.deltaTime;
            if (currentState != null)
            {
                currentState.Tick(this);
            }
        }
    }
}
