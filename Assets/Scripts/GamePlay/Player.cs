using UnityEngine;

public class Player : MonoBehaviour
{
    public float hp;
    public int gold;

    public PlayerSkiner playerSkiner;

    public float SubtractHp(float hp)
    {
        this.hp -= hp;
        if (this.hp < 0) this.hp = 0;
        return this.hp;
    }

    public void SubtractGold(int gold)
    {
        this.gold -= gold;
        Debug.LogWarning(this.gold);
    }
    
    public void PlusGold(int gold)
    {
        this.gold += gold;
    }
}
