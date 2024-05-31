using UnityEngine;

[System.Serializable]
public class DialogueChoose
{
    public Dialogue dialogue;

    [Header("-1: anything\n" +
            " 0: clear\n" +
            " 1: rain\n" +
            " 2: fog\n" +
            " 3: rain + fog\n")]
    [Range(-1, 3)]
    public int weatherType;

    [Range(0, 23)]
    public int hourOfStart;
    [Range(0, 23)]
    public int hourOfEnd;
}
