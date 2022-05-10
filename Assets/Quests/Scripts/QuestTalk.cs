using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest/New Quest Talk", order = 1)]
public class QuestTalk : Quest
{
    [Header("Requirement quest:")]
    public int npcId = -1;

    public int NpcId { get { return npcId; } set { npcId = value; } }
}
