using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFootstepSoundEffect : MonoBehaviour
{
    [SerializeField] private AudioClip footstepSound1;
    [SerializeField] private AudioClip footstepSound2;

    [SerializeField] private AudioClip currentSound1Wood;
    [SerializeField] private AudioClip currentSound2Wood;

    [SerializeField]  private AudioClip currentSound1;
    [SerializeField]  private AudioClip currentSound2;
     
    private FootPrintHandler footPrintHandler;

    private void Awake()
    {
        footPrintHandler = GameObject.Find("Global/Player").GetComponent<FootPrintHandler>(); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {            
            footPrintHandler.Footstep  = footstepSound1;
            footPrintHandler.Footstep1 = footstepSound2;

            footPrintHandler.FootstepWood = footstepSound1;
            footPrintHandler.FootstepWood1 = footstepSound2;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            footPrintHandler.Footstep  = currentSound1;
            footPrintHandler.Footstep1 = currentSound2;

            footPrintHandler.FootstepWood = currentSound1Wood;
            footPrintHandler.FootstepWood1 = currentSound2Wood;
        }
    }
}
