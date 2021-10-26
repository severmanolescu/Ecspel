using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    private CharacterPathfindingMovementHandler character;

    private Transform transformPlayer;

    private Pathfinding pathfinding;

    // Start is called before the first frame update
    void Start()
    {
        pathfinding = new Pathfinding(20, 10);

        transformPlayer = GameObject.Find("Player").GetComponent<Transform>();

        character = GetComponent<CharacterPathfindingMovementHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
            pathfinding.GetGrid().GetXY(transformPlayer.position, out int x, out int y);

            List<PathNode> path = pathfinding.FindPath(0, 0, x, y);

            if (path != null)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Debug.Log(new Vector3(path[i].x, path[i].y) + " " + new Vector3(path[i + 1].x, path[i + 1].y));

                    GetComponent<AIPathFinding>().MoveToLocation(new Vector3(path[i].x, path[i].y), 3);
                }
            }
        //}
    }
}
