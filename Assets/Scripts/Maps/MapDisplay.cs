using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SA
{
    public class MapDisplay : MonoBehaviour
    {
        public Text displayJob;
        public GameObject displayJobImage;
        public Text displayJobDescription;

        public void ChangeDisplayJob()
        {
            Map m = GameManagers.GetResourcesManager().currentMap.value;
            displayJob.text = m.mapName;
            displayJobImage.GetComponent<Image>().sprite = m.mapImage;
            displayJobDescription.text = m.description;
        }
    }
}
