using UnityEngine;

public static class EUtils
{
    public static int GetIndexLine(GameObject e)
    {
        string nameLayer = LayerMask.LayerToName(e.layer);
        return int.Parse(nameLayer.Substring(nameLayer.Length - 1));
    }

    public static float GetAngle(Vector2 dir)
    {
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }
}
