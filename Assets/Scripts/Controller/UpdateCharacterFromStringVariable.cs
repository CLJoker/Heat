using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class UpdateCharacterFromStringVariable : MonoBehaviour
    {
        public StringVariable curOutfit;
        public StringVariable maskId;
        public Character character;
        public bool updateOnEnable;
        public Mask hair;
        public BoolVariable isMale;
        public StatesManager states;
        public InputHandler input;

        public void UpdateCharacter()
        {
            //character.outfitID = curOutfit.value;
            //character.isFemale = !isMale.value;

            //if (maskId)
            //{
            //    ResourcesManager rm = Resources.Load("ResourcesManager") as ResourcesManager;
            //    hair = rm.GetMask(maskId.value);
            //}

            //character.LoadCharacter();
            //character.LoadMask(hair);

        }

        public void OnEnable()
        {
            if (updateOnEnable)
                UpdateCharacter();
        }
    }
}
