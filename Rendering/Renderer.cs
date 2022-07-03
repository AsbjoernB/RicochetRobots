using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using System.Numerics;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System;

namespace RicochetRobots
{
    public static class Renderer
    {
        // exists twice
        public static readonly float tilecount = 16;
        public static readonly float sidebarwidth = 8;

        public static readonly ReadOnlyDictionary<RGBY, Vector4> colors = new ReadOnlyDictionary<RGBY, Vector4>(new Dictionary<RGBY, Vector4>()
        {
            {RGBY.red, new Vector4(0.9f,0,0,1) },
            {RGBY.green, new Vector4(0,0.9f,0,1)},
            {RGBY.blue, new Vector4(0.5f,0.5f,1,1)},
            {RGBY.yellow, new Vector4(1f,1f,0.2f,1)}

        });

        public static GL gl { get; private set; }


        private static BufferObject<float> Vbo;
        private static BufferObject<uint> Ebo;
        private static VertexArrayObject<float, uint> Vao;

        private static Shader Shader;
        //Creating transforms for the transformations
        private static Transform[] Transforms = new Transform[1];



        #region camera
        private static Camera camera;

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
            options.Size = new Vector2D<int>(1280, 720);
            //options.Size = new Vector2D<int>((int)((tilecount + sidebarwidth) * tilepixels), (int)(tilecount * tilepixels));
            options.Title = "Ricochet Robots";
            Master.window = Window.Create(options);

            Master.window.Load += OnLoad;
            Master.window.Render += OnRender;
            Master.window.Closing += OnClose;
            Master.window.Resize += OnResize;
        }

        private static void OnResize(Vector2D<int> dimensions)
        {
            gl.Viewport(dimensions);
            Vector2 size = new Vector2(((float)Master.window.Size.X / (float)Master.window.Size.Y) * tilecount, tilecount);
            camera = new Camera((size / 2) - new Vector2(0.5f), size);
        }

        private static void OnLoad()
        {
            IInputContext input = Master.window.CreateInput();
            for (int i = 0; i < input.Keyboards.Count; i++)
            {
                input.Keyboards[i].KeyDown += KeyDown;
            }

            gl = GL.GetApi(Master.window);



            Ebo = new BufferObject<uint>(gl, Indices, BufferTargetARB.ElementArrayBuffer);
            Vbo = new BufferObject<float>(gl, Vertices, BufferTargetARB.ArrayBuffer);
            Vao = new VertexArrayObject<float, uint>(gl, Vbo, Ebo);

            Vao.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 5, 0);
            Vao.VertexAttributePointer(1, 2, VertexAttribPointerType.Float, 5, 3);

            Shader = new Shader(gl, "Shaders/shader.vert", "Shaders/shader.frag");


            Vector2 size = new Vector2(((float)Master.window.Size.X / (float)Master.window.Size.Y) * tilecount, tilecount);
            camera = new Camera((size / 2) - new Vector2(0.5f), size);
            //Vector2 size = new Vector2(16f / 9f * tilecount, tilecount);
            //Vector2 size = new Vector2(5);
            //camera = new Camera((size / 2) - new Vector2(0.5f), size);
            //camera = new Camera(new Vector2((Master.tilecount + Master.sidebarwidth) / 2f - 0.5f, tilecount / 2f - 0.5f), new Vector2(tilecount + sidebarwidth, tilecount));

            gl.Enable(EnableCap.Blend);
            gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            TextureBank.Init(gl);

        }


        private static unsafe void OnRender(double delta)
        {
            gl.Clear((uint)ClearBufferMask.ColorBufferBit);

            Vao.Bind();
            Shader.Use();
            Shader.SetUniform("uTexture0", 0);
        }

        public static unsafe void DrawTexture(Texture texture, Vector2 position, float rotation = 0f, float scale = 1f, float hueShift = 0, float alpha = 1)
        {
            texture.Bind();

            Shader.SetUniform("uModel", new Transform(new Vector3(position.X, position.Y, 0), rotation, scale).ViewMatrix);
            Shader.SetUniform("uCamera", camera.projectionMatrix);
            //Shader.SetUniform("uHue", hueShift);
            Shader.SetUniform("uAlpha", alpha);
            Shader.SetUniform("uTint", new Vector4(1f, 1f, 1f, 1f));


            gl.DrawElements(PrimitiveType.Triangles, (uint)Indices.Length, DrawElementsType.UnsignedInt, null);
        }
        public static unsafe void DrawTexture(Texture texture, Vector2 position, Vector4 tint, float rotation = 0f, float scale = 1f, float hueShift = 0, float alpha = 1)
        {
            texture.Bind();

            Shader.SetUniform("uModel", new Transform(new Vector3(position.X, position.Y, 0), rotation, scale).ViewMatrix);
            Shader.SetUniform("uCamera", camera.projectionMatrix);
            //Shader.SetUniform("uHue", hueShift);
            Shader.SetUniform("uAlpha", alpha);
            Shader.SetUniform("uTint", tint);


            gl.DrawElements(PrimitiveType.Triangles, (uint)Indices.Length, DrawElementsType.UnsignedInt, null);
        }


        private static void OnClose()
        {   
            Vbo?.Dispose();
            Ebo?.Dispose();
            Vao?.Dispose();
            Shader?.Dispose();
        }


        private static void KeyDown(IKeyboard arg1, Key arg2, int arg3)
        {
            if (arg2 == Key.Escape)
            {
                Master.window.Close();
            }

        }

        public static float hueShift(RGBY target)
        {
            switch (target)
            {
                case RGBY.red:
                    return 0;
                case RGBY.green:
                    return 140f/360f;
                case RGBY.blue:
                    return 230f/360f;
                case RGBY.yellow:
                    return 50f/360f;
                default:
                    return 0;
            }
        }
    }
}
