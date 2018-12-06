using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class WeaponHook : MonoBehaviour
    {
        public Transform leftHandIK;

        ParticleSystem[] particles;

        AudioSource audioSource;


        [HideInInspector]
        public float lastFired;

        public void Init()
        {
            GameObject go = new GameObject();
            go.name = "audio holder";
            go.transform.parent = this.transform;
            audioSource = go.AddComponent<AudioSource>();
            audioSource.spatialBlend = 1;

            particles = GetComponentsInChildren<ParticleSystem>();
        }

        public void Shoot()
        {
            if(particles != null)
            {
                for (int i = 0; i < particles.Length; i++)
                {
                    particles[i].Play();
                }
            }
        }
    }
}
