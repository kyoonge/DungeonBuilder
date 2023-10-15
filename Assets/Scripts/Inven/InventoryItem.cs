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

        // 현재 오브젝트의 부모 오브젝트의 Transform 컴포넌트 가져오기
        GameObject grandparentTransform = transform.parent.parent.gameObject;

            if (grandparentTransform != null)
            {
                Destroy(grandparentTransform);
            }
            else
            {
                Debug.Log("부모의 부모 오브젝트가 없습니다.");
            }

    }


    public void AddItem(Item newItem)
    {
        item = newItem;
    }

    //아이템을 배치 완료했을때
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
