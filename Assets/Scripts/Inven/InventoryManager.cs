using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    public static InventoryManager Instance;
    public Item[] ItemSampleList;
    public List<Item> Items = new List<Item>();

    public Transform ItemContent;
    public GameObject InventoryItem;
    // Start is called before the first frame update

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //�ӽ÷� �κ��丮 ä���
        AddListItem(0);
        AddListItem(0);
        AddListItem(1);
        AddListItem(2);
    }
    public void Pickup(ItemController item)
    {
        Add(item.Item);
        Destroy(item.gameObject);
    }

    public void Add(Item item)
    {
        Items.Add(item);
    }
    public void Remove(Item item) 
    {
        Items.Remove(item);
    }

    public void ListItems()
    {
        //�κ� ��ư ���������� ���� ������Ʈ
        foreach(var item in Items)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            //var itemName = obj.transform.Find("ItemName").GetComponent<Text>();
            Image itemIcon = obj.transform.Find("Border/ItemImage").GetComponent<Image>();
            InventoryItem inventoryItem = obj.transform.Find("Border/ItemImage").GetComponent<InventoryItem>();
            inventoryItem.AddItem(item);
            Debug.Log(itemIcon);
            //itemName.text = item.itemName;
            itemIcon.sprite = item.icon;
        }
    }

    public void AddListItem(int index)
    {
        //Cabinet, Seat, Table
        Item newItem;
        newItem = ItemSampleList[index];
    }
}
