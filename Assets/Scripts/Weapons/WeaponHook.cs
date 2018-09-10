using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class WeaponHook : MonoBehaviour
    {

        public Transform leftHandIK;
        ParticleSystem[] particles;


        private void OnEnable()
        {
            particles = transform.GetComponentsInChildren<ParticleSystem>();
        }

        public void Shoot()
        {
            if(particles != null && particles.Length > 0)
            {
                for (int i = 0; i < particles.Length; i++)
                {
                    particles[i].Play();
                }
            }
        }
    }
}

