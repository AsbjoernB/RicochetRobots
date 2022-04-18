using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using System;
using System.Numerics;

namespace RicochetRobots
{
    public static class Renderer
    {

        public static GL gl { get; private set; }

        private static BufferObject<float> Vbo;
        private static BufferObject<uint> Ebo;
        private static VertexArrayObject<float, uint> Vao;
        private static Texture TileTex;
        private static Texture WallTex;
        private static Texture RobotTex;
        private static Shader Shader;
        //Creating transforms for the transformations
        private static Transform[] Transforms = new Transform[1];



        #region camera
        private static Camera camera;
        private static float tilecount = 16;
        private static float sidebarwidth = 8;
        private static float tilepixels = 50;
        #endregion

        private static readonly float[] Vertices =
        {
            //X    Y      Z     U   V
             0.5f,  0.5f, 0.0f, 1f, 0f,
             0.5f, -0.5f, 0.0f, 1f, 1f,
            -0.5f, -0.5f, 0.0f, 0f, 1f,
            -0.5f,  0.5f, 0.5f, 0f, 0f
        };

        private static readonly uint[] Indices =
        {
            0, 1, 3,
            1, 2, 3
        };

        public static void Init()
        {
            var options = WindowOptions.Default;
            options.Size = new Vector2D<int>((int)((tilecount + sidebarwidth) * tilepixels), (int)(tilecount * tilepixels));
            options.Title = "Ricochet Robots";
            Program.window = Window.Create(options);

            Program.window.Load += OnLoad;
            Program.window.Render += OnRender;
            Program.window.Closing += OnClose;
        }

        private static void OnLoad()
        {
            IInputContext input = Program.window.CreateInput();
            for (int i = 0; i < input.Keyboards.Count; i++)
            {
                input.Keyboards[i].KeyDown += KeyDown;
            }

            gl = GL.GetApi(Program.window);

            Ebo = new BufferObject<uint>(gl, Indices, BufferTargetARB.ElementArrayBuffer);
            Vbo = new BufferObject<float>(gl, Vertices, BufferTargetARB.ArrayBuffer);
            Vao = new VertexArrayObject<float, uint>(gl, Vbo, Ebo);

            Vao.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 5, 0);
            Vao.VertexAttributePointer(1, 2, VertexAttribPointerType.Float, 5, 3);

            Shader = new Shader(gl, "Shaders/shader.vert", "Shaders/shader.frag");

            TileTex = new Texture(gl, "Sprites/tile.png");
            WallTex = new Texture(gl, "Sprites/wall.png");
            RobotTex = new Texture(gl, "Sprites/robot.png");


            camera = new Camera(new Vector2((tilecount + sidebarwidth) / 2f - 0.5f, tilecount / 2f - 0.5f), new Vector2(tilecount + sidebarwidth, tilecount));

            //Unlike in the transformation, because of our abstraction, order doesn't matter here.
            //Translation.
            Transforms[0] = new Transform();
            Transforms[0].Position = new Vector3(0f, 0f, 0f);

            gl.Enable(EnableCap.Blend);
            gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            TextureBank.Init(gl);

        }

        private static unsafe void OnRender(double obj)
        {
            gl.Clear((uint)ClearBufferMask.ColorBufferBit);

            Vao.Bind();
            Shader.Use();
            Shader.SetUniform("uTexture0", 0);

            Gameplay.Draw();
        }

        public static unsafe void DrawTexture(Texture texture, Vector2 position, float rotation = 0f, float scale = 1f, float hueShift = 0)
        {
            texture.Bind();

            Shader.SetUniform("uModel", new Transform(new Vector3(position.X, position.Y, 0), rotation, scale).ViewMatrix);
            Shader.SetUniform("uCamera", camera.projectionMatrix);
            Shader.SetUniform("uHue", hueShift);


            gl.DrawElements(PrimitiveType.Triangles, (uint)Indices.Length, DrawElementsType.UnsignedInt, null);
        }


        private static void OnClose()
        {
            Vbo.Dispose();
            Ebo.Dispose();
            Vao.Dispose();
            Shader.Dispose();
            TileTex.Dispose();
        }


        private static void KeyDown(IKeyboard arg1, Key arg2, int arg3)
        {
            if (arg2 == Key.Escape)
            {
                Program.window.Close();
            }
            if (arg2 == Key.A)
            {
                Console.WriteLine("A");
            }
        }

        public static float hueShift(RGBY target)
        {
            switch (target)
            {
                case RGBY.red:
                    return 0;
                case RGBY.green:
                    return xMath.DegreesToRadians(125);
                case RGBY.blue:
                    return xMath.DegreesToRadians(230);
                case RGBY.yellow:
                    return xMath.DegreesToRadians(55);
                default:
                    return 0;
            }
        }
    }
}
