using UnityEngine;

public class Boss : MonoBehaviour
{
    public int hp;

    public int SubtractHp(int hp)
    {
        this.hp -= hp;
        if (this.hp < 0) this.hp = 0;
        return this.hp;
    }
}
