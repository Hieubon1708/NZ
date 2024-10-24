using Coffee.UIExtensions;
using UnityEngine;

public class TutorialOject : MonoBehaviour
{
    public RectTransform target;
    public GameObject hand;
    public GameObject lig;

    public void EnabledTutorial(bool isEnable, Unmask unmask, GameObject unmaskObj)
    {
        hand.SetActive(isEnable);
        if (lig != null)
        {
            lig.SetActive(isEnable);
            unmask.fitTarget = target;
            unmaskObj.SetActive(isEnable);
        }
    }
}
