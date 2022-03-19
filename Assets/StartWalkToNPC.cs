using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartWalkToNPC : MonoBehaviour
{
    [SerializeField] private GameObject Npc;

    [SerializeField] private Vector3 positionToSpawn;

    [SerializeField] private List<NpcTimeSchedule> npcTimeSchedules;

    [SerializeField] private int seconds;

    [SerializeField] private bool stopPlayerForMoving;

    private bool spawned = false;

    private PlayerMovement playerMovement;
    private CanvasTabsOpen canvas;

    public List<NpcTimeSchedule> NpcTimeSchedules { get => npcTimeSchedules; set => npcTimeSchedules = value; }
    public GameObject NPC { get => Npc; set => Npc = value; }
    public Vector3 PositionToSpawn { get => positionToSpawn; set => positionToSpawn = value; }
    public int Seconds { get => seconds; set => seconds = value; }
    public bool StopPlayerForMoving { get => stopPlayerForMoving; set => stopPlayerForMoving = value; }

    private IEnumerator WaitForSeconds()
    {
        yield return new WaitForSeconds(Seconds);

        if (StopPlayerForMoving == true)
        {
            playerMovement.TabOpen = false;
            canvas.canOpenTabs = true;
        }

        Destroy(NPC);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && spawned == false)
        {
            NpcAIHandler npcAI = Instantiate(Npc).GetComponent<NpcAIHandler>();

            spawned = true;

            npcAI.transform.position = positionToSpawn;

            if (npcAI != null)
            {
                foreach (NpcTimeSchedule npcTimeSchedule in npcTimeSchedules)
                {
                    GameObject toLocation = new GameObject();
                    toLocation.transform.position = npcTimeSchedule.Position;

                    npcTimeSchedule.Location = toLocation.transform;

                    npcTimeSchedule.Point = npcAI.transform;
                }

                npcAI.gameObject.SetActive(true);

                npcAI.GetNpcPath();

                npcAI.ScheduleIndex = -1;
                npcAI.NpcTimeSchedules = npcTimeSchedules;
                npcAI.ChangeScheduleIndex();

                NPC = npcAI.gameObject;

                if(StopPlayerForMoving == true)
                {
                    playerMovement = GameObject.Find("Global/Player").GetComponent<PlayerMovement>();
                    canvas =  GameObject.Find("Global/Player/Canvas").GetComponent<CanvasTabsOpen>();

                    playerMovement.TabOpen = true;
                    canvas.canOpenTabs = false;
                }

                StartCoroutine(WaitForSeconds());
            }
        }
    }
}
