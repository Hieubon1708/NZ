using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float hp;
    public float damage;

    public void Start()
    {
        name = damage.ToString();
    }

    public float SubtractHp(float hp)
    {
        this.hp -= hp;
        if (this.hp < 0) this.hp = 0;
        return this.hp;
    }
}
