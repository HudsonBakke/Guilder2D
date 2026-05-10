using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guilder2D;

/// <summary>
/// This class holds the data and methods for the player. Input is passed in via the PlayerInput struct and SendInput() method, 
/// and then the Update() and Draw() methods are called.
/// </summary>
public class Player : IEntity
{
    private readonly Texture2D _playerSprite;
    private GameInput _input;
    private float _playerSpeed = 72f;
    private readonly StepAnimationState _stepAnimationState = new();

    public Player(AssetManager assets)
    {
        _playerSprite = assets.PlayerSpriteMap;
    }

    public int Width { get; } = 16;
    public int Height { get; } = 32;

    private Vector2 _position = new(20, 20);
    /// <summary>
    /// The player's world position
    /// </summary>
    public Vector2 Position 
    { 
        get { return _position; } 
        private set { _position = value; }
    }

    /// <summary>
    /// The player hitbox, used for collision
    /// </summary>
    public Rectangle CollisionBox => new(
        (int)_position.X, 
        (int)_position.Y + Height/2, 
        Width, 
        Height/2
    );

    /// <summary>
    /// The center of the player sprite/hitbox
    /// </summary>
    public Vector2 Center => new(
        Position.X + Width / 2f, 
        Position.Y + Height / 2f
    );

    public bool CollidesWith(IEntity other)
        => CollisionBox.Intersects(other.CollisionBox);

    /// <summary>
    /// Use this method to pass keyboard/mouse/controller input to the Player object.
    /// </summary>
    /// <param name="input">Input data</param>
    public void SendInput(GameInput input)
    {
        _input = input;
    }

    /// <summary>
    /// Call this method to update the player's state. Collision is handled in this method.
    /// </summary>
    /// <param name="gameTime">MonoGame GameTime object</param>
    /// <param name="map">The map that the player is in</param>
    public void Update(GameTime gameTime, Map? map = null)
    {
        Vector2 newDir = Vector2.Zero;

        if (_input.MoveUp == PressState.Pressed ||
            _input.MoveUp == PressState.Held) 
            newDir.Y -= 1;
        if (_input.MoveDown == PressState.Pressed ||
            _input.MoveDown == PressState.Held) 
            newDir.Y += 1;
        if (_input.MoveLeft == PressState.Pressed ||
            _input.MoveLeft == PressState.Held) 
            newDir.X -= 1;
        if (_input.MoveRight == PressState.Pressed ||
            _input.MoveRight == PressState.Held) 
            newDir.X += 1;

        _stepAnimationState.Update(
            gameTime, 
            newDir != Vector2.Zero, 
            newDir
        );

        Vector2 update = newDir == Vector2.Zero
            ? Vector2.Zero
            : Vector2.Normalize(newDir);
        Vector2 newPos = Position + update * _playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (map != null)
        {
            Vector2 oldPos = Position;

            // Test X direction for collision
            _position.X = newPos.X;
            if (map.CollidesWith(this)) _position.X = oldPos.X;

            // Test Y direction for collision
            _position.Y = newPos.Y;
            if (map.CollidesWith(this)) _position.Y = oldPos.Y;
        }
        else Position = newPos;
    }

    /// <summary>
    /// Call this method to draw the player to the screen.
    /// </summary>
    /// <param name="spriteBatch">MonoGame SpriteBatch object</param>
    public void Draw(SpriteBatch spriteBatch, Camera camera)
    {
        Vector2 drawPos = camera.WorldToScreen(_position);

        spriteBatch.Draw(
            texture: _playerSprite,
            position: drawPos,
            sourceRectangle: _stepAnimationState.GetSpriteSource(),
            color: Color.White
        );
    }
}