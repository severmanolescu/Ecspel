using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewRecipeHandler : MonoBehaviour
{
    private List<Sprite> sprites = new List<Sprite>();

    private Animator animator;

    [SerializeField] private Image itemImage;

    private bool startedAnimation = false;

    private int currentIndex = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void AddNewRecipie(Sprite sprite)
    {
        if (sprite != null)
        {
            sprites.Add(sprite);

            ChangeSprite();
        }
    }

    private void ChangeSprite()
    {
        if(currentIndex == 0 && startedAnimation == false && currentIndex < sprites.Count)
        {
            startedAnimation = true;

            itemImage.sprite = sprites[currentIndex];

            animator.SetBool("Start", true);

            StartCoroutine(WaitForAnimation());

            currentIndex++;
        }
    }

    public void SetNextSpriteBoolToTrue()
    {
        startedAnimation = false;
    }

    private IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(2);

        animator.SetBool("Start", false);
    }

    private void Update()
    {
        if(startedAnimation == false)
        {
            if (currentIndex < sprites.Count)
            {
                itemImage.sprite = sprites[currentIndex];

                currentIndex++;

                startedAnimation = true;

                animator.SetBool("Start", true);

                StartCoroutine(WaitForAnimation());
            }
            else
            {
                currentIndex = 0;

                sprites.Clear();
            }
        }
    }
}
