using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RicochetRobots
{
    public static class TextureBank
    {
        public static Dictionary<string, Texture> Textures = new Dictionary<string, Texture>();

        public static void Init(GL gl)
        {
            Textures.Add("tile", new Texture(gl, "Sprites/tile.png"));
            Textures.Add("wall", new Texture(gl, "Sprites/wall.png"));
            Textures.Add("robot", new Texture(gl, "Sprites/robot2.png"));

            // items
            Textures.Add("star", new Texture(gl, "Sprites/star.png"));
            Textures.Add("sun", new Texture(gl, "Sprites/sun.png"));
            Textures.Add("planet", new Texture(gl, "Sprites/planet.png"));
            Textures.Add("moon", new Texture(gl, "Sprites/moon.png"));
        }

    }
}
