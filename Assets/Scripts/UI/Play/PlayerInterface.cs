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
        private TextMeshProUGUI nameText;

        [SerializeField]
        private Button nameInputButton;

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

            this.nameInputButton.onClick.AddListener(delegate { InputScreen.Create().Set("What is the Name of this Player?", 3, 14, true, OnNameChanged, lineType: InputField.LineType.SingleLine, startValue: "", contentType: InputField.ContentType.Name, promptText: "Enter Name"); });
            this.removeButton.onClick.AddListener(OnRemove);
        }

        public void SetName(string newName)
        {
            this.pName = newName;
            this.nameText.text = newName;
        }

        public string GetName()
        {
            return this.pName;
        }

        private void OnNameChanged(string newName)
        {
            if (string.IsNullOrEmpty(newName))
            {
                return;
            }

            if (string.Equals(newName, this.pName))
            {
                return;
            }

            onNameChanged.Invoke(newName, this.pName, this);
            this.SetName(newName);
        }

        private void OnRemove()
        {
            onRemove.Invoke(this.pName);
            DestroySelf();
        }
    }
}
