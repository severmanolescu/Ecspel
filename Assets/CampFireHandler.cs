using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CampFireHandler : MonoBehaviour
{
    [SerializeField] private GameObject fireAnimation;
    [SerializeField] private GameObject smokeParticles;
    [SerializeField] private Light2D fireLight;

    [SerializeField] private Sprite ashSprite;

    [Range(0, 255)]
    [SerializeField] private int maxColor;

    [SerializeField] private Item coalItem;
    [Header("Coal drop when destroy")]
    [Header("First trop: ")]
    [Header("Minimum burn procentage:")]
    [Range(0, 100)]
    [SerializeField] private int minProcentageOfBurn;
    [Header("Procentage of spawn:")]
    [Range(0, 100)]
    [SerializeField] private int maxProcentageOfBurn;
    [Header("Procentage of spawn:")]
    [Range(0, 100)]
    [SerializeField] private int procentageOfSpawn;

    private SpriteRenderer spriteRenderer;

    private SpawnItem spawnItem;

    private int colorIndex;

    private void Awake()
    {
        fireAnimation.SetActive(false);
        fireLight.gameObject.SetActive(false);

        colorIndex = maxColor;

        spawnItem = GameObject.Find("Global").GetComponent<SpawnItem>();

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DestroyFire(bool spawnCoal = false)
    {
        StopAllCoroutines();

        Destroy(fireAnimation);
        Destroy(fireLight.gameObject);
        Destroy(gameObject.GetComponent<BoxCollider2D>());

        spriteRenderer.sprite = ashSprite;
        spriteRenderer.color = Color.white;
        spriteRenderer.sortingOrder = -1;

        var emission = smokeParticles.GetComponent<ParticleSystem>().emission;

        emission.enabled = false;

        if(spawnCoal)
        {
            float burnProcentage = ((maxColor -colorIndex) * 100) / maxColor;

            if(burnProcentage >= minProcentageOfBurn && burnProcentage <= maxProcentageOfBurn)
            {
                float changeOfSpawn = Random.Range(0, 100);

                if(changeOfSpawn <= procentageOfSpawn)
                {
                    spawnItem.SpawnItems(coalItem.Copy(), 1, transform.position);
                }
            }
            
        }

        Destroy(this);
    }

    public bool FireStarted()
    {
        return colorIndex == maxColor ?  false: true;
    }

    public void StartFire()
    {
        fireAnimation.SetActive(true);
        fireLight.gameObject.SetActive(true);

        smokeParticles.GetComponent<ParticleSystem>().Play();

        StartCoroutine(Fire());
    }

    private IEnumerator Fire()
    {
        while(colorIndex > 0)
        {
            yield return new WaitForSeconds(1);

            colorIndex--;

            spriteRenderer.color = new Color(colorIndex, colorIndex, colorIndex);
        }

        DestroyFire(true);
    }
}
