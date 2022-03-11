using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CaveIndexSelect : MonoBehaviour
{
    [SerializeField] private Transform spawnLocation;
    [SerializeField] private GameObject slotPrefab;

    private CaveSystemHandler caveSystemHandler;

    public void SpawnButtons(int maxIndex, CaveSystemHandler caveSystemHandler)
    {
        if(this.caveSystemHandler == null)
        {
            this.caveSystemHandler = caveSystemHandler;
        }

        if(slotPrefab != null && spawnLocation != null && maxIndex > 0)
        {
            Button[] buttons = spawnLocation.GetComponentsInChildren<Button>();

            foreach(Button button in buttons)
            {
                Destroy(button.gameObject);
            }

            int indexOfCave = 1;

            while(indexOfCave < maxIndex)
            {
                GameObject instantiateButton = Instantiate(slotPrefab);

                instantiateButton.transform.SetParent(spawnLocation);

                int auxiliar = indexOfCave;

                instantiateButton.GetComponent<Button>().onClick.AddListener(delegate { ButtonPressed(auxiliar); });

                instantiateButton.GetComponentInChildren<TextMeshProUGUI>().text = indexOfCave.ToString();

                if(indexOfCave == 1)
                {
                    indexOfCave += 4;
                }
                else
                {
                    indexOfCave += 5;
                }
            }
        }
    }

    public void ButtonPressed(int index)
    {
        if(caveSystemHandler != null)
        {
            caveSystemHandler.TeleportToCaveWithIndex(index - 1);
        }

        gameObject.SetActive(false);
    }
}
