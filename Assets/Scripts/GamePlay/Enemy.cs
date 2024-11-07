using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp;
    public int damage;
    public Bone bone;

    public int SubtractHp(int hp)
    {
        this.hp -= hp;
        if (this.hp < 0) this.hp = 0;
        return this.hp;
    }
}
