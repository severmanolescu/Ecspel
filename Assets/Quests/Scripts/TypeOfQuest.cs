using System;

[Serializable]
public class TypeOfQuest
{
    public Quest quest;
    public GoToLocation goToLocation;
    public GiveItem giveItem;
    public CutTrees cutTrees;

    public Quest Quest
    {
        get
        {
            if (quest = null)
            {
                return quest;
            }
            else if (goToLocation != null)
            {
                return goToLocation;
            }
            else if (giveItem != null)
            {
                return giveItem;
            }
            else if (cutTrees != null)
            {
                return cutTrees;
            }

            return null;
        }
    }
}
