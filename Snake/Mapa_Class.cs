using System;

namespace Snake
{
    public class Mapa_Class
    {
        int sizeX = 500;
        int sizeY = 500;
        int sizeC = 125;
        public bool[,] tableroOcupado { get; set; }
        public int fila { get; set; }
        public int columna { get; set; }

        public int dimensionY(int n)
        {
            sizeC = n / (Math.Max(this.fila, this.columna));
            sizeX = sizeC * this.fila;
            return sizeX + 1;
        }

        public int dimensionX(int n)
        {
            sizeC = n / (Math.Max(this.fila, this.columna));
            sizeY = sizeC * this.columna;
            return sizeY + 1;
        }

        public int dimensionC(int n)
        {
            sizeC = n / (Math.Max(this.fila, this.columna));
            return sizeC;
        }

        public int locationY(int n)
        {
            int i = (n - sizeX) / 2;
            return i;
        }

        public int locationX(int n)
        {
            int i = (n - sizeY) / 2;
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