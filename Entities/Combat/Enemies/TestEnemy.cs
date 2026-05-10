using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guilder2D;

public class TestEnemy : Enemy
{
    private StepAnimationState _stepAnimationState = new();

    public TestEnemy(AssetManager assets)
        : base(assets.PlayerSpriteMap) 
    { 
        _position.X = 20; 
        _position.Y = 20; 
    }

    public override int Width { get; } = 16;
    public override int Height { get; } = 32;

    public override void Update(GameTime gameTime, Map map)
    {
        _targetPos = map.Entities.GlobalEntities
            .OfType<Player>()
            .FirstOrDefault()
            ?.Position ?? _targetPos;

        Vector2 newPos = _position + FacingDirection * _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (map != null)
        {
            Vector2 oldPos = _position;

            // Test X direction for collision
            _position.X = newPos.X;
            if (map.CollidesWith(this)) _position.X = oldPos.X;

            // Test Y direction for collision
            _position.Y = newPos.Y;
            if (map.CollidesWith(this)) _position.Y = oldPos.Y;
        }
        else _position = newPos;

        _stepAnimationState.Update(
            gameTime, 
            FacingDirection != Vector2.Zero, 
            FacingDirection
        );
    }

    public override void Draw(SpriteBatch spriteBatch, Camera camera)
    {
        Vector2 drawPos = camera.WorldToScreen(_position);

        spriteBatch.Draw(
            texture: _texture,
            position: drawPos,
            sourceRectangle: _stepAnimationState.GetSpriteSource(),
            color: Color.White
        );
    }
}