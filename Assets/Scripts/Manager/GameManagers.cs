﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public static class GameManagers
    {
        static ObjectPooler objectPooler;
        public static ObjectPooler GetObjPool()
        {
            if(objectPooler == null)
            {
                objectPooler = Resources.Load("ObjectPooler") as ObjectPooler;
                objectPooler.Init();
            }

            return objectPooler;
        }
        static ResourcesManager resourcesManager;

        public static ResourcesManager GetResourcesManager()
        {
            if(resourcesManager == null)
            {
                resourcesManager = Resources.Load("ResourcesManager") as ResourcesManager;
                resourcesManager.Init();
            }

            return resourcesManager;
        }

        static AmmoPool ammoPool;
        public static AmmoPool GetAmmoPool()
        {
            if(ammoPool == null)
            {
                ammoPool = Resources.Load("AmmoPool") as AmmoPool;
                ammoPool.Init();
            }

            return ammoPool;
        }
    }
}