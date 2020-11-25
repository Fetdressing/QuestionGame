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
        private UnityInputFieldInterface nameInput;

        [SerializeField]
        private Button removeButton;

        private System.Action<string, string, PlayerInterface> onNameChanged;
        private System.Action<string> onRemove;

        /// <summary>
        /// Sets this.
        /// </summary>
        /// <param name="name">The name of this.</param>
        /// <param name="onNameChanged">The first string representing the new name and the second one the old one.</param>
        /// <param name="onRemove">On removing this.</param>
        public void Set(string name, System.Action<string, string, PlayerInterface> onNameChanged, System.Action<string> onRemove)
        {
            this.SetName(name);

            this.onNameChanged = onNameChanged;
            this.onRemove = onRemove;

            this.nameInput.onEndEdit.AddListener(OnNameChanged);
            this.removeButton.onClick.AddListener(OnRemove);
        }

        public void SetName(string newName)
        {
            this.pName = newName;
            this.nameInput.SetTextWithoutNotify(newName);
        }

        private void OnNameChanged(string newName)
        {
            if (string.Equals(newName, this.pName))
            {
                return;
            }

            onNameChanged.Invoke(newName, this.pName, this);
            this.pName = newName;
        }

        private void OnRemove()
        {
            onRemove.Invoke(this.pName);
            DestroySelf();
        }
    }
}
