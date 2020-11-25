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

        private void Awake()
        {
            addPlayerButton.onClick.AddListener(AddPlayer);

            for (int i = 0; i < 2; i++)
            {
                AddPlayer();
            }
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
            GameObject ob = Instantiate(pInterfacePrefab.gameObject);
            ob.GetComponent<PlayerInterface>().Set(nameToUse, OnRenamePlayer, RemovePlayer);
            ob.transform.SetParent(pInterfaceHolder);
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
        }

        private string GetRandomNameFromPool()
        {
            return randomNamePool[Random.Range(0, randomNamePool.Length - 1)];
        }
    }
}
