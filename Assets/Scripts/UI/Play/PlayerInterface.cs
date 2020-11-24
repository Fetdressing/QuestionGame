using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

namespace Play
{
    public class PlayerInterface : UIBase
    {
        private string pName;

        [SerializeField]
        private TextMeshProUGUI nameDisplay;

        [SerializeField]
        private Button removeButton;

        public void Set(string name, System.Action<string> onRemove)
        {
            this.pName = name;
            this.nameDisplay.text = name;

            removeButton.onClick.AddListener(() =>
            {
                onRemove.Invoke(this.pName);
                DestroySelf();
            });
        }
    }
}
