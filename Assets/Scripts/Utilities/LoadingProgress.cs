using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SA
{
    public class LoadingProgress : MonoBehaviour
    {
        public Text progressText;
        public Slider progressBar;

        public static LoadingProgress singleton;
        private void Awake()
        {
            if(singleton == null)
            {
                singleton = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        public void UpdateLoadingProgress(string text, float progress)
        {
            progressBar.value = progress;
            progressText.text = text;
        }
    }
}
