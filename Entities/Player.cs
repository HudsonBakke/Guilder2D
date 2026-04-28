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
    private readonly AnimationState _animationState = new();

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
    /// The player hitbox, used for collision and combat
    /// </summary>
    public Rectangle Hitbox => new(
        (int)_position.X, 
        (int)_position.Y, 
        Width, 
        Height
    );

    /// <summary>
    /// The center of the player sprite/hitbox
    /// </summary>
    public Vector2 Center => new(
        Position.X + Width / 2f, 
        Position.Y + Height / 2f
    );

    public bool CollidesWith(IEntity other)
        => Hitbox.Intersects(other.Hitbox);

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

        _animationState.Update(
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
            sourceRectangle: _animationState.GetSpriteSource(),
            color: Color.White
        );
    }

    private class AnimationState
    {
        private Vector2 _direction;
        private Step _currentStep;
        private Step _lastStep;
        private float _timeSinceLastStep = 0.0f;
        private readonly float _stepSpeed = 0.3f;

        public AnimationState()
        {
            _direction = Vector2.Zero;
            _currentStep = Step.Neutral;
            _lastStep = Step.Right;
        }

        public void Update(GameTime gameTime, bool isMoving, Vector2 newDir)
        {
            if (isMoving)
            {
                _direction = newDir;
                _timeSinceLastStep += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_timeSinceLastStep >= _stepSpeed) 
                {
                    _timeSinceLastStep = 0.0f;
                    switch (_currentStep)
                    {
                        case Step.Left:
                            _currentStep = Step.Neutral;
                            _lastStep = Step.Left;
                            break;

                        case Step.Right:
                            _currentStep = Step.Neutral;
                            _lastStep = Step.Right;
                            break;

                        case Step.Neutral:
                            if (_lastStep == Step.Right)
                            {
                                _currentStep = Step.Left;
                            }
                            else
                            {
                                _currentStep = Step.Right;
                            }
                            _lastStep = Step.Neutral;
                            break;
                    }
                }
            }

            else
            {
                _timeSinceLastStep = 0.0f;
                _currentStep = Step.Neutral;
                _lastStep = Step.Right;
            }
        }

        public Rectangle GetSpriteSource()
        {
            Rectangle spriteSource = new(0,0,16,32);

            // Handling direction

            // Left and up
             if (_direction.X == -1 && _direction.Y == -1) spriteSource.X = 64;
            // Left
            else if (_direction.X == -1 && _direction.Y == 0) spriteSource.X = 0;
            // Left and down
            else if (_direction.X == -1 && _direction.Y == 1) spriteSource.X = 0;
            // Up
            else if (_direction.X == 0 && _direction.Y == -1) spriteSource.X = 64;
            // Not moving
            else if (_direction.X == 0 && _direction.Y == 0) spriteSource.X = 0;
            // Down
            else if (_direction.X == 0 && _direction.Y == 1) spriteSource.X = 0;
            // Right and up
            else if (_direction.X == 1 && _direction.Y == -1) spriteSource.X = 96;
            // Right
            else if (_direction.X == 1 && _direction.Y == 0) spriteSource.X = 32;
            // Right and down
            else if (_direction.X == 1 && _direction.Y == 1) spriteSource.X = 32;

            // Handling step

            if (_currentStep == Step.Neutral) spriteSource.Y = 0;
            else if (_currentStep == Step.Left) spriteSource.Y = 32;
            else if (_currentStep == Step.Right) spriteSource.Y = 64;

            return spriteSource;
        }

        private enum Step
        {
            Neutral,
            Left,
            Right
        }
    }   
}