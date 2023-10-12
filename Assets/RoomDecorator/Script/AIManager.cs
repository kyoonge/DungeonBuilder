using Model;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AIManager : MonoBehaviour {

    private Sorter sorter;
    private Tiles tiles;

    public NavMeshAgent agent;
    public GameObject idol;
    public Toggle mode;

    void Awake()
    {
        sorter = GameObject.Find("Unit").GetComponent<Sorter>();
        tiles = GameObject.Find("Tiles").GetComponent<Tiles>();
    }

    void Update ()
    {
        var pos = agent.gameObject.transform.position;
        idol.transform.position = new Vector2(pos.x, pos.z / 2f);
        var unit = idol.GetComponent<Character>();
        var tile = tiles.GetTileByPoint(idol.transform.position);
        if (unit.origin != tile)
        {
            unit.UpdateTile(tile);
            sorter.SortUnit(unit);
        }

        idol.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = agent.gameObject.transform.eulerAngles.y < 180f;

        if (!mode.isOn)
        {
            if (Input.GetMouseButtonUp(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                var hits = Physics.RaycastAll(ray, Mathf.Infinity);
                foreach (var hit in hits)
                {
                    var selectedTile = hit.transform.gameObject.GetComponent<Tile>();
                    if (selectedTile != null)
                    {
                        var position = selectedTile.gameObject.transform.position;
                        agent.SetDestination(new Vector3(position.x, 0, position.y * 2));
                    }
                }
            }
        }

       
    }
}
