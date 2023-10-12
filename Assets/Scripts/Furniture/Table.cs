using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour, IInventoryItem
{
#region PublicVariables
    private Sprite _image = null;

    public Sprite Image
    {
        get
        {
            return _image;
        }
    }

    public void OnPickup()
    {
        //ToDo: Add logic what happens when axe is picked up by player
        gameObject.SetActive(false);
    }
#endregion
#region PrivateVariables
#endregion
#region PublicMethod    
    public string Name
    {
        get
        {
            return "Table";
        }
    }
#endregion



#region PrivateMethod
#endregion
}