using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RicochetRobots
{

    /// <summary>
    /// WIP class, for now a simple projeciton matrix is used. Might be completely changed later
    /// </summary>
    public class Camera
    {
        public Vector2 FocusPosition { get; set; }
        public Vector2 Size { get; set; }

        public Camera(Vector2 FocusPosition, Vector2 Size)
        {
            this.FocusPosition = FocusPosition;
            this.Size = Size;
        }

        public Matrix4x4 projectionMatrix => Matrix4x4.CreateOrthographic(Size.X, Size.Y, 0, 100) * Matrix4x4.CreateTranslation(new Vector3(-FocusPosition / (Size / 2), 0));
    }
}
