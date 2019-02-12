﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class StateManager : MonoBehaviour, IHittable
    {
        public MovementValue movementValues;
        public Inventory inventory;

        [System.Serializable]
        public class MovementValue
        {
            public float horizontal;
            public float vertical;
            public float moveAmount;
            public Vector3 moveDirection;
            public Vector3 lookDirection;
            public Vector3 aimPosition;
        }
        
        public State currentState;

        public bool isAiming;
        public bool isInteracting;
        public bool isShooting;
        public bool isCrouching;
        public bool isReloading;
        public bool isVaulting;

        public void SetCrouching()
        {
            isCrouching = !isCrouching;
        }

        public void SetReloading()
        {
            isReloading = true;
        }

        [HideInInspector]
        public Animator anim;

        [HideInInspector]
        public float delta;
        [HideInInspector]
        public Transform mTransform;
        [HideInInspector]
        public Rigidbody rigid;
        [HideInInspector]
        public LayerMask ignoreLayers;
        [HideInInspector]
        public AnimatorHook animHook;

        public StateActions initActionsBatch;
        public VaultData vaultData;
        public AnimHashes hashes;
        public Ballistics ballisticsAction;

        private void Start()
        {
            mTransform = this.transform;
            rigid = GetComponent<Rigidbody>();
            rigid.drag = 4;
            rigid.angularDrag = 999;
            rigid.constraints = RigidbodyConstraints.FreezeRotation;
            ignoreLayers = ~(1 << 9 | 1 << 3);

            anim = GetComponentInChildren<Animator>();

            initActionsBatch.Execute(this);

            hashes = new AnimHashes();
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

        public void PlayAnimation(string targetAnim)
        {
            anim.CrossFade(targetAnim, 0.2f);
        }

        public void OnHit(StateManager shooter, Weapon wp, Vector3 dir, Vector3 pos)
        {
           
        }
    }
}
