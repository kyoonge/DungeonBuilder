using Model;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System.Collections;

public class AStarPathfinding : MonoBehaviour
{
    public Tile[,] pathTiles; // 타일 배열
    public Vector2Int startTileCoordinates = new Vector2Int(9, 1);
    public Vector2Int endTileCoordinates = new Vector2Int(1, 9);

    private List<Vector2Int> FindPath(Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        Dictionary<Vector2Int, float> gScore = new Dictionary<Vector2Int, float>();
        Dictionary<Vector2Int, float> fScore = new Dictionary<Vector2Int, float>();

        List<Vector2Int> openSet = new List<Vector2Int>();
        openSet.Add(start);
        gScore[start] = 0;
        fScore[start] = HeuristicCostEstimate(start, end);

        while (openSet.Count > 0)
        {
            Vector2Int current = GetLowestFScore(openSet, fScore);
            if (current == end)
            {
                path = ReconstructPath(cameFrom, current);
                break;
            }

            openSet.Remove(current);

            foreach (Vector2Int neighbor in GetNeighbors(current))
            {
                float tentativeGScore = gScore[current] + 1; // Assuming each move costs 1 (adjust if needed)

                if (pathTiles[neighbor.x, neighbor.y].isTrack && (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor]))
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + HeuristicCostEstimate(neighbor, end);

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        return path;
    }

    private float HeuristicCostEstimate(Vector2Int current, Vector2Int goal)
    {
        return Mathf.Abs(current.x - goal.x) + Mathf.Abs(current.y - goal.y); // Manhattan distance
    }

    private Vector2Int GetLowestFScore(List<Vector2Int> openSet, Dictionary<Vector2Int, float> fScore)
    {
        float lowestFScore = float.MaxValue;
        Vector2Int lowestFNode = openSet[0];

        foreach (Vector2Int node in openSet)
        {
            if (fScore.ContainsKey(node) && fScore[node] < lowestFScore)
            {
                lowestFScore = fScore[node];
                lowestFNode = node;
            }
        }

        return lowestFNode;
    }

    private List<Vector2Int> GetNeighbors(Vector2Int current)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        // Add adjacent tiles (up, down, left, right) as neighbors.
        if (current.x > 0) neighbors.Add(new Vector2Int(current.x - 1, current.y));
        if (current.x < pathTiles.GetLength(0) - 1) neighbors.Add(new Vector2Int(current.x + 1, current.y));
        if (current.y > 0) neighbors.Add(new Vector2Int(current.x, current.y - 1));
        if (current.y < pathTiles.GetLength(1) - 1) neighbors.Add(new Vector2Int(current.x, current.y + 1));

        return neighbors;
    }

    private List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        path.Add(current);

        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Insert(0, current);
        }

        return path;
    }

    void Start()
    {
        
    }

    public void FindingPath()
    {
        pathTiles = Tiles.Instance.tiles;
        List<Vector2Int> path = FindPath(startTileCoordinates, endTileCoordinates);

        if (path.Count > 0)
        {
            AIManager.Instance.animator.Play("walk");
            StartCoroutine(MoveToNextCoordinate(path));

            // Path found, you can now move the player along the path.
            //foreach (Vector2Int tileCoordinates in path)
            //{
            //    Debug.Log("path: " + tileCoordinates);
            //    // Move the player to the tile at tileCoordinates.
            //    Vector2 pos = pathTiles[tileCoordinates.x, tileCoordinates.y].transform.position;
            //    AIManager.Instance.idol.transform.DOMove(pos,0);
            //}
            
        }
        else
        {
            Debug.Log("no path");
            // No path found, handle this case.
            UIManager.Instance.PrintWarning();
        }
    }

    private IEnumerator MoveToNextCoordinate(List<Vector2Int> _paths)
    {
        
        foreach (Vector2Int tileCoordinates in _paths)
        {
            Vector2 pos = pathTiles[tileCoordinates.x, tileCoordinates.y].transform.position;

            // Ray를 쏴서 몬스터 확인
            bool isMonsterInWay = CheckForMonster(pos);

            if (isMonsterInWay)
            {

                // 몬스터가 있는 경우 0.2초 동안 대기
                yield return new WaitForSeconds(0.2f);
                AIManager.Instance.idol.GetComponent<SpriteRenderer>().color = Color.white;
            }
            //else
            //{
            AIManager.Instance.idol.transform.DOMove(new Vector3(pos.x, pos.y, 0), 0.3f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(0.3f);
            //}
        }
        yield return new WaitForSeconds(0.3f);
        ResetPlayer();
    }

    public void ResetPlayer()
    {
        AIManager.Instance.animator.Play("Idle");
        AIManager.Instance.idol.transform.position = AIManager.Instance.resetPosition;
        UIManager.Instance.startButton.SetActive(true);
        UIManager.Instance.InitHP();
    }

    private bool CheckForMonster(Vector2 position)
    {
        // 몬스터를 감지할 상자 영역을 계산
        Vector2 playerPosition = (Vector2)AIManager.Instance.idol.transform.position;
        Vector2 direction = position - playerPosition;
        float distance = direction.magnitude;
        Vector2 boxSize = new Vector2(distance, 1.0f); // 상자 영역 크기 (가로는 거리, 세로는 감지 범위)


        // 상자 영역 내에 있는 모든 충돌체를 검색
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(playerPosition + direction * 0.5f, boxSize, 0f);


        foreach (Collider2D collider in hitColliders)
        {
            if (collider.CompareTag("Monster"))
            {
                if(collider.transform.GetComponent<ItemController>().Item.itemType == Item.ItemType.Present)
                {
                    if(collider.transform.GetComponent<ItemController>().Item.isUsed == false)
                    {
                        AIManager.Instance.idol.GetComponent<SpriteRenderer>().color = Color.green;
                        UIManager.Instance.IncreaseHP(collider.transform.GetComponent<ItemController>().Item.Demage);
                        Debug.Log("Present detected!");
                        collider.transform.GetComponent<ItemController>().Item.isUsed = true;
                        return true;
                    }
                    else
                    {
                        collider.transform.GetComponent<ItemController>().Item.isUsed = false;
                    }
                }
                else
                {
                    AIManager.Instance.idol.GetComponent<SpriteRenderer>().color = Color.red;
                    UIManager.Instance.DecreaseHP(collider.transform.GetComponent<ItemController>().Item.Demage);
                    Debug.Log("Monster detected!");
                    return true;
                }

            }
        }

        return false;
    }

}
