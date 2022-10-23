using UnityEngine;

public class FarmPlotHandler : MonoBehaviour
{
    public Sprite drySoil;
    public Sprite wetSoil;

    private SpriteRenderer spriteRenderer;

    public int noOfDryDays;

    private bool dry = true;

    public Sprite DrySoil { get => drySoil; set => drySoil = value; }
    public Sprite WetSoil { get => wetSoil; set => wetSoil = value; }
    public int NoOfDryDays { get => noOfDryDays; set => noOfDryDays = value; }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); 

        drySoil = spriteRenderer.sprite;

        GameObject.Find("Global/DayTimer").GetComponent<ChangeSoilsState>().AddSoil(this);
    }

    public void ChangeSprites(Sprite dry, Sprite wet)
    {
        drySoil = dry;
        wetSoil = wet;

        if(dry)
        {
            spriteRenderer.sprite = dry;
        }
        else
        {
            spriteRenderer.sprite = wet;
        }
    }

    public void DrySoilChangeSprite()
    {
        spriteRenderer.sprite = drySoil;

        if (dry == true)
        {
            noOfDryDays++;

            if(noOfDryDays >= 2)
            {
                GameObject cropObject = GameObject.Find("Global/BuildSystem").GetComponent<BuildSystemHandler>().GetCropFromPosition(transform.position);

                if (cropObject != null)
                {
                    cropObject.GetComponent<CropGrow>().DryCrop();
                }
            }
        }
        else
        {
            noOfDryDays = 0;

            dry = true;
        }
    }

    public void WetSoilChangeSprite()
    {
        spriteRenderer.sprite = wetSoil;

        dry = false;

        noOfDryDays = 0;
    }
}
