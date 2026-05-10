using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guilder2D;

/// <summary>
/// Defines all types of entities (concrete implementations only)
/// </summary>
public enum EntityType
{
    Player,

    MapObject_Pot,

    Enemy_TestEnemy
}

/// <summary>
/// Maintains a reference to all entities, allowing easy lookup, and allowing entities to easily interact with each other
/// </summary>
public class EntityManager
{
    private readonly List<IEntity> _entities = [];
    private readonly List<IEntity> _spawnQueue = [];
    private readonly List<IEntity> _despawnQueue = [];

    /// <summary>
    /// List of all entities currently loaded in the world
    /// </summary>
    public IReadOnlyList<IEntity> GlobalEntities => _entities;

    /// <summary>
    /// Use this method to spawn a new entity. You must pass in an already created entity using the factory
    /// </summary>
    /// <param name="entity">The entity to be spawned in</param>
    public void Spawn(IEntity entity)
    {
        _spawnQueue.Add(entity);
    }

    /// <summary>
    /// Use this method to despawn an entity. You must search for the entity that you want to despawn 
    /// in GlobalEntities, and pass in that specific object.
    /// </summary>
    /// <param name="entity">The entity to be despawned</param>
    public void Despawn(IEntity entity)
    {
        _despawnQueue.Add(entity);
    }

    /// <summary>
    /// Updates every entity in the list
    /// </summary>
    /// <param name="gameTime">MonoGame GameTime object</param>
    /// <param name="map">Map object</param>
    public void UpdateAll(GameTime gameTime, Map map)
    {
        FlushEntityChanges();

        foreach (IEntity entity in _entities)
        {
            entity.Update(gameTime, map);
        }
    }

    private void FlushEntityChanges()
    {
        foreach (IEntity entity in _despawnQueue)
        {
            _entities.Remove(entity);
        }
        _despawnQueue.Clear();

        if (_spawnQueue.Count > 0)
        {
            _entities.AddRange(_spawnQueue);
            _spawnQueue.Clear();
        }
    }

    /// <summary>
    /// Draws every entity to the screen
    /// </summary>
    /// <param name="spriteBatch">MonoGame SpriteBatch object</param>
    /// <param name="camera">Camera object</param>
    public void DrawAll(SpriteBatch spriteBatch, Camera camera)
    {
        foreach (IEntity entity in GlobalEntities)
        {
            entity.Draw(spriteBatch, camera);
        }
    }
}

/// <summary>
/// Factory used to create new entity objects
/// </summary>
public static class EntityFactory
{
    /// <summary>
    /// Generate a new entity based on the type
    /// </summary>
    /// <param name="assets">AssetManager object</param>
    /// <param name="type">Entity type</param>
    /// <returns>New entity</returns>
    public static IEntity GetEntity(AssetManager assets, EntityType type)
    {
        return type switch
        {
            EntityType.Player => new Player(assets),
            EntityType.MapObject_Pot => new Pot(assets),
            EntityType.Enemy_TestEnemy => new TestEnemy(assets),
            _ => throw new System.Exception("Didn't define an entity type.")
        };
    }

    /// <summary>
    /// Generate a new entity based on save state data
    /// </summary>
    /// <param name="assets">AssetManager object</param>
    /// <param name="data">Entity data</param>
    /// <returns>New entity</returns>
    public static IEntity GetFromEntityData(AssetManager assets, EntityData data)
    {
        throw new System.NotImplementedException();
    }
}

/// <summary>
/// Used for loading entities from save files, will be implemented later
/// </summary>
public class EntityData {}