using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Singleton<Inventory>
{   
    // using a struct instead of a dictionary just cuz unity doesn't show dictionaries in the editor
    [Serializable]
    public struct BootlegDictionary
    {
        public GameObject item;
        public int amount;
    }
    public BootlegDictionary[] dictionarySetup;
    public Dictionary<GameObject, int> items;   // the powerup scripts are used as keys, values are how many there are
    public GameObject activeItem;
    public PowerUp activeScript;
    public int activeIndex = 0;

    private void Start()
    {
        for (int i=0; i<dictionarySetup.Length; i++)
        {
            items.Add(dictionarySetup[i].item, dictionarySetup[i].amount);
        }
        EventManager.Instance.PowerUpCollected.AddListener(AddItem);
    }

    private void Update()
    {   
        // e is use active item
        if (Input.GetKeyDown(KeyCode.E))
        {
            //activeScript.Activate();
        }
        // q is rotate active item
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //activeIndex = (activeIndex + 1) % items.Count;
            //activeItem = items[activeIndex];
            activeScript = activeItem.GetComponent<PowerUp>();
            EventManager.Instance.ItemSwap.Invoke(activeItem);
        }
    }

    private void AddItem(GameObject newItem)
    {
        PowerUp powerUpScript = newItem.GetComponent<PowerUp>();
        if (powerUpScript.isUseable())
        {
            //items.Add(newItem);
            activeItem = newItem;
            activeScript = powerUpScript;
            //activeIndex = items.Count;
        }
    }
}
