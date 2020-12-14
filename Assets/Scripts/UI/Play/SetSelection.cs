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

        private static SetSelection instance;
        private static SetSelection Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = UIUtil.GetAllObjectsInScene<SetSelection>()[0];
                }

                return instance;
            }
        }

        public static List<QuestionManager.QuestionSet> GetSets()
        {
            List<QuestionManager.QuestionSet> questionList = new List<QuestionManager.QuestionSet>();

            SetSelectionInterface[] interfaces = Instance.interfaceHolder.GetComponentsInChildren<SetSelectionInterface>();
            for (int i = 0; i < interfaces.Length; i++)
            {
                if (interfaces[i].IsOn)
                {
                    questionList.Add(interfaces[i].Get());
                }
            }

            return questionList;
        }

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
                ob.GetComponent<SetSelectionInterface>().Set(QuestionManager.GetSet(setNameList[i]), i == 0, Validate);
            }
        }

        private void Validate()
        {
            SetSelectionInterface[] interfaces = interfaceHolder.GetComponentsInChildren<SetSelectionInterface>();

            if (interfaces.Length == 0)
            {
                ConfirmScreen.Create().Set("Error! No sets exist, something went wrong.", Application.Quit, useCancel: false);
            }

            int nrOn = 0;

            for (int i = 0; i < interfaces.Length; i++)
            {
                nrOn += interfaces[i].IsOn ? 1 : 0;
            }

            PlayHandler.SetNextButtonsActive(interfaces.Length > 0 && nrOn > 0);
        }
    }
}
