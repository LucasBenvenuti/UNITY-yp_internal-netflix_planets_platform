using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class InterestPoint : MonoBehaviour, IPointerExitHandler
{
    public enum Type
    {
        Estrelas, DIY, FeiraDeCiencias, Jogo
    }

    public Type interestPointType;

    public string interestPointName;

    public bool canBeClicked = false;

    public string url = "klaus";
    public string urlAccessibility = "https://player.vimeo.com/video/460152085";

    [SerializeField] Animator animator;
    [SerializeField] Planet planet;
    [SerializeField] TextMeshProUGUI typeText;

    //can be game or video
    public string type = "game";

    [Tooltip("Only usable if it's a game")]
    public string orientation = "portrait"; //portrait ou landscape 

    [SerializeField] Button button;
    public VideoClip videoClip;
    public VideoPlayer videoPlayer;
    public string newScene;

    void Awake()
    {
        if (button != null)
        {
            button.onClick.AddListener(() => StartCoroutine(HandleButtonClick()));
            if (interestPointType == Type.Estrelas)
            {
                IncreaseStarButtonsSize();
            }
        }

        BoxCollider collider = gameObject.AddComponent(typeof(BoxCollider)) as BoxCollider;
        collider.center = new Vector3(0.0997626185f, 0.34070453f, -0.456716657f);
        collider.size = new Vector3(2.44348955f, 1.68140829f, 2.70773053f);

        typeText.text = GetTypeName();
    }

    private void Update()
    {
        this.transform.LookAt(2 * transform.position - MousePan.instance.transform.position);
        LookForInterestPoints();
    }

    public string GetTypeName()
    {
        switch (interestPointType)
        {
            case Type.Estrelas:
                if (planet.planetName == "mundomisterio")
                {
                    return "Estrelas";
                }
                else
                {
                    return "Estrela";
                }
            case Type.DIY:
                return "Faça Você Mesmo";
            case Type.FeiraDeCiencias:
                return "Feira de Ciências";
            case Type.Jogo:
                return "Jogo";
            default:
                return "";
        }
    }


    void IncreaseStarButtonsSize()
    {
        RectTransform rt = button.GetComponent<RectTransform>();
        rt.localScale = new Vector3(1.5f, 1.5f, 1.5f);
    }
    IEnumerator HandleButtonClick()
    {
        Debug.Log("TOUCHED");

        yield return null;

        if (type == "game")
        {
            Debug.Log("Games not available at the moment.");

            // if (SceneController.instance)
            // {
            //     SceneController.instance.Close("FiniteMode", 1f);
            // }
        }
        else
        {
            VideoController.instance.ShowUI(true, videoClip);
        }
    }

    //USED ON ANIMATION EVENTS - InterestAppear animation
    public void CanBeClicked()
    {
        canBeClicked = !canBeClicked;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("MouseOver", false);
    }

    void LookForInterestPoints()
    {
        if (planet.showingInterestPoints && !MousePan.instance.isGoingBack)
        {
            Vector3 direction = (transform.position - Camera.main.transform.position).normalized;

            float angle = Vector3.Dot(Camera.main.transform.forward, direction);
            if (angle > 0.985f && angle <= 1f)
            {
                if (StartSceneBttn.finishedAnimation)
                {
                    Ray ray = new Ray(Camera.main.transform.position, direction);
                    RaycastHit hitInfo;
                    bool hitted = Physics.SphereCast(ray, 1f, out hitInfo, 9999f);

                    if (hitted)
                    {
                        if (GameObject.ReferenceEquals(hitInfo.collider.gameObject, gameObject))
                        {
                            animator.SetBool("MouseOver", true);
                        }
                    }

                }
            }
            else
            {
                animator.SetBool("MouseOver", false);
            }
        }
        else
        {
            animator.SetBool("MouseOver", false);
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, .25f);
    }
#endif
}
