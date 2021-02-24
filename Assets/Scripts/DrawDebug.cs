using Math2D;
using UnityEngine;

public static class DrawDebug
{
    static Vector2 upLeft = Vector2.up + Vector2.left;
    static Vector2 upRight = Vector2.up + Vector2.right;
    static Vector2 downLeft = Vector2.down + Vector2.left;
    static Vector2 downRight = Vector2.down + Vector2.right;

    public static void DrawBox2D(Vector3 center, Vector2 size, Color color)
    {
        Vector2 topLeft = center + (upLeft * size / 2).ToVector3OnXY(0);
        Vector2 topRight = center + (upRight * size / 2).ToVector3OnXY(0);
        Vector2 bottomLeft = center + (downLeft * size / 2).ToVector3OnXY(0);
        Vector2 bottomRight = center + (downRight * size / 2).ToVector3OnXY(0);

        Debug.DrawLine(topLeft, topRight, color);
        Debug.DrawLine(topRight, bottomRight, color);
        Debug.DrawLine(bottomRight, bottomLeft, color);
        Debug.DrawLine(bottomLeft, topLeft, color);
    }
}
