﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class InputHandler : MonoBehaviour
    {
        public bool isNonControl = false;
        float horizontal;
        float vertical;

        bool aimInput;
        bool sprintInput;
        bool shootInput;
        bool crouchInput;
        bool reloadInput;
        bool switchInput;
        bool pivotInput;

        bool isInit;
        public bool debugAim;

        float delta;

        public StatesManager states;
        public CameraHandler camHandler;
        public PlayerReferences p_references;
        public GameSettings gameSettings;
        bool updateUI;

        private void Start()
        {
            if (gameSettings == null)
                gameSettings = Resources.Load("GameSettings") as GameSettings;
            gameSettings.r_manager.Init();
            InitInGame();
        }

        public void InitInGame()
        {
            p_references.Init();
            if (states.r_manager == null)
                states.r_manager = gameSettings.r_manager;
            states.LoadPlayerProfile(gameSettings.playerProfile);
            states.Init();
            camHandler.Init(this);

            UpdatePlayerReferencesForWeapon(states.w_manager.GetCurrentWeapon());
            updateUI = true;

            isInit = true;
        }

        #region Fixed Update
        private void FixedUpdate()
        {
            if (!isInit)
                return;

            delta = Time.fixedDeltaTime;
            GetInput_FixedUpdate();
            InGame_UpdateStates_FixedUpdate();
            states.FixedTick(delta);

            //camHandler.FixedTick(delta);

            if (states.rigid.velocity.sqrMagnitude > 0.5f)
                p_references.targetSpread.value = 120f;
        }

        void GetInput_FixedUpdate()
        {
            if (isNonControl)
                return;

            vertical = Input.GetAxis(StaticStrings.Vertical);
            horizontal = Input.GetAxis(StaticStrings.Horizontal);
        }

        void InGame_UpdateStates_FixedUpdate()
        {
            states.inp.horizontal = horizontal;
            states.inp.vertical = vertical;
            states.inp.moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

            Vector3 moveDir = camHandler.mTransform.forward * vertical;
            moveDir += camHandler.mTransform.right * horizontal;
            moveDir.Normalize();
            states.inp.moveDirection = moveDir;

            states.inp.rotateDirection = camHandler.mTransform.forward;

        }
        #endregion

        #region Update
        private void Update()
        {
            if (!isInit)
                return;


            delta = Time.deltaTime;
            GetInput_Update();
            AimPosition();
            InGame_UpdateStates_Update();


            if (debugAim)
                states.states.isAiming = true;
            states.Tick(delta);

            if(updateUI)
            {
                updateUI = false;
                UpdatePlayerReferencesForWeapon(states.w_manager.GetCurrentWeapon());

                p_references.e_UpdateUI.Raise();
            }
        }

        void GetInput_Update()
        {

            if (isNonControl)
                return;

            aimInput = Input.GetMouseButton(1);
            shootInput = Input.GetMouseButton(0);
            pivotInput = Input.GetButtonDown(StaticStrings.Pivot);
            reloadInput = Input.GetButtonDown(StaticStrings.Reload);
        }

        void InGame_UpdateStates_Update()
        {
            if (reloadInput)
            {
                bool isReloading = states.Reload();
                if (isReloading)
                {
                    aimInput = false;
                    shootInput = false;
                    updateUI = true;
                }
            }

            states.states.isAiming = aimInput;

            if(shootInput)
            {
                states.states.isAiming = true;
                bool shootActual = states.ShootWeapon(Time.realtimeSinceStartup);
                if(shootActual)
                {
                    p_references.targetSpread.value += 80;
                    updateUI = true;
                }
            }

            p_references.isAiming.value = states.states.isAiming;

            if (pivotInput)
            {
                p_references.isLeftPivot.value = !p_references.isLeftPivot.value;
            }
        }

        void AimPosition()
        {
            Ray ray = new Ray(camHandler.camTrans.position, camHandler.camTrans.forward);

            states.inp.aimPosition = ray.GetPoint(30);

            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 100, states.ignoreLayers))
            {
                states.inp.aimPosition = hit.point;
            }
        }
        #endregion

        #region Manager Functions

        public void UpdatePlayerReferencesForWeapon(RuntimeWeapon r)
        {
            p_references.curAmmo.value = r.curAmmo;
            p_references.curCarrying.value = r.curCarrying;
        }

        #endregion
    }

    public enum GamePhase
    {
        inGame,
        inMenu
    }
}

