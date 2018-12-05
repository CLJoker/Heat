using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public static class GameManagers
    {
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

    }
}
