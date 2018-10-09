using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SA.UI
{
    public class UIElement : MonoBehaviour
    {
        UIUpdater updater;

        private void Awake()
        {
            updater = UIUpdater.singleton;

            if (updater != null)
                updater.elements.Add(this);
        }

        public virtual void Init()
        {

        }

        public virtual void Tick(float delta)
        {

        }
    }
}
