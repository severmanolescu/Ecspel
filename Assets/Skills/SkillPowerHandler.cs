using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillPowerHandler : MonoBehaviour
{
    private List<Image> images = new List<Image>();

    private TextMeshProUGUI levelText;

    [SerializeField] private Color notUpgradedColor;

    public void Awake()
    {
        levelText = GetComponentInChildren<TextMeshProUGUI>();

        levelText.text = "0";

        images = GetComponentsInChildren<Image>().ToList();
        images.RemoveAt(0);
        images.RemoveAt(images.Count - 1);

        foreach (Image image in images)
        {
            image.color = notUpgradedColor;
        }
    }

    public void ChangeLevel(int level)
    {
        if (level > 0)
        {
            for (int indexOfImages = 0; indexOfImages < level; indexOfImages++)
            {
                if (indexOfImages < images.Count)
                {
                    images[indexOfImages].color = Color.white;
                }
            }

            for (int indexOfImages = level; indexOfImages < images.Count; indexOfImages++)
            {
                if (indexOfImages < images.Count)
                {
                    images[indexOfImages].color = notUpgradedColor;
                }
            }

            levelText.text = (level).ToString();
        }
        else
        {
            foreach (Image image in images)
            {
                image.color = notUpgradedColor;
            }
        }
    }
}
