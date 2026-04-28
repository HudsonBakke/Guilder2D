using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guilder2D;

public interface IEntity
{
    public int Width { get; }
    public int Height { get; }
    public Vector2 Position { get; }

    /// <summary>
    /// The entity's hitbox
    /// </summary>
    public Rectangle Hitbox { get; }

    public bool CollidesWith(IEntity other);

    /// <summary>
    /// This method is overriden for each entity type to update its state each frame
    /// </summary>
    /// <param name="gameTime">MonoGame GameTime object</param>
    /// <param name="map">Map object (can be null)</param>
    public abstract void Update(GameTime gameTime, Map? map);

    /// <summary>
    /// This method is overriden for each entity type to draw it to the virtual screen
    /// </summary>
    /// <param name="spriteBatch">MonoGame SpriteBatch object</param>
    /// <param name="camera">Camera object</param>
    public abstract void Draw(SpriteBatch spriteBatch, Camera camera);
}