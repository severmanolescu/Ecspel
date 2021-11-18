using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectHandler : MonoBehaviour
{
    [SerializeField] private GameObject spawnPoint;

    [SerializeField] private GameObject spawnPointCanvas;
    [SerializeField] private GameObject prefabEffectCanvas;

    private List<EffectHandlerClass> effects = new List<EffectHandlerClass>();

    private PlayerStats playerStats;

    private PlayerItemUse playerItemUse;

    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();

        playerItemUse = GetComponent<PlayerItemUse>();
        
        playerMovement= GetComponent<PlayerMovement>();
    }

    public void AddEffect(Effect effect)
    {
        GameObject effectShow = new GameObject();

        effectShow.AddComponent<Image>().sprite = effect.EffectSprite;

        GameObject instatiate = Instantiate(effectShow, spawnPoint.transform);

        foreach (EffectHandlerClass handlerClass in effects)
        {
            if (handlerClass.Effect == effect)
            {
                Destroy(handlerClass.GameObject);

                Destroy(handlerClass.GameObjectCanvas);

                effects.Remove(handlerClass);

                break;
            }
        }
        Destroy(effectShow);

        GameObject canvas = Instantiate(prefabEffectCanvas, spawnPointCanvas.transform);

        canvas.GetComponent<CanvasEffectDataSet>().DataSet(effect);

        EffectHandlerClass effectHandler = new EffectHandlerClass(effect, instatiate, canvas, Time.time);
        effects.Add(effectHandler);
    }

    private void Update()
    {
        foreach(EffectHandlerClass effect in effects)
        {

            switch(effect.Effect.EffectType)
            {
                case EffectType.Poison: playerStats.TakeDamageEffect(effect.Effect.Power * Time.deltaTime); break;

                case EffectType.InstantDamage:
                    {
                        if (effect.StartEffect == false)
                        {
                            playerStats.TakeDamageEffect(effect.Effect.Power);

                            effect.StartEffect = true;
                        }

                        break;
                    }

                case EffectType.LowDamage:
                    {
                        if (effect.StartEffect == false)
                        {
                            playerItemUse.AttackDecrease = effect.Effect.Power;

                            effect.StartEffect = true;
                        }

                        break;
                    }

                case EffectType.Slow:
                    {
                        if (effect.StartEffect == false)
                        {
                            playerMovement.SlowEffect = effect.Effect.Power;

                            GetComponent<Animator>().speed /= effect.Effect.Power;

                            effect.StartEffect = true;
                        }

                        break;
                    }
                case EffectType.Fatigue:
                    {
                        if (effect.StartEffect == false)
                        {
                            effect.StartEffect = true;

                            playerMovement.FatiqueEffect = effect.Effect.Power; 
                        }

                        break;
                    }

            }

            if (Time.time >= effect.TimeStart + effect.Effect.Duration)
            {
                switch(effect.Effect.EffectType)
                {
                    case EffectType.LowDamage: playerItemUse.AttackDecrease = 1f; break;

                    case EffectType.Slow: playerMovement.SlowEffect = 1f; GetComponent<Animator>().speed *= effect.Effect.Power; break;

                    case EffectType.Fatigue: playerMovement.FatiqueEffect = 1f; break;
                }

                Destroy(effect.GameObject);
                Destroy(effect.GameObjectCanvas);

                effects.Remove(effect);
            }
            else
            {
                effect.GameObjectCanvas.GetComponent<CanvasEffectDataSet>().ChangeDuration(-(Time.time - effect.TimeStart - effect.Effect.Duration));
            }
        }
    }
}
