using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

namespace Play
{
    public class PlayerSelection : MonoBehaviour
    {
        [SerializeField]
        private PlayerInterface pInterfacePrefab;

        [SerializeField]
        private RectTransform pInterfaceHolder;

        [SerializeField]
        private Button addPlayerButton;

        [SerializeField]
        private Button clearPlayersButton;

        [SerializeField]
        private string[] randomNamePool;

        private HashSet<string> activePlayers = new HashSet<string>();
        private List<string> activePlayerHexColors = new List<string>();

        private static PlayerSelection instance;
        private static PlayerSelection Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = UIUtil.GetAllObjectsInScene<PlayerSelection>()[0];
                }

                if (instance == null)
                {
                    Debug.LogError("Instance was null.");
                }

                return instance;
            }
        }

        public static List<System.Tuple<string, string>> GetPlayers()
        {
            List<System.Tuple<string, string>> playerList = new List<System.Tuple<string, string>>();
            int currIndex = 0;
            foreach (string p in Instance.activePlayers)
            {
                playerList.Add(new System.Tuple<string, string>(p, Instance.activePlayerHexColors[currIndex]));
                currIndex++;
            }

            return playerList;
        }

        private void OnEnable()
        {
            if (activePlayers.Count == 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    AddPlayer();
                }
            }

            Validate();
        }

        private void Validate(bool throwError = true)
        {
            if (pInterfaceHolder.childCount != activePlayers.Count)
            {
                if (throwError)
                {
                    Debug.LogError(this.GetType().FullName + ": Missmatch in nr displayed players and actual players. (Graphics: " + pInterfaceHolder.childCount.ToString() + ", Players: " + activePlayers.Count);
                }
            }

            PlayHandler.SetNextButtonsActive(activePlayers.Count > 0);
        }

        private void Awake()
        {
            addPlayerButton.onClick.AddListener(AddPlayer);
        }

        private void AddPlayer()
        {
            int tries = 0;
            const int maxTries = 10;
            string nameToUse = "";

            while (tries < maxTries)
            {
                nameToUse = GetRandomNameFromPool();
                if (!activePlayers.Contains(nameToUse))
                {
                    break;
                }

                tries++;
            }

            // Add extra stuff to the name to make it valid.
            while (string.IsNullOrEmpty(nameToUse) || activePlayers.Contains(nameToUse))
            {
                nameToUse += "_x";
            }

            activePlayers.Add(nameToUse);
            activePlayerHexColors.Add(UIUtil.GetPlayerColorHex(activePlayers.Count - 1));
            GameObject ob = Instantiate(pInterfacePrefab.gameObject);
            ob.GetComponent<PlayerInterface>().Set(nameToUse, OnRenamePlayer, RemovePlayer);
            ob.transform.SetParent(pInterfaceHolder);
            ob.transform.localScale = Vector3.one;
            Validate();
        }

        private void OnRenamePlayer(string newName, string oldName, PlayerInterface pInterface)
        {
            if (activePlayers.Contains(newName))
            {
                ConfirmScreen.Create().Set("Invalid name, it already existed. Reverting back...", useCancel: false);
                // The new name already exists.
                pInterface.SetName(oldName); // Revert back name change.
            }
            else
            {
                activePlayers.Remove(oldName);
                activePlayers.Add(newName);
            }
        }

        private void RemovePlayer(string name)
        {
            activePlayers.Remove(name);
            activePlayerHexColors.RemoveAt(activePlayerHexColors.Count - 1);
            Validate(false);
            UIUtil.InvokeDelayed(() => { Validate(true); }, 1);
        }

        private string GetRandomNameFromPool()
        {
            return randomNamePool[Random.Range(0, randomNamePool.Length - 1)];
        }
    }
}
