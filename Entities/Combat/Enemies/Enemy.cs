using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guilder2D;

public abstract class Enemy : IEntity
{
    protected readonly Texture2D _texture; // May be a single sprite but more than likely is a sprite sheet
    protected Vector2 _targetPos;
    protected float _speed = 45f;

    public Enemy(Texture2D texture)
    {
        _texture = texture;
    }

    public virtual int Width { get; }
    public virtual int Height { get; }

    protected Vector2 _position;
    /// <summary>
    /// The enemy's world position
    /// </summary>
    public Vector2 Position 
    { 
        get { return _position; } 
        protected set { _position = value; }
    }

    public Rectangle CollisionBox => new(
        (int)_position.X,
        (int)_position.Y,
        Width,
        Height
    );

    /// <summary>
    /// Normalized vector pointing in the direction that the enemy is facing
    /// </summary>
    public Vector2 FacingDirection
        => Vector2.Normalize(_targetPos - _position);

    public bool CollidesWith(IEntity other)
        => CollisionBox.Intersects(other.CollisionBox);

    public abstract void Update(GameTime gameTime, Map map); // AI logic will be defined here in children classes

    public abstract void Draw(SpriteBatch spriteBatch, Camera camera);
}