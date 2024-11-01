using UnityEngine;

public class Boss : MonoBehaviour
{
    public void Play()
    {
        UIHandler.instance.tutorial.TutorialButtonPlayBoss(true);
    }
}
