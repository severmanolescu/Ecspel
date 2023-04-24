using UnityEngine;

public class FarmPlotHandler : MonoBehaviour
{
    public Sprite drySoil;
    private Sprite wetEffect;

    public SpriteRenderer spriteRenderer;

    public int noOfDryDays;

    private bool dry = true;

    private GameObject wetObject;

    public Sprite DrySoil { get => drySoil; set => drySoil = value; }
    public int NoOfDryDays { get => noOfDryDays; set => noOfDryDays = value; }
    public Sprite WetEffect { get => wetEffect; set => wetEffect = value; }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        drySoil = spriteRenderer.sprite;

        GameObject.Find("Global/DayTimer").GetComponent<ChangeSoilsState>().AddSoil(this);
    }

    public void ChangeSprites(Sprite drySprite)
    {
        drySoil = drySprite;

        if (GameObject.Find("Global/DayTimer").GetComponent<DayTimerHandler>().Raining == false)
        {
            spriteRenderer.sprite = drySoil;
        }
        else
        {
            WetSoilChangeSprite();
        }
    }

    public void DrySoilChangeSprite()
    {
        spriteRenderer.sprite = drySoil;

        if (dry == true)
        {
            noOfDryDays++;

            if (noOfDryDays >= 2)
            {
                GameObject cropObject = GameObject.Find("Global/BuildSystem").GetComponent<BuildSystemHandler>().GetCropFromPosition(transform.position);

                if (cropObject != null)
                {
                    //cropObject.GetComponent<CropGrow>().DryCrop();
                }
            }
        }
        else
        {
            noOfDryDays = 0;

            dry = true;

            if (wetObject != null)
            {
                Destroy(wetObject);

                wetObject = null;
            }
        }
    }

    public void WetSoilChangeSprite()
    {
        if (dry == true)
        {
            wetObject = new GameObject();

            wetObject.transform.parent = transform;
            wetObject.transform.localPosition = Vector3.zero;

            wetObject.AddComponent<SpriteRenderer>();
            wetObject.GetComponent<SpriteRenderer>().sortingOrder = -1;
            wetObject.GetComponent<SpriteRenderer>().sprite = wetEffect;

            Instantiate(wetObject);

            dry = false;

            noOfDryDays = 0;
        }
    }
}
