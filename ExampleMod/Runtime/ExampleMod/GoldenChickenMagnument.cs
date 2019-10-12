using System;
using Ananke.Attachment.Core.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SignsOfLife;
using SignsOfLife.Prefabs.StaticPrefabs;
using SignsOfLife.Utils;

namespace Runtime.ExampleMod
{
    public class GoldenChickenMagnument : StaticPrefab
    {
        private static readonly int[,] TileData =
        {
            {0, 0},
            {0, 0}
        };

        public GoldenChickenMagnument(
            StaticPrefabType staticPrefabType,
            DumbGraphicsRegistry.ResourceDefinition textureResource)
            : base("Golden Chicken Magnument", TileData, staticPrefabType)
        {
            _cutOutBounds = true;
            Description = "Indicates your domain.";
            _texture = textureResource.GetRenderable().Item1;
            _currentBounds = textureResource.GetRenderable().Item2;
        }

        private Texture2D _texture;
        private Rectangle _currentBounds;

        //TODO: I believe this is INWORLD drawing
        public override void Draw(
            SpriteBatch spriteBatch,
            Vector2 screenGameFieldPosition,
            Matrix transformMatrix)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null,
                null, _scissorRasterizerState, null,
                transformMatrix);
            Rectangle scissorRectangle = spriteBatch.GraphicsDevice.ScissorRectangle;
            if (_cutOutBounds)
                HandleScissorRect(spriteBatch, screenGameFieldPosition);
            spriteBatch.Draw(_texture,
                new Vector2(Position.X, Position.Y) -
                screenGameFieldPosition, _currentBounds, Color.White);
            if (_cutOutBounds)
                spriteBatch.GraphicsDevice.ScissorRectangle = scissorRectangle;
            spriteBatch.End();
        }

        //TODO: I believe this is a PREVIEW drawing
        public override void Draw(
            SpriteBatch spriteBatch,
            float screenX,
            float screenY,
            Color color,
            Matrix transformMatrix)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null,
                null, null, null, transformMatrix);
            spriteBatch.Draw(_texture, new Vector2(screenX, screenY),
                _currentBounds, Color.White);
            spriteBatch.End();
        }

        //TODO: WUT?
        public override bool IsAtPointPreciseSlow(Vector2 globalPoint)
        {
            if (!IsAtPoint(globalPoint))
                return false;
            if (globalPoint.X < 0.0)
                globalPoint.X = Map.GetWorldBounds().Width + globalPoint.X;
            if (globalPoint.X > (double) Map.GetWorldBounds().Width)
                globalPoint.X -= Map.GetWorldBounds().Width;
            if (globalPoint.X > (double) X &&
                globalPoint.X < X + (double) Width &&
                (globalPoint.Y > (double) Y &&
                 globalPoint.Y < Y + (double) Height))
            {
                double num1 = globalPoint.X - (double) Map._screenGameFieldPosition.X;
                float num2 = globalPoint.Y - Map._screenGameFieldPosition.Y;
                float num3 = X + _animationOffset.X - Map._screenGameFieldPosition.X;
                float num4 = Y + _animationOffset.Y - Map._screenGameFieldPosition.Y;
                double num5 = num3;
                int num6 = (int) Math.Floor(num1 - num5);
                int num7 = (int) Math.Floor(num2 - (double) num4);
                if (Flipped)
                    num6 = Width - num6;
                try
                {
                    if (!TextureUtils.IsPixelOnTextureTransparent(_texture, _currentBounds.X + num6,
                        _currentBounds.Y + num7))
                        return true;
                }
                catch (Exception ex)
                {
                    Logger.Echo(ex.ToString());
                    Logger.WriteLine(ex.ToString());
                    return false;
                }
            }

            return false;
        }
    }
}