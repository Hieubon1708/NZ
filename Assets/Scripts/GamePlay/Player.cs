using UnityEngine;

public class Player : MonoBehaviour
{
    public float hp;

    public float SubtractHp(float hp)
    {
        this.hp -= hp;
        if (this.hp < 0) this.hp = 0;
        return this.hp;
    }
}
