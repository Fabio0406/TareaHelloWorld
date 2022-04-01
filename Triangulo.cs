using System;
using OpenTK;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace InicioOpenTK
{
    public class Triangulo : GameWindow
    {
        private int controladorVBuffer;

        private int SDibujo;

        private int vertexArrayHandle;

        public Triangulo()
            : base (GameWindowSettings.Default,NativeWindowSettings.Default)
        {
            // se utiliza para centrar la ventana al centro de la ventana
            this.CenterWindow(new Vector2i(1080, 720)); 
                                //tamaño o resolucion de ventana
        }

        protected override void OnResize(ResizeEventArgs e) // funcion que ajusta el tamaño
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            base.OnResize(e);
        }

        protected override void OnLoad() // funcion de cargar
        {           
           GL.ClearColor(new Color4 (1f, 0.6f, 0.1f, 0f));  // color de la vantana 
                                                            //(rojo,verde,azul,negro)

            float[] vertices = new float[]
            {
                 0.0f, 0.5f, 0f, // vertice superior (1)
                 0.5f, -0.5f, 0f,//    ||   inferior derecha (2)
                 -0.5f, -0.5f, 0f,//   ||      ||    izquierda (3)
            };

            this.controladorVBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, this.controladorVBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            this.vertexArrayHandle = GL.GenVertexArray();
            GL.BindVertexArray(this.vertexArrayHandle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, this.controladorVBuffer);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.BindVertexArray(0);



            string VShaderCode =
                @"
                #version 330 core
                layout (location = 0) in vec3 aPosition;
                layout (location = 1) in vec4 aColor
                void main()
                {
                gl_Position = vec4(aPosition, 1f);
                
                }
                ";

            string PShaderCode =
                @"
                #version 330 core 
                out vec4 PColor;
                void main()
                {   
                    PColor = vec4(1.0f,0.0f,0.0f,0.0f);
                }";

            int VShaderHandle = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VShaderHandle, VShaderCode);
            GL.CompileShader(VShaderHandle);

            int PShaderHandle = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(PShaderHandle, PShaderCode);
            GL.CompileShader(PShaderHandle);

            this.SDibujo = GL.CreateProgram();

            GL.AttachShader(this.SDibujo, VShaderHandle);
            GL.AttachShader(this.SDibujo, PShaderHandle);   

            GL.LinkProgram(this.SDibujo);

            GL.DetachShader(this.SDibujo,VShaderHandle);
            GL.DetachShader(this.SDibujo,PShaderHandle);

            GL.DeleteShader(VShaderHandle);
            GL.DeleteShader(PShaderHandle);
            base.OnLoad();
        }

        protected override void OnUnload() // funcion de descarga
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(this.controladorVBuffer);

            GL.UseProgram(0);
            GL.DeleteProgram(this.SDibujo);
            base.OnUnload();
        }

        protected override void OnUpdateFrame(FrameEventArgs args) // esto se repite 1/60
        {
            base.OnUpdateFrame(args);
        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {            
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.UseProgram(this.SDibujo);
            GL.BindVertexArray(this.vertexArrayHandle);
            GL.DrawArrays(PrimitiveType.Triangles,0,3);
            SwapBuffers(); 

            base.OnRenderFrame(args);
        }
    }
}