using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    public static InventoryManager Instance;
    public GameObject[] scrollViews;
    public GameObject inventory;
    public Item[] ItemSampleList;
    public List<Item> Items = new List<Item>();

    public Transform ItemContent;
    public GameObject InventoryItem;

    public Transform[] spawnPositions;

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
        ListItems();
    }
    public void Pickup(ItemController item)
    {
        //������Ʈ �����ϰ� �κ��� �ֱ�
        Add(item.Item);
        Destroy(item.gameObject);

        foreach (var scrollView in scrollViews)
        {
            scrollView.SetActive(false);
        }
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
        // ItemContent ������Ʈ�� �ڽ� ������Ʈ�� ��� ����
        foreach (Transform child in ItemContent)
        {
            Destroy(child.gameObject);
        }

        //�κ� ������Ʈ
        foreach (var item in Items)
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

    public void CreateObject(Item item)
    {
        int spawnPos = Random.Range(0, 7);
        GameObject newObject;
        newObject = Resources.Load("Objects/" + item.itemName) as GameObject;
        Instantiate(newObject, spawnPositions[spawnPos].position, Quaternion.identity);
        
    }
}
