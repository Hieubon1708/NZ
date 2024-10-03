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

    public static float RandomXDistanceByCar(float xPlus1, float xPlus2)
    {
        return Random.Range(CarController.instance.transform.position.x + xPlus1, CarController.instance.transform.position.x + xPlus2);
    }

    public static float RandomYDistanceByCar(float yPlus1, float yPlus2)
    {
        return Random.Range(CarController.instance.transform.position.y + yPlus1, CarController.instance.transform.position.y + yPlus2);
    }

    public static Vector2 ClampXYDistanceByCar(Vector2 pos, Vector2 dir, float xPlus1, float xPlus2, float yPlus1, float yPlus2)
    {
        if (pos.x + dir.x < CarController.instance.transform.position.x + xPlus1 || pos.x + dir.x > CarController.instance.transform.position.x + xPlus2) dir.x = -dir.x;
        if (pos.y + dir.y < CarController.instance.transform.position.y + yPlus1 || pos.y + dir.y > CarController.instance.transform.position.y + xPlus2) dir.y = -dir.y;
        return pos + dir;
    }
}
