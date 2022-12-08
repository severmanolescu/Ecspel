using UnityEngine;

public class QuestCutTreesHandler : MonoBehaviour
{
    public void SetCutTreesQuest(Quest quest)
    {
        GameObject prefabGameObject = new GameObject();

        GameObject @object = Instantiate(prefabGameObject);

        @object.AddComponent<QuestCutTrees>().SetQuest((CutTrees)quest);
    }
}
