using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConstructionData : MonoBehaviour
{
    [SerializeField] private LocationGridSave locationGridSave;

    [SerializeField] private GameObject holesLocation; 
    
    [SerializeField] private GameObject dirtPilesLocation; 

    [SerializeField] private GameObject pilonsLocation; 

    [SerializeField] private Sprite[] holeStages;
    [SerializeField] private Sprite[] pileStages;

    [Header("Data for the shovel:")]
    [SerializeField] private GameObject shovel;
    [SerializeField] private Transform shovelPosition;
    [SerializeField] private Direction shovelDirection;

    [Header("Data for the pilons:")]
    [SerializeField] private Transform pilonsStackLocation;
    [SerializeField] private Direction pilonDirection;
    [SerializeField] private GameObject pilonStacksLocation;

    [Header("Data for the planks:")]
    [SerializeField] private Transform planksLocation;
    [SerializeField] private Direction planksDirection;
    
    [Header("Data for the table:")]
    [SerializeField] private Transform tableLocation;
    [SerializeField] private Direction tableDirection;

    [SerializeField] private Transform helperPosition;

    private List<Vector3> positions = new List<Vector3>();

    private SpriteRenderer[] holes;
    private SpriteRenderer[] piles;
    private SpriteRenderer[] pilons;
    private SpriteRenderer[] pilonsStack;

    private BuildSystemHandler buildSystemHandler;

    private int indexOfHole = 0;
    private int indexOfSprite = 0;

    private NpcConstructionAI npcConstruction;
    private BuilderHelperAI builderHelperAI;

    private DialogueBetweenNPCs dialogueBetweenNPCs;

    public Transform ShovelPosition { get => shovelPosition; }
    public Transform PilonsLocation { get => pilonsStackLocation; }
    public Transform PlanksLocation { get => planksLocation; }
    public Transform TableLocation  { get => tableLocation;  }
    public Transform HelperPosition { get => helperPosition; }

    public BuilderHelperAI BuilderHelperAI { get => builderHelperAI; }

    public Direction ShovelDirection { get => shovelDirection; }
    public Direction PilonsDirection { get => pilonDirection; }
    public Direction PlanksDirection { get => planksDirection; }
    public Direction TableDirection { get => tableDirection; }
    public NpcConstructionAI NpcConstruction { get => npcConstruction; }

    private void Awake()
    {
        buildSystemHandler = GameObject.Find("Global/BuildSystem").GetComponent<BuildSystemHandler>();

        dialogueBetweenNPCs = GetComponent<DialogueBetweenNPCs>();

        holes =  holesLocation.GetComponentsInChildren<SpriteRenderer>();
        piles =  dirtPilesLocation.GetComponentsInChildren<SpriteRenderer>();
        pilons = pilonsLocation.GetComponentsInChildren<SpriteRenderer>();
        pilonsStack = pilonStacksLocation.GetComponentsInChildren<SpriteRenderer>();

        positions = new List<Vector3>();

        foreach(SpriteRenderer hole in holes)
        {
            positions.Add(locationGridSave.GetNeighborNode(hole.transform.position, -1, 0));
            
            hole.sprite = null;

            hole.GetComponent<BoxCollider2D>().enabled = false;
        }

        foreach(SpriteRenderer pile in piles)
        {
            pile.sprite = null;
        }

        foreach(SpriteRenderer pilon in pilons)
        {
            pilon.enabled = false;
        }
    }

    public void PickUpPilon()
    {
        if(pilonsStack != null && pilonsStack.Length > 0)
        {
            GameObject pilon =  pilonsStack[0].gameObject;

            pilonsStack = pilonsStack.Skip(1).ToArray();

            Destroy(pilon);
        }
    }

    public void SetBuilderAI(NpcConstructionAI npcConstruction)
    {
        if (npcConstruction != null)
        {
            this.npcConstruction = npcConstruction;

            if (dialogueBetweenNPCs != null)
            {
                dialogueBetweenNPCs.FirstNPC = npcConstruction.GetComponent<DialogueDisplay>();

                dialogueBetweenNPCs.StartDialogue(0);
            }
        }
    }

    public void SetHelperAI(BuilderHelperAI builderHelperAI)
    {
        if(builderHelperAI != null)
        {
            this.builderHelperAI = builderHelperAI;

            if(dialogueBetweenNPCs != null)
            {
                dialogueBetweenNPCs.SecondNPC = builderHelperAI.GetComponent<DialogueDisplay>();
            }
        }
    }

    public void ReadyForHelperPilon()
    {
        builderHelperAI.GoGetPilon();
    }

    public void PilonInPlace()
    {
        if(npcConstruction != null)
        {
            npcConstruction.HoleFill();
        }
    }

    public void PutToPile()
    {
        if (indexOfHole < piles.Length &&
           indexOfSprite - 1 >= 0 && indexOfSprite - 1 < pileStages.Length)
        {
            piles[indexOfHole].sprite = pileStages[indexOfSprite - 1];
        }
    }

    public void RemoveFromPile()
    {
        if (indexOfHole < piles.Length &&
           indexOfSprite - 1 >= 0 && indexOfSprite - 1 < pileStages.Length)
        {
            piles[indexOfHole].sprite = pileStages[indexOfSprite-- - 1];
        }
    }

    private void PutPilon()
    {
        if (indexOfHole < pilons.Length)
        {
            pilons[indexOfHole].enabled = true;
        }
    }

    public bool CheckIfLeftDirt()
    {
        if (indexOfSprite - 1 >= 0)
        {
            return true;
        }

        PutPilon();

        builderHelperAI.LetThePilon();

        npcConstruction.GoToTheNextHole();

        indexOfHole++;

        return false;
    }

    public void GetShovel()
    {
        if(shovel != null)
        {
            shovel.SetActive(false);
        }
    }

    public void PutShovel()
    {
        if (shovel != null)
        {
            shovel.SetActive(true);
        }
    }

    public Vector3 GetNextHole()
    {
        if(holes.Length != 0 && indexOfHole < holes.Length)
        {
            indexOfSprite = 0;

            return positions[indexOfHole];
        }

        return DefaulData.nullVector;
    }

    public Vector3 GetHolePositionForTheHelper()
    {
        if (indexOfHole < holes.Length)
        {
            return locationGridSave.GetNeighborNode(holes[indexOfHole].transform.position, 0, 1);
        }

        return DefaulData.nullVector;
    }

    private void SetHoleToTheGrid()
    {
        buildSystemHandler.ChangeNodeData(GetHolePosition(), false, false, false, false);
    }

    public bool WidenTheHole()
    {
        if(indexOfHole < holes.Length)
        {
            if(indexOfSprite < holeStages.Length)
            {
                holes[indexOfHole].sprite = holeStages[indexOfSprite];

                indexOfSprite++;

                if(indexOfSprite == holeStages.Length)
                {
                    SetHoleToTheGrid();

                    holes[indexOfHole].GetComponent<BoxCollider2D>().enabled = true;

                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        return false;
    }

    public Vector3 GetHolePosition()
    {
        if (indexOfHole < holes.Length)
        {
            return holes[indexOfHole].transform.position;
        }

        return DefaulData.nullVector;
    }
}
