using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region PublicVariables
    #endregion
    #region PrivateVariables
    private const int SLOTS = 5;

    private List<IInventoryItem> mItems = new List<IInventoryItem>();

    public event EventHandler<InventoryEventArgs> ItemAdded;

    public void AddItem(IInventoryItem item)
    {
        if (mItems.Count < SLOTS)
        {
            //Collider collider = (item as MonoBehaviour).GetComponent<Collider>();
            //if (collider.enabled)
            //{
            //    collider.enabled = false;
            //    mItems.Add(item);
            //    item.OnPickup();

            //    if(ItemAdded != null)
            //    {
            //        ItemAdded(this, new InventoryEventArgs(item));
            //    }
            //}

            mItems.Add(item);
            item.OnPickup();

            if(ItemAdded != null)
            {
                ItemAdded(this, new InventoryEventArgs(item));
            }
        }
    }
#endregion
#region PublicMethod
#endregion
#region PrivateMethod
#endregion
}