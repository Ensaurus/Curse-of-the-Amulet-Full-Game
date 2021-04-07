using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : Singleton<Inventory>
{   
    // using a struct instead of a dictionary just cuz unity doesn't show dictionaries in the editor
    [Serializable]
    public struct BootlegDictionary
    {
        public string item;
        public int amount;
    }
    public BootlegDictionary[] dictionarySetup;
    public Dictionary<string, int> items = new Dictionary<string, int>();   // strings are used as keys ex. "camera"/"trap", values are how many there are
    public string[] keyArr;
    public Item activeItem;
    public int keyIndex;
    public string activeKey;
    public bool empty; // true when no items in inventory 

    private void Start()
    {
        EventManager.Instance.PowerUpCollected.AddListener(AddItem);
        // setup the dictionary with values from editor
        for (int i=0; i<dictionarySetup.Length; i++)
        {
            items.Add(dictionarySetup[i].item, dictionarySetup[i].amount);
        }
        keyArr = items.Keys.ToArray();
        activeKey = "camera";
        activeItem = GrabItemFromKey("camera");
    }

    private void Update()
    {   
        // e is use active item
        if (Input.GetKeyDown(KeyCode.E) && !empty)
        { 
            activeItem.Use();
            items[activeKey] -= 1;
            if (items[activeKey] == 0)
            {
                string key = FindNextValidKey();
                activeItem = GrabItemFromKey(key);
                activeKey = key;
            }
            EventManager.Instance.ItemUsed.Invoke(activeItem);
        }
        // q is rotate active item
        if (Input.GetKeyDown(KeyCode.Q) && !empty)
        {
            string key = FindNextValidKey();
            activeItem = GrabItemFromKey(key);
            activeKey = key;
            EventManager.Instance.ItemSwap.Invoke(activeItem);
        }
    }

    private void AddItem(GameObject newItem)
    {
        PowerUp powerUpScript = newItem.GetComponent<PowerUp>();
        if (powerUpScript.isUseable())
        {
            string itemKey = powerUpScript.GetItem();
            items[itemKey] += powerUpScript.GetAmount();
            activeItem = GrabItemFromKey(itemKey);
            keyIndex = Array.IndexOf(keyArr, itemKey);
            activeKey = itemKey;
            empty = false;
            EventManager.Instance.ItemIncrease.Invoke(activeItem);
        }
    }

    // returns the key of the next item in the inventory for which u posess at least 1
    private string FindNextValidKey()
    {
        int counter = 0;
        keyIndex = (keyIndex + 1) % keyArr.Length;
        string key = keyArr[keyIndex];
        // scroll through dictionary until u find an item that exists in inv or find it to be empty
        while (items[key] == 0 && !empty)
        {
            keyIndex = (keyIndex + 1) % keyArr.Length;
            key = keyArr[keyIndex];
            // if it's checked all the entries in the dictionary, dictionary is empty
            if (counter == keyArr.Length)
            {
                empty = true;
                return "empty";
            }
            counter++;
        }
        return key;
    } 

    private Item GrabItemFromKey(string key)
    {
        switch (key)
        {
            case "camera":
                return CameraManager.Instance;
            case "soul":
                return LostSoulManager.Instance;
            case "trap":
                return TrapSigilManager.Instance;
            case "empty":
                return null;
            default:
                Debug.Log("PowerUp passed with invalid key in GetItem()");
                return null;
        }
    }
}
