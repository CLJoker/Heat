using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class StateManager : MonoBehaviour, IHittable
    {
        public int photonId;
        public PlayerStats stats;
        public MovementValue movementValues;
        public Inventory inventory;

        [HideInInspector]
        public List<Rigidbody> ragdollRB = new List<Rigidbody>();
        [HideInInspector]
        public List<Collider> ragdollCols = new List<Collider>();

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

        public bool isLocal;
        public bool isAiming;
        public bool isInteracting;
        public bool isShooting;
        public bool isCrouching;
        public bool isReloading;
        public bool isVaulting;
        public bool isGrounded;
        public bool isDead;

        public bool shootingFlag;
        public bool reloadingFlag;
        public bool vaultingFlag;
        public bool healthChangedFlag;

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
        public new Rigidbody rigidbody;
        [HideInInspector]
        public LayerMask ignoreLayers;
        [HideInInspector]
        public AnimatorHook animHook;


        public VaultData vaultData;
        public AnimHashes hashes;
        public Ballistics ballisticsAction;

        public MultiplayerListener multiplayerListener;
        public bool isOfflineController;
        public StateActions offlineActions;

        private void Start()
        {
            mTransform = this.transform;
            rigidbody = GetComponent<Rigidbody>();
            stats.health = 100;
            healthChangedFlag = true;
            if (isOfflineController)
            {
                offlineActions.Execute(this);
            }
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

        public void SetCurrentState(State targetState)
        {
            if(currentState != null)
            {
                currentState.OnExit(this);
            }

            currentState = targetState;
            currentState.OnEnter(this);
        }

        public void OnHit(StateManager shooter, Weapon wp, Vector3 dir, Vector3 pos)
        {
            GameObject hitParticle = GameManagers.GetObjPool().RequestObject("Blood_Fx");
            Quaternion rot = Quaternion.LookRotation(-dir);
            hitParticle.transform.position = pos;
            hitParticle.transform.rotation = rot;

            stats.health -= wp.ammoType.damageValue;
            if(stats.health <= 0)
            {
                stats.health = 0;

                if (!isDead)
                {
                    isDead = true;
                    MultiplayerManager.singleton.BroadcastKillPlayer(photonId, shooter.photonId);
                    KillPlayer();
                }
            }

            healthChangedFlag = true;
        }

        public void SpawnPlayer(Vector3 spawnPosition, Quaternion rotation)
        {
            if (isLocal)
            {

            }
            healthChangedFlag = true;
            stats.health = 100;

            mTransform.position = spawnPosition;
            mTransform.rotation = rotation;
            anim.Play("Locomotion Normal");
            anim.Play("Empty Override");
            isDead = false;
        }

        public void KillPlayer()
        {
            isDead = true;
            anim.CrossFade("death2", 0.4f);
        }
    }
}
