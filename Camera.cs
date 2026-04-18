using System;
using Microsoft.Xna.Framework;

namespace Guilder2D;

/// <summary>
/// Defines a camera entity that can follow the player around
/// </summary>
public class Camera
{
    public Camera() {}

    /// <summary>
    /// The camera position in world space
    /// </summary>
    public Vector2 Position { get; private set; }

    /// <summary>
    /// Makes the camera attempt to follow the target position (usually the player)
    /// </summary>
    public void Follow(
        Vector2 targetPosition,
        int viewportWidth,
        int viewportHeight,
        int mapWidth,
        int mapHeight)
    {
        float x = targetPosition.X - viewportWidth / 2f;
        float y = targetPosition.Y - viewportHeight / 2f;

        x = MathHelper.Clamp(x, 0, mapWidth - viewportWidth);
        y = MathHelper.Clamp(y, 0, mapHeight - viewportHeight);

        Position = new Vector2(x, y);
    }

    /// <summary>
    /// Converts a world position to a pixel-snapped screen position
    /// </summary>
    public Vector2 WorldToScreen(Vector2 worldPosition)
    {
        Vector2 screen = worldPosition - Position;
        return new Vector2(
            MathF.Round(screen.X),
            MathF.Round(screen.Y)
        );
    }
}