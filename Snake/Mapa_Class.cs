using System;

namespace Snake
{
    public class Mapa_Class
    {
        private int sizeC = 125;
        private int sizeX = 500;
        private int sizeY = 500;
        public bool[,] tableroOcupado { get; set; }
        public int fila { get; set; }
        public int columna { get; set; }

        public int dimensionY(int n)
        {
            sizeC = n / Math.Max(fila, columna);
            sizeX = sizeC * fila;
            return sizeX + 1;
        }

        public int dimensionX(int n)
        {
            sizeC = n / Math.Max(fila, columna);
            sizeY = sizeC * columna;
            return sizeY + 1;
        }

        public int dimensionC(int n)
        {
            sizeC = n / Math.Max(fila, columna);
            return sizeC;
        }

        public int locationY(int n)
        {
            var i = (n - sizeX) / 2;
            return i;
        }

        public int locationX(int n)
        {
            var i = (n - sizeY) / 2;
            return i;
        }
    }

    public static class Compartir_Class
    {
        public static bool[,] snake;
        public static int fila;
        public static int columna;
        public static int velocidad;
        public static int huevos;
    }
}