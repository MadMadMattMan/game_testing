using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCManager : MonoBehaviour
{
    CharacterController player;
    InventoryManager inventory;
    Animator beeAnimator;
    HashSet<string> playerItems = new HashSet<string>();
    bool inChat = false;
    int chatStage = 0;
    int chatString = 0;


    [Header("Dialouge")]
    [SerializeField] List<string> introDialougeLines = new List<string>();
    [SerializeField] List<string> reminderDialougeLines = new List<string>();
    [SerializeField] List<string> closingDialougeLines = new List<string>();
    [SerializeField] List<string> goAwayDialouge = new List<string>();

    [Header("Settings")]
    [SerializeField] List<string> requiredItemsForUnlock = new List<string>();
    [SerializeField] GameObject chatObject;
    TextMeshProUGUI chatbox;
    [SerializeField] GameObject exclMark;

    void Awake() {
        StartCoroutine(AsyncAwake());
    }

    IEnumerator AsyncAwake() {
        yield return new WaitForFixedUpdate();
        player = GameObject.FindWithTag("Player").GetComponent<CharacterController>();
        inventory = player.inventoryManager;
        chatbox = chatObject.GetComponentInChildren<TextMeshProUGUI>();
        beeAnimator = GameObject.FindWithTag("Bee").GetComponent<Animator>();
    }

    public void Interact() {
        if (!inChat) {
            // inventory check 
            if (chatStage == 1) {
                Debug.Log("Checking inventory");
                // Add new inventory items to tracker
                foreach (var item in inventory.inventory)
                {
                    string name = item.name;
                    if (!playerItems.Contains(name))
                        playerItems.Add(name);
                }
                // Check if tracker contains required items, if not loop chat
                int collectedItems = 0;
                foreach (string item in requiredItemsForUnlock) {
                    if (playerItems.Contains(item)) 
                        collectedItems++;
                }
                if (collectedItems == requiredItemsForUnlock.Count)
                    chatStage++;
                Debug.Log($"Collected {collectedItems}/{requiredItemsForUnlock.Count}");
            }

            chatObject.SetActive(true);
            inChat = true;
            ProgressChat();
        }
    }

    public void ProgressChat() {
        // Select current chat list
        List<string> chatOption;
        if (chatStage == 0)
            chatOption = introDialougeLines;
        else if (chatStage == 1)
            chatOption = reminderDialougeLines;
        else if (chatStage == 2)
            chatOption = closingDialougeLines;
        else
            chatOption = goAwayDialouge;

        // check for current empty chat, if so skip list
        if (chatOption.Count == 0) {
            chatStage++;
            ProgressChat();
            return;
        }

        // escape if finished
        if (chatString >= chatOption.Count) {
            EndChat();
            return;
        }

        // Update text
        chatbox.text = chatOption[chatString];
        chatString++;
    }

    void EndChat() {
        chatString = 0;
        inChat = false;
        chatObject.SetActive(false);

        if (chatStage == 1) return;
        chatStage++;
        if (chatStage == 3) {
            beeAnimator.SetTrigger("Create Bee");
            exclMark.SetActive(false);
        }
    }
}
