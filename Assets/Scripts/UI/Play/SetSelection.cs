using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

namespace Play
{
    public class SetSelection : MonoBehaviour
    {
        [SerializeField]
        private RectTransform interfaceHolder;

        [SerializeField]
        private SetSelectionInterface setSelectionInterfacePrefab;

        private void OnEnable()
        {
            List<Transform> children = Utility.GetFirstLevelChildren(interfaceHolder);
            for (int i = 0; i < children.Count; i++)
            {
                Destroy(children[i].gameObject);
            }

            List<string> setNameList = QuestionManager.GetAllSetNames();
            for (int i = 0; i < setNameList.Count; i++)
            {
                GameObject ob = Instantiate(setSelectionInterfacePrefab.gameObject);
                ob.transform.SetParent(interfaceHolder);
                ob.transform.localScale = Vector3.one;
                ob.GetComponent<SetSelectionInterface>().Set(QuestionManager.GetSet(setNameList[i]), i == 0);
            }
        }
    }
}
