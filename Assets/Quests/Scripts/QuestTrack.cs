using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections.Generic;

public class QuestTrack : MonoBehaviour
{
    private Vector3 targetPosition = Vector3.zero;

    private Camera mainCamera;
    private Sprite arrowSprite;
    private Sprite crossSprite;

    private Image pointerImage;
    private RectTransform pointerRectTransform;

    private void Awake()
    {
        mainCamera = GameObject.Find("Player/UICamera").GetComponent<Camera>();

        arrowSprite = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Quests/Sprites/QuestArrow.png", typeof(Sprite));
        crossSprite = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Quests/Sprites/QuestCross.png", typeof(Sprite));

        pointerImage = gameObject.GetComponentInChildren<Image>();
        pointerRectTransform = pointerImage.gameObject.GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (targetPosition != Vector3.zero)
        {
            Vector3 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(targetPosition);

            bool isOffScreen = targetPositionScreenPoint.x <= DefaulData.borderSizeQuestTrack ||
                               targetPositionScreenPoint.x >= Screen.width - DefaulData.borderSizeQuestTrack ||
                               targetPositionScreenPoint.y <= DefaulData.borderSizeQuestTrack ||
                               targetPositionScreenPoint.y >= Screen.height - DefaulData.borderSizeQuestTrack;

            if (isOffScreen)
            {
                RotatePointerTorwardsTargetPosition();

                pointerImage.sprite = arrowSprite;

                Vector3 cappedTargetScreenPosition = targetPositionScreenPoint;

                if (cappedTargetScreenPosition.x <= DefaulData.borderSizeQuestTrack)
                {
                    cappedTargetScreenPosition.x = DefaulData.borderSizeQuestTrack;
                }
                if (cappedTargetScreenPosition.x >= Screen.width - DefaulData.borderSizeQuestTrack)
                {
                    cappedTargetScreenPosition.x = Screen.width - DefaulData.borderSizeQuestTrack;
                }
                if (cappedTargetScreenPosition.y <= DefaulData.borderSizeQuestTrack)
                {
                    cappedTargetScreenPosition.y = DefaulData.borderSizeQuestTrack;
                }
                if (cappedTargetScreenPosition.y >= Screen.height - DefaulData.borderSizeQuestTrack)
                {
                    cappedTargetScreenPosition.y = Screen.height - DefaulData.borderSizeQuestTrack;
                }

                Vector3 pointerWorldPosition = mainCamera.ScreenToWorldPoint(cappedTargetScreenPosition);

                pointerRectTransform.position = pointerWorldPosition;
                pointerRectTransform.localPosition = new Vector3(pointerRectTransform.localPosition.x, pointerRectTransform.localPosition.y, 0f);

                pointerImage.gameObject.SetActive(true);
            }
            else
            {
                pointerImage.sprite = crossSprite;

                Vector3 pointerWorldPosition = mainCamera.ScreenToWorldPoint(targetPositionScreenPoint);

                pointerRectTransform.position = pointerWorldPosition;
                pointerRectTransform.localPosition = new Vector3(pointerRectTransform.localPosition.x, pointerRectTransform.localPosition.y, 0f);

                pointerRectTransform.localEulerAngles = Vector3.zero;

                pointerImage.gameObject.SetActive(true);
            }
        }
        else
        {
            pointerImage.gameObject.SetActive(false);
        }
    }

    private void RotatePointerTorwardsTargetPosition()
    {
        Vector3 toPosition = targetPosition;
        Vector3 fromPosition = Camera.main.transform.position;
        fromPosition.z = 0f;
        Vector3 direction = (toPosition - fromPosition).normalized;

        float angle = DefaulData.GetAngleFromVectorFloat(direction);

        pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle - 90);
    }

    public void TrackQuest(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    public void StopTrackQuest()
    {
        targetPosition = Vector3.zero;
    }
}
