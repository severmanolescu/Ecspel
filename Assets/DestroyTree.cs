using UnityEngine;
using UnityEditor;

public class DestroyTree : MonoBehaviour
{
    [SerializeField] private Item sapling;

    private GameObject itemWorld;

    private int spawn;

    public int Spawn { set { spawn = value; } }
    public GameObject ItemWorld { get { return itemWorld; } }

    private void Awake()
    {
        itemWorld = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Items/ItemWorld.prefab", typeof(GameObject));
    }

    public void Destroy()
    {
        switch(spawn)
        {
            case 1:
                {
                    ItemWorld game = Instantiate(itemWorld).GetComponent<ItemWorld>();

                    Vector3 position = GetComponentInParent<Transform>().position;

                    position.x -= 1f;

                    game.transform.position = position;

                    game.SetItem(DefaulData.GetItemWithAmount(DefaulData.log, 2));
                    game.MoveToPoint();

                    game = Instantiate(itemWorld).GetComponent<ItemWorld>();

                    game.transform.position = position;

                    game.SetItem(DefaulData.GetItemWithAmount(sapling, Random.Range(2, 3)));
                    game.MoveToPoint();

                    break;
                }

            case 2:
                {
                    ItemWorld game = Instantiate(itemWorld).GetComponent<ItemWorld>();

                    Vector3 position = GetComponentInParent<Transform>().position;

                    position.x += 2f;

                    game.transform.position = position;

                    game.SetItem(DefaulData.GetItemWithAmount(DefaulData.log, 2));
                    game.MoveToPoint();

                    game = Instantiate(itemWorld).GetComponent<ItemWorld>();

                    game.transform.position = position;

                    game.SetItem(DefaulData.GetItemWithAmount(sapling, Random.Range(2, 3)));
                    game.MoveToPoint();

                    break;
                }
        }

        Destroy(this.gameObject);
    }
}
