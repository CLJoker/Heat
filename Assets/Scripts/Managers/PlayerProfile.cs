using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [System.Serializable]
    public class PlayerProfile
    {
        public string userName;
        public StringVariable outfitId;
        public Sprite avatar;
        public StringVariable mask_id;
        public StringVariable mw_id;
        public StringVariable sw_id;
        public BoolVariable isFemale;
    }
}
