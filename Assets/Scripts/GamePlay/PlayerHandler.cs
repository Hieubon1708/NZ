using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public Player playerInfo;
    public HealthHandler healthHandler;

    public void OnEnable()
    {
        healthHandler.SetTotalHp(playerInfo.hp);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        float hp = playerInfo.SubtractHp(1000);
        healthHandler.SubtractHp(hp);
        if (hp == 0)
        {

        }
    }
}
