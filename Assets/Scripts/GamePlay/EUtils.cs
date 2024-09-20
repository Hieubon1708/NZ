using UnityEngine;

public static class EUtils
{
    public static int GetIndexLine(GameObject e)
    {
        string nameLayer = LayerMask.LayerToName(e.layer);
        return int.Parse(nameLayer.Substring(nameLayer.Length - 1));
    }
}
