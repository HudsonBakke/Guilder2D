using Microsoft.Xna.Framework;

namespace Guilder2D;

public interface ICombatAgent
{
    CombatInfo CombatInfo { get; }
    Rectangle Hurtbox { get; }
    Vector2 Position { get; }
}