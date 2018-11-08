using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public static class StaticFunctions
    {
        public static string JobTypeToString(JobType t)
        {
            switch (t)
            {
                case JobType.shootout:
                    return "SHOOTOUT";
                case JobType.heist:
                    return "HEIST";
                case JobType.deathmatch:
                    return "DEATHMATCH";
                default:
                    return "";
            }
        }
    }
}
