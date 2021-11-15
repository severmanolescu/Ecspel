using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectHandlerClass
{
    private Effect effect;

    private GameObject gameObject;
    private GameObject gameObjectCanvas;

    private bool startEffect;

    private float timeStart;

    public EffectHandlerClass(Effect effect, GameObject gameObject, GameObject gameObjectCanvas, float timeStart)
    {
        this.effect = effect;
        this.gameObject = gameObject;
        this.gameObjectCanvas = gameObjectCanvas;
        this.timeStart = timeStart;
        this.StartEffect = false;
    }

    public Effect Effect { get => effect; set => effect = value; }
    public GameObject GameObject { get => gameObject; set => gameObject = value; }
    public float TimeStart { get => timeStart; set => timeStart = value; }
    public bool StartEffect { get => startEffect; set => startEffect = value; }
    public GameObject GameObjectCanvas { get => gameObjectCanvas; set => gameObjectCanvas = value; }
}
