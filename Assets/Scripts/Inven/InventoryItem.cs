using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public Item item;
    public Button button;

    private void Start()
    {
        button.onClick.AddListener(()=>UseItem(this.item));
    }

    public void RemoveItem()
    {
        InventoryManager.Instance.Remove(item);

        // ���� ������Ʈ�� �θ� ������Ʈ�� Transform ������Ʈ ��������
        GameObject grandparentTransform = transform.parent.parent.gameObject;

            if (grandparentTransform != null)
            {
                Destroy(grandparentTransform);
            }
            else
            {
                Debug.Log("�θ��� �θ� ������Ʈ�� �����ϴ�.");
            }

    }


    public void AddItem(Item newItem)
    {
        item = newItem;
    }

    //�������� ��ġ �Ϸ�������
    public void UseItem(Item item)
    {
        switch (item.itemType)
        {
            case Item.ItemType.Furniture:
                UIManager.Instance.IncreaseCost(item.cost);
                break;
            case Item.ItemType.Monster:
                UIManager.Instance.IncreaseCost(item.cost);
                break;
            case Item.ItemType.Present:
                UIManager.Instance.IncreaseCost(item.cost);
                break;
        }
        RemoveItem();
        InventoryManager.Instance.CreateObject(item);
    }
}
