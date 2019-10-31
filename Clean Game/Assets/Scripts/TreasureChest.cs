using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    [SerializeField] private string[] possibleItems = new string[] 
    { 
        "Dagger",
        "Shield",
        "Helmet",
        "ChestPlate",
        "LegPlate",
        "Shoes",
        "2H Sowrd"
    };
    [SerializeField] private int amountOfItems;

    [SerializeField]private string[] items;

    // Start is called before the first frame update
    void Start()
    {
        amountOfItems = Random.Range(1,4);
        items = new string[amountOfItems];
        for (int i = 0; i < amountOfItems; i++)
        {
            items[i] = possibleItems[Random.Range(0,possibleItems.Length)];
        }
    }

    public string[] GetItems()
    {
        Destroy(this.gameObject);
        return items;
    }
}
