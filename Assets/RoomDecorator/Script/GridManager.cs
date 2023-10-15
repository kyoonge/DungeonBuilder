using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Model;

public class GridManager : MonoBehaviour {

    public static GridManager Instance;

    private Tiles tiles;
    private Sorter sorter;

    private Furniture SelectedFurniture;
    private ItemController invenFurniture;

    private bool dragging = false;

    public Transform interactBtnGroup;
    public Button placeButton;
    public Button rotateButton;
	public Button undoButton;
    public Toggle mode;
    public SpriteRenderer grids;

    //inventory
    public bool isInvenMode;
    public bool isTileMode;



    void Awake ()
    {
        Instance = this;

        sorter = GameObject.Find("Unit").GetComponent<Sorter>();
        tiles = GameObject.Find("Tiles").GetComponent<Tiles>();
    }

    void Start () {
        placeButton.onClick.AddListener(() => {
			OnPlaceFurniture(SelectedFurniture);
            sorter.SortAll();
            interactBtnGroup.gameObject.SetActive(false);
        });
        rotateButton.onClick.AddListener(() => {
                List<Tile> area;
                RotateItem(out area);
        });
        undoButton.onClick.AddListener(() => {
            OnUndo(SelectedFurniture);
            sorter.SortAll();
			interactBtnGroup.gameObject.SetActive(false);
		});
        mode.onValueChanged.AddListener(value => grids.enabled = value);

        InventoryManager.Instance.inventory.SetActive(false);
    }
	
	void Update () {

        if (!mode.isOn) //일반모드일때 리턴
            return;

        if(isInvenMode == true)
        {
            //인벤 모드일때
            mode.interactable = SelectedFurniture == null;
        
            if (Input.GetMouseButtonDown(0))
            {
                OnBeginDrag(isHold => dragging = isHold);
                Debug.Log("Click");
            }
            else if (Input.GetMouseButtonUp(0))
            {
                dragging = false;
                OnEndDrag();
            }
            if (dragging)
                OnDrag();

            if (Input.GetMouseButtonDown(1))
            {
                PutItemToInven();
            }
        }

        if(isTileMode == true)
        {
            //타일 모드일때

            //드래그

            if(Input.GetMouseButtonDown(0))
            {
                // 마우스 클릭한 위치를 스크린 좌표로 가져옴
                Vector3 clickPosition = Input.mousePosition;

                // 메인 카메라에서 마우스 클릭 위치로 레이를 쏨
                Ray ray = Camera.main.ScreenPointToRay(clickPosition);

                // 레이캐스트를 수행하고 충돌 정보를 저장
                RaycastHit hitInfo;

                if (Physics.Raycast(ray, out hitInfo))
                {
                    // 충돌한 오브젝트 가져오기
                    GameObject clickedObject = hitInfo.collider.gameObject;
                    if (clickedObject.layer != LayerMask.NameToLayer("Tile"))
                    {
                        Debug.Log("return");
                        return;
                    }

                }
                else { return; }


                Tiles.Instance.DragTileStart();
                dragging = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (dragging)
                {
                    dragging = false;
                    Tiles.Instance.DragTileEnd();
                }

            }
            if (dragging)
            {
                Tiles.Instance.DragTile();
            }

            if (Input.GetMouseButtonDown(1))
            {
                Tiles.Instance.UndoTile();
            }
        }

    }


    private void OnBeginDrag(Action<bool> isHold) {

        if (SelectedFurniture == null)
        {
            var furniture = OnSelect(child => child.transform.parent.GetComponent<Furniture>() != null);
            if (furniture != null)
            {
                SelectedFurniture = furniture.transform.parent.GetComponent<Furniture>();
                SelectedFurniture.Unplaced(); // 고정 해제
            }
            isHold(furniture != null);
            Debug.Log("1");
        }
        else
        {
            var furniture = OnSelect(child => child.transform.parent.GetComponent<Furniture>() != null);
            isHold(furniture != null && furniture.transform.parent.GetComponent<Furniture>() == SelectedFurniture);
            Debug.Log("2");
        }

    }

    //좌클릭으로 인벤에 넣는 함수
    private void PutItemToInven()
    {
        if (SelectedFurniture == null)
        {
            var furniture = OnSelect(child => child.transform.parent.GetComponent<ItemController>() != null);
            if (furniture != null)
            {
                invenFurniture = furniture.transform.parent.GetComponent<ItemController>();
                InventoryManager.Instance.Pickup(invenFurniture);
            }
        }
    }

    private void OnDrag()
    {
        if (SelectedFurniture == null)
            return;

        var tile = OnSelect(obj => obj.GetComponent<Tile>() != null);
        //타일 위일때
        if (tile != null)
        {
            interactBtnGroup.gameObject.SetActive(false);
            SelectedFurniture.Move(tile.GetComponent<Tile>());

            List<Tile> area;
            OnInvalid(SelectedFurniture, out area);
        }
        else
        {
            Debug.Log("Draging");
        }
    }

    private void OnEndDrag()
    {
        if (SelectedFurniture == null)
            return;

        var centerPoint = Camera.main.WorldToScreenPoint(SelectedFurniture.transform.position);
        interactBtnGroup.position = centerPoint;
        interactBtnGroup.gameObject.SetActive(true);

        List<Tile> area;
        placeButton.interactable = !(OnInvalid(SelectedFurniture, out area));
        undoButton.interactable = SelectedFurniture.previous != null;
    }

    private void RotateItem(out List<Tile> area)
    {
        area = new List<Tile>();
		if (SelectedFurniture != null)
        {
            SelectedFurniture.Rotate();
			placeButton.interactable = !(OnInvalid(SelectedFurniture, out area));
        }
    }

    private GameObject OnSelect(Predicate<GameObject> condition)
	{	
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hits = Physics.RaycastAll(ray, Mathf.Infinity);
        foreach (var hit in hits)
            if (condition(hit.transform.gameObject))
                return hit.transform.gameObject;
        return null;
    }

    private void OnPlaceFurniture(Furniture furniture)
    {
        if (furniture == null)
            return;

        List<Tile> area;
        if (!OnInvalid(furniture, out area))
        {
            furniture.Place(area);
            furniture.SetColor(Color.white);
            SelectedFurniture = null;
        }
    }

    private bool OnInvalid(Furniture furniture, out List<Tile> area)
    {
        area = new List<Tile>();
        for (int i = 0; i < furniture.width; i++)
        {
            for (int j = 0; j < furniture.length; j++)
            {
                var tile = tiles.GetTileByCoordinate(furniture.origin.x + j, furniture.origin.y + i);
                if (tile == null || tile.isBlock)
                {
                    furniture.SetColor(Color.red);
                    return true;
                }

                area.Add(tile);
            }
        }

        furniture.SetColor(Color.green);
        return false;
    }

	private void OnUndo (Furniture furniture)
	{
        if (furniture.previous == null)
            return;

        furniture.Move (furniture.previous.tile);
        furniture.Rotate (furniture.previous.direction);
        OnPlaceFurniture(furniture);
    }

    public void StartInvenMode()
    {
        isInvenMode = true;
        isTileMode = false;
        InventoryManager.Instance.ShutScrollView();
        InventoryManager.Instance.scrollViews[0].SetActive(true);
        InventoryManager.Instance.ItemParent.gameObject.SetActive(true);
    }

    public void StartTileMode()
    {
        isInvenMode = false;
        isTileMode = true;
        InventoryManager.Instance.ShutScrollView();
        InventoryManager.Instance.scrollViews[1].SetActive(true);
        InventoryManager.Instance.ItemParent.gameObject.SetActive(false);
    }

}
