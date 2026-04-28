using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Guilder2D;

public static class AssetPaths
{
    public static class Textures
    {
        public static class Combat
        {
            public static class CombatAnimations
            {
                public const string Swoosh = "Combat/CombatAnimations/swoosh";
            }
        }

        public static class MenuAssets
        {
            public static class Backgrounds
            {
                public const string MainMenu = "MenuAssets/Backgrounds/main-menu-background";
            }

            public static class Buttons
            {
                public const string StartGame = "MenuAssets/Buttons/start_game_button";
                public const string ExitGame = "MenuAssets/Buttons/exit_game_button";
            }
        }

        public static class SpriteMaps
        {
            public const string Player = "SpriteMaps/Player/player_sprite_map";
        }

        public static class Tilesets
        {
            public const string TestTileset = "Tilesets/test_tileset/tile_atlas";
        }

        public static class WorldAssets
        {
            public static class Objects
            {
                public const string Pot = "WorldAssets/Objects/pot";
                public const string BrokenPot = "WorldAssets/Objects/broken-pot";
                public const string Rock1 = "WorldAssets/Objects/rock-1";
            }
        }
    }

    public static class Maps
    {
        public const string TestMap = "Maps/PlacedMaps/test_map.json";
    }
}

public class AssetManager
{
    private readonly ContentManager _content;
    private readonly Dictionary<string, Texture2D> _textureCache = [];

    public AssetManager(ContentManager content)
    {
        _content = content;
    }

    public Texture2D LoadTexture(string assetName)
    {
        if (!_textureCache.TryGetValue(assetName, out Texture2D texture))
        {
            texture = _content.Load<Texture2D>(assetName);
            _textureCache[assetName] = texture;
        }

        return texture;
    }

    public Texture2D Swoosh => LoadTexture(AssetPaths.Textures.Combat.CombatAnimations.Swoosh);
    public Texture2D MainMenu => LoadTexture(AssetPaths.Textures.MenuAssets.Backgrounds.MainMenu);
    public Texture2D StartGameButton => LoadTexture(AssetPaths.Textures.MenuAssets.Buttons.StartGame);
    public Texture2D ExitGameButton => LoadTexture(AssetPaths.Textures.MenuAssets.Buttons.ExitGame);
    public Texture2D PlayerSpriteMap => LoadTexture(AssetPaths.Textures.SpriteMaps.Player);
    public Texture2D TestTileset => LoadTexture(AssetPaths.Textures.Tilesets.TestTileset);
    public Texture2D Pot => LoadTexture(AssetPaths.Textures.WorldAssets.Objects.Pot);
    public Texture2D BrokenPot => LoadTexture(AssetPaths.Textures.WorldAssets.Objects.BrokenPot);
    public Texture2D Rock1 => LoadTexture(AssetPaths.Textures.WorldAssets.Objects.Rock1);

    public PlacedMap TestMap => MapLoader.LoadMap(this, AssetPaths.Maps.TestMap);
}