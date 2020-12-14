using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RotaryHeart;
using RotaryHeart.Lib.SerializableDictionary;

namespace Play
{
    public class PlayHandler : MonoBehaviour
    {
        [SerializeField]
        private PhaseDict phaseDict;

        [SerializeField]
        private UnityButtonInterface[] nextButtons;

        [SerializeField]
        private UnityButtonInterface[] backButtons;

        private Phase currPhase;

        private enum Phase
        {
            PlayerSelection,
            SetSelection,
            Play,
            End
        }

        private static PlayHandler instance;
        private static PlayHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<PlayHandler>();
                }

                return instance;
            }
        }

        private Phase CurrPhase
        {
            get
            {
                return currPhase;
            }
        }

        public static void SetNextButtonsActive(bool active)
        {
            for (int i = 0; i < Instance.nextButtons.Length; i++)
            {
                Instance.nextButtons[i].gameObject.SetActive(active);
            }
        }

        private void Awake()
        {
            SetActivePhase(Phase.PlayerSelection);

            for (int i = 0; i < nextButtons.Length; i++)
            {
                nextButtons[i].onClick.AddListener(() => { SetActivePhase(CurrPhase + 1); });
            }

            for (int i = 0; i < backButtons.Length; i++)
            {
                backButtons[i].onClick.AddListener(() => { SetActivePhase(CurrPhase - 1); });
            }
        }

        private void SetActivePhase(Phase phase)
        {
            if (phase < 0 || phase >= Phase.End)
            {
                Debug.Log("Invalid phase: " + phase.ToString());
                return;
            }

            currPhase = phase;

            foreach (Phase p in phaseDict.Keys)
            {
                PhaseObj pObj;
                if (phaseDict.TryGetValue(p, out pObj))
                {
                    pObj.SetActive(phase == p);
                }
            }
        }

        [System.Serializable]
        private class PhaseObj
        {
            [SerializeField]
            private RectTransform root;

            public void SetActive(bool active)
            {
                this.root.gameObject.SetActive(active);
            }
        }

        [System.Serializable]
        private class PhaseDict : SerializableDictionaryBase<Phase, PhaseObj>
        {
        }
    }
};
