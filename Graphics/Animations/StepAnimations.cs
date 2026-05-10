using Microsoft.Xna.Framework;

namespace Guilder2D;

public class StepAnimationState
{
    private Vector2 _direction;
    private Step _currentStep;
    private Step _lastStep;
    private float _timeSinceLastStep = 0.0f;
    private readonly float _stepSpeed = 0.3f;

    public StepAnimationState()
    {
        _direction = Vector2.Zero;
        _currentStep = Step.Neutral;
        _lastStep = Step.Right;
    }

    /// <summary>
    /// Call this to update the animation
    /// </summary>
    /// <param name="gameTime">MonoGame GameTime object</param>
    /// <param name="isMoving">Whether or not the entity is moving</param>
    /// <param name="newDir">Heading direction</param>
    public void Update(GameTime gameTime, bool isMoving, Vector2 newDir)
    {
        if (isMoving)
        {
            _direction = newDir;

            // Clamp magnitude of _direction's components to 1
            if (_direction.X > 0) _direction.X = 1;
            else if (_direction.X < 0) _direction.X = -1;

            if (_direction.Y > 0) _direction.Y = 1;
            else if (_direction.Y < 0) _direction.Y = -1;

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

    /// <summary>
    /// Call this to get the source rectangle for rendering
    /// </summary>
    /// <returns>The source rectangle on the sprite sheet</returns>
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