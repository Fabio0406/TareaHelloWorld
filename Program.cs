using System;
namespace InicioOpenTK
{
    class program
    {
        static void Main (string[] agrs)
        {
            using(Triangulo HELLOWORLD = new Triangulo())
            {
                HELLOWORLD.Run(); 
            }
        }
    }
}
