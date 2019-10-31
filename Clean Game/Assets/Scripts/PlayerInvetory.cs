using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInvetory : MonoBehaviour
{
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [Space]
    [SerializeField] private Text inventoryText;

    [SerializeField] private List<string> items = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        Interact();
    }

    void Interact()
    {
        if (Input.GetKeyDown((interactKey)))
        {
            LayerMask mask = LayerMask.GetMask("Interactable");
            Collider[] colliders = Physics.OverlapSphere(transform.position, 1.0f, mask);
            if (colliders.Length > 0)
            {
                Collider colliderHit = colliders[0];
                if (colliderHit.tag == "TreasureChest")
                {
                    TreasureChest treasureChest = colliderHit.GetComponent<TreasureChest>();
                    AddItems(treasureChest.GetItems());
                }
            }
        }
    }

    void AddItems(string[] items)
    {
        foreach (var item in items)
        {
            AddItem(item);
        }
    }

    void AddItem(string item)
    {
        items.Add(item);
        UpdateUI();
    }

    void UpdateUI()
    {
        string inventoryString = "Inventory:\n";
        foreach (var item in items)
        {
            inventoryString += item + "\n";
        }
        inventoryText.text = inventoryString;
    }
}
