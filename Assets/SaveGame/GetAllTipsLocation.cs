using System.Collections.Generic;
using UnityEngine;

public class GetAllTipsLocation : MonoBehaviour
{
    [SerializeField] private GameObject locationOfTips;

    [SerializeField] private List<Tip> tips;

    public Tip GetTipByNO(int tipNo)
    {
        foreach (Tip tip in tips)
        {
            if (tip.tipNo == tipNo)
            {
                return tip;
            }
        }

        return null;
    }

    public List<TipsSave> GetAllTips()
    {
        List<TipsSave> saveTips = new List<TipsSave>();

        TipShow[] tipShows = locationOfTips.GetComponentsInChildren<TipShow>();

        foreach (TipShow tip in tipShows)
        {
            TipsSave tipsSave = new TipsSave();

            tipsSave.TipID = tip.Tip.tipNo;

            tipsSave.PositionX = tip.transform.localPosition.x;
            tipsSave.PositionY = tip.transform.localPosition.y;

            BoxCollider2D boxCollider = tip.GetComponent<BoxCollider2D>();

            if (boxCollider != null)
            {
                tipsSave.ColliderSizeX = boxCollider.size.x;
                tipsSave.ColliderSizeY = boxCollider.size.y;

                saveTips.Add(tipsSave);
            }
        }

        return saveTips;
    }

    public void SetTipToWorld(List<TipsSave> tipsSave)
    {
        TipShow[] tipShows = locationOfTips.GetComponentsInChildren<TipShow>();

        foreach (TipShow tipShow in tipShows)
        {
            Destroy(tipShow.gameObject);
        }

        foreach (TipsSave tip in tipsSave)
        {
            GameObject tipObject = new GameObject();

            tipObject.AddComponent<BoxCollider2D>();
            tipObject.AddComponent<TipShow>();

            tipObject.transform.parent = locationOfTips.transform;

            tipObject.transform.localPosition = new Vector3(tip.PositionX, tip.PositionY);

            BoxCollider2D collider2D = tipObject.GetComponent<BoxCollider2D>();

            collider2D.size = new Vector2(tip.ColliderSizeX, tip.ColliderSizeY);

            collider2D.isTrigger = true;

            tipObject.GetComponent<TipShow>().Tip = GetTipByNO(tip.TipID);
        }
    }
}
