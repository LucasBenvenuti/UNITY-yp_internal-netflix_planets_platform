using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class PlanetHover : MonoBehaviour
{
    [SerializeField] SpriteRenderer logoSpriteRenderer;
    [SerializeField] Image characterImageRenderer;
    [SerializeField] Animator spriteAnimator;
    [SerializeField] Animator characterSpriteAnimator;
    [SerializeField] Sprite logoSprite;
    [SerializeField] Sprite characterSprite;
    [SerializeField] Transform logoWrapper;
    public bool canShowLogo = true;

    bool isShowingLogo;

    //     [DllImport("__Internal")]
    //     private static extern bool IsMobile();

    //     public static bool isMobile()
    //     {
    // #if !UNITY_EDITOR && UNITY_WEBGL
    //              return IsMobile();
    // #endif
    //         return false;
    //     }
    // Start is called before the first frame update
    void Start()
    {
        logoSpriteRenderer.sprite = logoSprite;
        characterImageRenderer.sprite = characterSprite;
    }

    // Update is called once per frame
    void Update()
    {
        logoWrapper.LookAt(Camera.main.transform);

        // if (isMobile())
        // {
        Vector3 direction = (transform.position - Camera.main.transform.position).normalized;

        float angle = Vector3.Dot(Camera.main.transform.forward, direction);

        if (angle > 0.95f && angle <= 1f)
        {
            if (StartSceneBttn.finishedAnimation)
                ShowLogo();
        }
        else
        {
            HideLogo();
        }
        // }

        if (spriteAnimator.GetCurrentAnimatorStateInfo(0).IsName("LogoOn") != isShowingLogo)
        {
            if (isShowingLogo)
            {
                spriteAnimator.SetBool("Visible", true);
                characterSpriteAnimator.SetBool("Visible", true);
            }
            else
            {
                spriteAnimator.SetBool("Visible", false);
                characterSpriteAnimator.SetBool("Visible", false);
            }
        }

    }

    public void ShowLogo()
    {
        if (isShowingLogo || !canShowLogo)
        {
            return;
        }
        isShowingLogo = true;
        spriteAnimator.SetBool("HasHovered", true);
        spriteAnimator.SetBool("Visible", true);

        characterSpriteAnimator.SetBool("HasHovered", true);
        characterSpriteAnimator.SetBool("Visible", true);

    }

    public void HideLogo()
    {
        if (!isShowingLogo)
        {
            return;
        }
        isShowingLogo = false;
        spriteAnimator.SetBool("Visible", false);

        characterSpriteAnimator.SetBool("Visible", false);

    }

    // private void OnMouseOver()
    // {
    //     spriteAnimator.SetBool("HasHovered", true);
    //     ShowLogo();
    // }

    // private void OnMouseExit()
    // {
    //     HideLogo();
    // }
}
