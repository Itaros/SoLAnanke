using System;
using System.Collections.Generic;
using System.IO;
using SignsOfLife;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ananke.Attachment.Core.Graphics
{
    public class DumbGraphicsRegistry
    {

        public DumbGraphicsRegistry()
        {
            SpaceGame.GetGraphicsDevice().DeviceReset += OnReset;
        }

        private void OnReset(object sender, EventArgs e)
        {
            foreach (var definition in _definitions)
            {
                Load(definition);
            }
        }

        public ResourceDefinition DefineResource(FileInfo pathToTexture, Rectangle rectangle)
        {
            ResourceDefinition def = new ResourceDefinition(pathToTexture, rectangle);
            _definitions.Add(def);
            Load(def);
            return def;
        }

        private void Load(ResourceDefinition def)
        {
            Texture2D sprite;
            using (FileStream fileStream =
                new FileStream(def.PathToTexture.FullName, FileMode.Open))
            {
                sprite = Texture2D.FromStream(SpaceGame.GetGraphicsDevice(), fileStream);
            }
            def.SetRenderable(sprite);
        }

        private List<ResourceDefinition> _definitions = new List<ResourceDefinition>();

        public delegate void OnGraphicsReloadHandler(ResourceDefinition definition);
        
        public class ResourceDefinition
        {
            public ResourceDefinition(FileInfo pathToTexture, Rectangle rectangle)
            {
                PathToTexture = pathToTexture;
                Area = rectangle;
            }
            
            public FileInfo PathToTexture { get; }
            public Rectangle Area { get; }

            public event OnGraphicsReloadHandler OnGraphicsReload;
            
            private Texture2D _texture;
            
            public Tuple<Texture2D, Rectangle> GetRenderable()
            {
                return new Tuple<Texture2D, Rectangle>(_texture, Area);
            }

            internal void SetRenderable(Texture2D texture)
            {
                _texture?.Dispose();
                _texture = texture;
                OnGraphicsReload?.Invoke(this);
            }
            
        }
        
    }
}