using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA.UI;

namespace SA
{
    public class StatesManager : MonoBehaviour
    {
        [System.Serializable]
        public class InputVariables
        {
            public float horizontal;
            public float vertical;
            public float moveAmount;
            public Vector3 moveDirection;
            public Vector3 aimPosition;
            public Vector3 rotateDirection;

        }

        [System.Serializable]
        public class ControllerStates
        {
            public bool onGround;
            public bool isAiming;
            public bool isCrouching;
            public bool isRunning;
            public bool isInteracting;

        }

        public ResourcesManager r_manager;
        public ControllerStates states;
        public ControllerStats stats;
        public InputVariables inp;
        public WeaponManager w_manager;
        public Character character;
        public UI_LoadPlayerHUD playerHUD;


        #region References
        public Animator anim;
        public GameObject activeModel;
        [HideInInspector]
        public AnimatorHook a_hook;
        [HideInInspector]
        public Rigidbody rigid;
        [HideInInspector]
        public Collider controllerCollider;

        List<Collider> ragdollColliders = new List<Collider>();
        List<Rigidbody> ragdollRigids = new List<Rigidbody>();

        [HideInInspector]
        public LayerMask ignoreLayers;
        [HideInInspector]
        public LayerMask ignoreForGround;
        [HideInInspector]
        public Transform mTransform;
        //[HideInInspector]
        //public Transform referencesParent;
        public CharState curState;

        public float delta;
        #endregion

        #region Init

        public void LoadPlayerProfile(PlayerProfile p)
        {
            w_manager.mw_id = p.mw_id.value;
            w_manager.sw_id = p.sw_id.value;
            character = GetComponent<Character>();
            character.outfitID = p.outfitId.value;
            character.maskObj = r_manager.GetMask(p.mask_id.value);
            character.isFemale = p.isFemale.value;
            playerHUD = GetComponent<UI_LoadPlayerHUD>();
            if(playerHUD != null)
            {
                playerHUD.playerAvatar.sprite = p.avatar;
                playerHUD.playerName.text = p.userName;
            }
        }

        public void Init()
        {
            r_manager.Init();

            mTransform = gameObject.transform;
            SetupAnimator();
            SetupRigidbody();
            controllerCollider = GetComponent<Collider>();
            SetupRagdoll();

            ignoreLayers = ~(1 << 9);
            ignoreForGround = ~(1 << 9 | 1 << 10);

            a_hook = activeModel.AddComponent<AnimatorHook>();
            a_hook.Init(this);
            Init_WeaponManager();
            SetupCharacter();
        }

        void SetupAnimator()
        {
            if(activeModel == null)
            {
                anim = GetComponentInChildren<Animator>();
                activeModel = anim.gameObject;
            }

            if (anim == null)
                anim = GetComponentInChildren<Animator>();

            anim.applyRootMotion = false;
        }

        void SetupRigidbody()
        {
            rigid = GetComponent<Rigidbody>();
            rigid.isKinematic = false;
            rigid.drag = 4;
            rigid.angularDrag = 999;
            rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }

        void SetupRagdoll()
        {
            Rigidbody[] rigids = activeModel.GetComponentsInChildren<Rigidbody>();
            foreach(Rigidbody r in rigids)
            {
                if (r == rigid)
                {
                    continue;
                }

                Collider c = r.gameObject.GetComponent<Collider>();
                c.isTrigger = true;

                ragdollRigids.Add(r);
                ragdollColliders.Add(c);
                r.isKinematic = true;
                r.gameObject.layer = 10;
            }
        }

        void SetupCharacter()
        {
            
            character.Init(this);
        }
        #endregion

        #region Fixed Update
        public void FixedTick(float d)
        {
            delta = d;

            switch(curState)
            {
                case CharState.normal:
                    states.onGround = OnGround();
                    if(states.isAiming)
                    {
                        MovementAiming();
                    }
                    else
                    {
                        MovementNormal();
                    }
                    RotationNormal();
                    break;
                case CharState.onAir:
                    rigid.drag = 0;
                    states.onGround = OnGround();
                    break;
                case CharState.cover:
                    break;
                case CharState.vaulting:
                    break;
                default:
                    break;
            }
        }

        void RotationNormal()
        {
            if (!states.isAiming)
            {
                inp.rotateDirection = inp.moveDirection;
            }
            Vector3 targetDir = inp.moveDirection;
            targetDir.y = 0;

            if (targetDir == Vector3.zero)
                targetDir = mTransform.forward;

            Quaternion lookDir = Quaternion.LookRotation(targetDir);
            Quaternion targetRot = Quaternion.Slerp(mTransform.rotation, lookDir, stats.rotateSpeed * delta);
            mTransform.rotation = targetRot;
        }

        void MovementNormal()
        {
            if (inp.moveAmount > 0.05f)
                rigid.drag = 0;
            else
                rigid.drag = 4;

            float speed = stats.walkSpeed;
            if (states.isRunning)
                speed = stats.runSpeed;
            if (states.isCrouching)
                speed = stats.crouchSpeed;

            Vector3 dir = Vector3.zero;
            dir = mTransform.forward * (speed * inp.moveAmount);
            rigid.velocity = dir;
        }

        void MovementAiming()
        {
            float speed = stats.aimSpeed;
            Vector3 v = inp.moveDirection * speed;
            rigid.velocity = v;
        }
        #endregion

        #region Update
        float rT;

        public void Tick(float d)
        {
            delta = d;

            switch (curState)
            {
                case CharState.normal:
                    states.onGround = OnGround();
                    HandleAnimationAll();
                    a_hook.Tick();

                    if (states.isInteracting)
                    {
                        rT += delta;
                        if(rT > 3)
                        {
                            states.isInteracting = false;
                            rT = 0;
                        }
                    }
                    break;
                case CharState.onAir:
                    states.onGround = OnGround();
                    break;
                case CharState.cover:
                    break;
                case CharState.vaulting:
                    break;
                default:
                    break;
            }
        }

        void HandleAnimationAll()
        {
            anim.SetBool(StaticStrings.sprint, states.isRunning);
            anim.SetBool(StaticStrings.aiming, states.isAiming);
            anim.SetBool(StaticStrings.crouch, states.isCrouching);

            if(states.isAiming)
            {
                HandleAnimationsAiming();
            }
            else
            {
                HandleAnimationsNormal();
            }
        }

        void HandleAnimationsNormal()
        {
            if (inp.moveAmount > 0.05f)
            {
                rigid.drag = 0;
                anim.SetBool(StaticStrings.sprint, true);
            }
            else
            {
                rigid.drag = 4;
                anim.SetBool(StaticStrings.sprint, false);
            }

            float anim_v = inp.moveAmount;
            anim.SetFloat(StaticStrings.vertical, anim_v, 0.15f, delta);
        }

        void HandleAnimationsAiming()
        {
            float v = inp.vertical;
            float h = inp.horizontal;

            anim.SetFloat(StaticStrings.horizontal, h, 0.2f, delta);
            anim.SetFloat(StaticStrings.vertical, v, 0.2f, delta);
        }
        #endregion

        #region Manager Functions
        public void Init_WeaponManager()
        {
            CreateRuntimeWeapon(w_manager.mw_id, ref w_manager.m_weapon);
            EquipRuntimeWeapon(w_manager.m_weapon);
        }

        public void CreateRuntimeWeapon(string id, ref RuntimeWeapon r_w_m)
        {
            Weapon w = r_manager.GetWeapon(id);
            RuntimeWeapon rw = r_manager.runtime.WeaponToRuntimeWeapon(w);

            GameObject go = Instantiate(w.modelPrefab);
            rw.m_instance = go;
            rw.w_actual = w;
            rw.w_hook = go.GetComponent<WeaponHook>();
            go.SetActive(false);

            Transform p = anim.GetBoneTransform(HumanBodyBones.RightHand);
            go.transform.parent = p;
            go.transform.localPosition = Vector3.zero;
            go.transform.localEulerAngles = Vector3.zero;
            go.transform.localScale = Vector3.one;

            r_w_m = rw;
        }

        public void EquipRuntimeWeapon(RuntimeWeapon rw)
        {
            rw.m_instance.SetActive(true);
            a_hook.EquipWeapon(rw);

            anim.SetFloat(StaticStrings.WeaponType, rw.w_actual.WeaponType);
            w_manager.SetCurrentWeapon(rw);
        }

        public bool ShootWeapon(float t)
        {
            bool retVal = false;

            RuntimeWeapon c = w_manager.GetCurrentWeapon();
            if (c.curAmmo > 0)
            {
                if (t - c.lastFired > c.w_actual.fireRate)
                {
                    c.lastFired = t;
                    c.ShootWeapon();
                    retVal = true;
                    a_hook.RecoilAnim();
                }
            }

            return retVal;
        }

        public bool Reload()
        {
            RuntimeWeapon c = w_manager.GetCurrentWeapon();
            bool retVal = false;
            if(c.curAmmo < c.w_actual.megazineAmmo)
            {
                if(c.w_actual.megazineAmmo <= c.curCarrying)
                {
                    c.curAmmo = c.w_actual.megazineAmmo;
                    c.curCarrying -= c.curAmmo;
                }
                else
                {
                    c.curAmmo = c.curCarrying;
                    c.curCarrying = 0;
                }

                retVal = true;
                anim.CrossFade("Rifle Reload", 0.2f);
                states.isInteracting = true;
            }

            return retVal;
        }

        #endregion

        bool OnGround()
        {
            Vector3 origin = mTransform.position;
            origin.y += 0.6f;
            Vector3 dir = -Vector3.up;
            float dis = 0.7f;
            RaycastHit hit;
            if(Physics.Raycast(origin, dir, out hit, dis, ignoreForGround))
            {
                Vector3 tp = hit.point;
                mTransform.position = tp;
                return true;
            }

            return false;
        }

    }

    public enum CharState
    {
        normal,
        onAir,
        cover,
        vaulting
    }
}
