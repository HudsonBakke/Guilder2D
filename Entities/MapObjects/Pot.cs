using Microsoft.Xna.Framework.Graphics;

namespace Guilder2D;

public sealed class Pot : MapObject
{
    private Texture2D _broken;

    public Pot(AssetManager assets) 
        : base(assets.Pot, true)
    {
        _broken = assets.BrokenPot;
    }

    public override void OnSelect()
    {
        _texture = _broken;
    }

    public override void OnInteract()
    {
        throw new System.NotImplementedException();
    }
}