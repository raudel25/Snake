using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Snake
{
    public partial class Form2 : Form
    {
        bool cerrarform1 = true;
        bool Start_Game = false;
        int puntuacion = 0;
        int crecer = 0;
        string direccion = "";
        int indice = 0;
        int[] serpX = new int[10000];
        int[] serpY = new int[10000];
        int[] huevosX = new int[Compartir_Class.huevos];
        int[] huevosY = new int[Compartir_Class.huevos];
        bool[] huevosVisible = new bool[Compartir_Class.huevos];
        int[] huevosvalor = new int[Compartir_Class.huevos];

        int huevos;
        int canthuevosAct;
        int velocidad;
        Graphics g;
        Mapa_Class mapa = new Mapa_Class();
        bool[,] snake = new bool[10000, 10000];
        bool[,] muros = new bool[10000, 10000];
        int muros1 = 0;
        PictureBox Act = new PictureBox();
        PictureBox[] Serp = new PictureBox[10000];
        PictureBox[] HuevoPic = new PictureBox[Compartir_Class.huevos];
        Form1 main = new Form1();

        public Form2(Form1 newform)
        {
            main = newform;
            InitializeComponent();
            pictureBox1.Visible = false;
            Juego();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
        }

        private void Label3_Click(object sender, EventArgs e)
        {
            Form2 hola = new Form2(main);
            cerrarform1 = false;
            hola.Show();
            this.Close();
        }


        private void Label2_Click(object sender, EventArgs e)
        {
            main.Show();
            cerrarform1 = false;
            this.Close();
        }

        private void Form2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                if (!Start_Game)
                {
                    Start_Game = true;
                    direccion = "up";
                    timer1.Start();
                    crecer++;
                }

                if (direcAct() != "down")
                {
                    direccion = "up";
                }
            }

            if (e.KeyCode == Keys.Right)
            {
                if (!Start_Game)
                {
                    Start_Game = true;
                    direccion = "right";
                    timer1.Start();
                    crecer++;
                }

                if (direcAct() != "left")
                {
                    direccion = "right";
                }
            }

            if (e.KeyCode == Keys.Down)
            {
                if (!Start_Game)
                {
                    Start_Game = true;
                    direccion = "down";
                    timer1.Start();
                    crecer++;
                }

                if (direcAct() != "up")
                {
                    direccion = "down";
                }
            }

            if (e.KeyCode == Keys.Left)
            {
                if (!Start_Game)
                {
                    Start_Game = true;
                    direccion = "left";
                    timer1.Start();
                    crecer++;
                }

                if (direcAct() != "right")
                {
                    direccion = "left";
                }
            }

            if (e.KeyCode == Keys.A)
            {
                timer1.Stop();
            }
        }

        public void Juego()
        {
            label2.Visible = false;
            label3.Visible = false;
            label1.Text = "Puntuación: " + puntuacion + "";
            mapa.fila = Compartir_Class.fila;
            mapa.columna = Compartir_Class.columna;
            velocidad = Compartir_Class.velocidad;
            timer1.Interval = 1000 / velocidad;
            huevos = Compartir_Class.huevos;
            for (int i = 0; i < mapa.fila; i++)
            {
                for (int j = 0; j < mapa.columna; j++)
                {
                    snake[i, j] = Compartir_Class.snake[i, j];
                    muros[i, j] = Compartir_Class.snake[i, j];
                    if (!muros[i, j])
                    {
                        muros1++;
                    }
                }
            }

            Construir_Mapa();
            PintarHuevos();
            GenerarComida();
            Construir_SerpienteRND();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            Start_Game = true;

            Moverse();
        }

        public void Construir_Mapa()
        {
            PictureBox pic = new PictureBox();

            pic.Size = new Size(mapa.dimensionX(600), mapa.dimensionY(600));
            pic.Location = new Point(100 + mapa.locationX(600), 80 + mapa.locationY(600));
            pic.Paint += Pintar;
            Act = pic;
            this.Controls.Add(pic);
        }

        private void Pintar(object sender, PaintEventArgs e)
        {
            g = e.Graphics;
            Pen p = new Pen(Color.Black, 1);
            for (int i = 0; i <= mapa.columna; i++)
            {
                g.DrawLine(p, i * mapa.dimensionC(600), 0, i * mapa.dimensionC(600), mapa.fila * mapa.dimensionC(600));
            }

            for (int i = 0; i <= mapa.fila; i++)
            {
                g.DrawLine(p, 0, i * mapa.dimensionC(600), mapa.dimensionC(600) * mapa.columna,
                    i * mapa.dimensionC(600));
            }

            for (int i = 0; i < mapa.fila; i++)
            {
                for (int j = 0; j < mapa.columna; j++)
                {
                    if (!muros[i, j])
                    {
                        SolidBrush brush = new SolidBrush(Color.Brown);
                        g.FillRectangle(brush, j * mapa.dimensionC(600) + 1, i * mapa.dimensionC(600) + 1,
                            mapa.dimensionC(600) - 1, mapa.dimensionC(600) - 1);
                    }
                }
            }
        }

        public void Construir_SerpienteRND()
        {
            int[] coorX = new int[10000];
            int[] coorY = new int[10000];
            int[] direcX = {0, -1, 0, 1};
            int[] direcY = {1, 0, -1, 0};
            string[] direcCons = new string[625 * 4];
            int iteartor = 0;
            for (int x = 0; x < mapa.fila; x++)
            {
                for (int y = 0; y < mapa.columna; y++)
                {
                    if (snake[x, y])
                    {
                        bool e = true;
                        for (int j = 0; j < huevos; j++)
                        {
                            if (huevosX[j] == x && huevosY[j] == y)
                            {
                                e = false;
                            }
                        }

                        if (e)
                        {
                            coorX[iteartor] = x;
                            coorY[iteartor] = y;
                            iteartor++;
                        }
                    }
                }
            }

            int i = Random(iteartor);
            serpX[0] = coorX[i];
            serpY[0] = coorY[i];
            snake[coorX[i], coorY[i]] = false;
            PintarSerpiente(indice);
            PintarMover();
        }

        public void PintarSerpiente(int n)
        {
            PictureBox pic1 = new PictureBox();
            pic1.Size = new Size(mapa.dimensionC(600) - 1, mapa.dimensionC(600) - 1);
            if (n == 0)
            {
                Bitmap vieja = (Bitmap) Snake.Properties.Resources.cabezaArriba;
                Bitmap nueva = new Bitmap(vieja, mapa.dimensionC(600) - 1, mapa.dimensionC(600) - 1);
                pic1.Image = nueva;
            }
            else
            {
                Bitmap vieja = (Bitmap) Snake.Properties.Resources.CuerpoSerp;
                Bitmap nueva = new Bitmap(vieja, mapa.dimensionC(600) - 1, mapa.dimensionC(600) - 1);
                pic1.Image = nueva;
            }

            pic1.BringToFront();
            this.Controls.Add(pic1);
            Serp[n] = pic1;
        }

        public void GenerarComida()
        {
            int[] coorX = new int[10000];
            int[] coorY = new int[10000];
            bool[] libre = new bool[10000];
            bool[] libre1 = new bool[10000];
            int[] coorX1 = new int[10000];
            int[] coorY1 = new int[10000];
            int[] direcX = {0, -1, 0, 1};
            int[] direcY = {1, 0, -1, 0};
            string[] direcCons = new string[625 * 4];
            int contlibre = 0;
            int iteartor = 0;
            int iteartor1 = 0;
            for (int x = 0; x < mapa.fila; x++)
            {
                for (int y = 0; y < mapa.columna; y++)
                {
                    if (snake[x, y])
                    {
                        contlibre++;
                        int cant_obs = 0;
                        for (int j = 0; j < 4; j++)
                        {
                            int poscabezX = 0;
                            int poscabezY = 0;
                            if (Dentro_Matriz(x + direcX[j], y + direcY[j]))
                            {
                                poscabezX = x + direcX[j];
                                poscabezY = y + direcY[j];
                            }
                            else
                            {
                                poscabezX = x + direcX[j];
                                poscabezY = y + direcY[j];
                                if (x + direcX[j] < 0) poscabezX = mapa.fila - 1;
                                if (x + direcX[j] >= mapa.fila) poscabezX = 0;
                                if (y + direcY[j] < 0) poscabezY = mapa.columna - 1;
                                if (y + direcY[j] >= mapa.columna) poscabezY = 0;
                            }

                            if (!snake[poscabezX, poscabezY])
                            {
                                cant_obs++;
                            }
                        }

                        if (cant_obs <= 2)
                        {
                            coorX[iteartor] = x;
                            coorY[iteartor] = y;
                            libre[iteartor] = true;
                            iteartor++;
                        }
                        else
                        {
                            coorX1[iteartor1] = x;
                            coorY1[iteartor1] = y;
                            libre1[iteartor1] = true;
                            iteartor1++;
                        }

                        cant_obs = 0;
                    }
                }
            }

            int cant = 0;
            int cant1 = 0;
            if (contlibre >= huevos)
            {
                if (huevos > iteartor)
                {
                    cant = iteartor;
                    cant1 = huevos - iteartor;
                }
                else
                {
                    cant = huevos;
                    cant1 = 0;
                }
            }
            else
            {
                cant = iteartor;
                cant1 = iteartor1;
            }

            canthuevosAct = cant + cant1;

            int canthuevo = 0;
            int crono = 0;
            int c = 0;
            int d = 0;
            while (true)
            {
                if (c == cant) break;
                Random rnd = new Random(DateTime.Now.Millisecond + crono + 4);
                d = rnd.Next(iteartor);
                if (libre[d])
                {
                    libre[d] = false;
                    huevosX[canthuevo] = coorX[d];
                    huevosY[canthuevo] = coorY[d];

                    HuevoPic[canthuevo].Visible = true;
                    huevosVisible[canthuevo] = true;

                    canthuevo++;
                    c++;
                }

                crono++;
            }

            crono = 0;
            c = 0;
            d = 0;

            while (true)
            {
                if (c == cant1) break;
                Random rnd = new Random(DateTime.Now.Millisecond - crono * crono - 4);
                d = rnd.Next(iteartor1);
                if (libre1[d])
                {
                    libre1[d] = false;
                    huevosX[canthuevo] = coorX1[d];
                    huevosY[canthuevo] = coorY1[d];

                    HuevoPic[canthuevo].Visible = true;
                    huevosVisible[canthuevo] = true;

                    canthuevo++;
                    c++;
                }

                crono++;
            }
        }

        public void PintarHuevos()
        {
            for (int i = 0; i < huevos; i++)
            {
                PictureBox pic2 = new PictureBox();
                Bitmap vieja = (Bitmap) Snake.Properties.Resources.huevo;
                Bitmap nueva = new Bitmap(vieja, mapa.dimensionC(600) - 1, mapa.dimensionC(600) - 1);
                pic2.Image = nueva;
                pic2.Size = new Size(mapa.dimensionC(600) - 1, mapa.dimensionC(600) - 1);
                pic2.Paint += NumerarHuevos;
                this.Controls.Add(pic2);
                HuevoPic[i] = pic2;
                HuevoPic[i].Visible = false;
            }
        }

        private void NumerarHuevos(object sender, PaintEventArgs e)
        {
            var a = sender;
            string text = "";
            bool mayor10 = false;
            for (int i = 0; i < huevos; i++)
            {
                if (a == HuevoPic[i])
                {
                    text = (i + 1) + "";
                    if (i + 1 >= 10) mayor10 = true;
                }
            }

            using (Font myFont = new Font("Arial", (50 * mapa.dimensionC(600)) / 150))
            {
                if (mayor10)
                {
                    mayor10 = false;
                    e.Graphics.DrawString(text, myFont, Brushes.Black,
                        new PointF((25 * mapa.dimensionC(600)) / 150, (40 * mapa.dimensionC(600)) / 150));
                }
                else
                {
                    e.Graphics.DrawString(text, myFont, Brushes.Black,
                        new PointF((45 * mapa.dimensionC(600)) / 150, (40 * mapa.dimensionC(600)) / 150));
                }
            }
        }

        public string direcAct()
        {
            if (serpX[0] - serpX[1] == -1 && serpY[0] - serpY[1] == 0)
            {
                return "up";
            }

            if (serpX[0] - serpX[1] == mapa.fila - 1 && serpY[0] - serpY[1] == 0)
            {
                return "up";
            }

            if (serpX[0] - serpX[1] == 0 && serpY[0] - serpY[1] == 1)
            {
                return "right";
            }

            if (serpX[0] - serpX[1] == 0 && serpY[0] - serpY[1] == -mapa.columna + 1)
            {
                return "right";
            }

            if (serpX[0] - serpX[1] == 1 && serpY[0] - serpY[1] == 0)
            {
                return "down";
            }

            if (serpX[0] - serpX[1] == -mapa.fila + 1 && serpY[0] - serpY[1] == 0)
            {
                return "down";
            }

            if (serpX[0] - serpX[1] == 0 && serpY[0] - serpY[1] == -1)
            {
                return "left";
            }

            if (serpX[0] - serpX[1] == 0 && serpY[0] - serpY[1] == mapa.columna - 1)
            {
                return "left";
            }

            return "";
        }

        public void Moverse()
        {
            int movX = 0;
            int movY = 0;
            if (direccion == "up")
            {
                movX = -1;
                movY = 0;
            }

            if (direccion == "right")
            {
                movX = 0;
                movY = 1;
            }

            if (direccion == "down")
            {
                movX = 1;
                movY = 0;
            }

            if (direccion == "left")
            {
                movX = 0;
                movY = -1;
            }

            int poscabezX = 0;
            int poscabezY = 0;
            if (Dentro_Matriz(serpX[0] + movX, serpY[0] + movY))
            {
                poscabezX = serpX[0] + movX;
                poscabezY = serpY[0] + movY;
            }
            else
            {
                poscabezX = serpX[0] + movX;
                poscabezY = serpY[0] + movY;
                if (serpX[0] + movX < 0) poscabezX = mapa.fila - 1;
                if (serpX[0] + movX >= mapa.fila) poscabezX = 0;
                if (serpY[0] + movY < 0) poscabezY = mapa.columna - 1;
                if (serpY[0] + movY >= mapa.columna) poscabezY = 0;
            }

            if (snake[poscabezX, poscabezY])
            {
                for (int i = 0; i <= indice; i++)
                {
                    snake[serpX[i], serpY[i]] = true;
                }

                int cambX = 0;
                int cambY = 0;
                int cambio = 0;
                for (int i = 0; i <= indice; i++)
                {
                    if (i == 0)
                    {
                        cambX = serpX[i];
                        cambY = serpY[i];
                        serpX[i] = poscabezX;
                        serpY[i] = poscabezY;
                    }
                    else
                    {
                        cambio = cambX;
                        cambX = serpX[i];
                        serpX[i] = cambio;
                        cambio = cambY;
                        cambY = serpY[i];
                        serpY[i] = cambio;
                    }

                    snake[serpX[i], serpY[i]] = false;
                }

                if (crecer != 0)
                {
                    indice++;
                    serpX[indice] = cambX;
                    serpY[indice] = cambY;
                    PintarSerpiente(indice);
                    snake[cambX, cambY] = false;
                    crecer--;
                }

                Comer();

                PintarMover();
            }
            else
            {
                timer1.Stop();
                label1.Text = "Has Perdido " + label1.Text;
                label3.Visible = true;
                label2.Visible = true;
            }
        }

        public void Comer()
        {
            for (int i = 0; i < huevos; i++)
            {
                if (huevosX[i] == serpX[0] && huevosY[i] == serpY[0])
                {
                    if (HuevoPic[i].Visible == true)
                    {
                        huevosVisible[i] = false;
                        canthuevosAct--;

                        crecer = i + 1;
                        if (canthuevosAct == 0)
                        {
                            GenerarComida();
                        }
                        else Puntuacion(i);

                        break;
                    }
                }
            }
        }

        public int Random(int n)
        {
            Random rnd = new Random();
            return rnd.Next(n);
        }

        public bool Dentro_Matriz(int x, int y)
        {
            if (x < 0 || y < 0) return false;
            if (x >= mapa.fila || y >= mapa.columna) return false;
            return true;
        }

        public void Puntuacion(int i)
        {
            int[] direcX = {0, -1, 0, 1};
            int[] direcY = {1, 0, -1, 0};
            int[,] cerca = new int[mapa.fila, mapa.columna];
            int s = 0;
            for (int x1 = 0; x1 < mapa.fila; x1++)
            {
                for (int y1 = 0; y1 < mapa.columna; y1++)
                {
                    cerca[x1, y1] = 0;
                }
            }

            cerca[huevosX[i], huevosY[i]] = mapa.fila * mapa.columna;
            int x;
            int y;
            int[] cambX = new int[10000];
            int[] cambY = new int[10000];
            int[] cambX1 = new int[10000];
            int[] cambY1 = new int[10000];
            cambX[0] = huevosX[i];
            cambY[0] = huevosY[i];

            int indice1 = 1;
            int indice2 = 0;
            int indice3 = 0;


            while (s != mapa.fila * mapa.columna - muros1 - 1)
            {
                for (int g = 0; g <= indice2; g++)
                {
                    x = cambX[g];
                    y = cambY[g];

                    for (int j = 0; j < 4; j++)
                    {
                        int poscabezX = 0;
                        int poscabezY = 0;
                        if (Dentro_Matriz(x + direcX[j], y + direcY[j]))
                        {
                            poscabezX = x + direcX[j];
                            poscabezY = y + direcY[j];
                        }
                        else
                        {
                            poscabezX = x + direcX[j];
                            poscabezY = y + direcY[j];
                            if (x + direcX[j] < 0) poscabezX = mapa.fila - 1;
                            if (x + direcX[j] >= mapa.fila) poscabezX = 0;
                            if (y + direcY[j] < 0) poscabezY = mapa.columna - 1;
                            if (y + direcY[j] >= mapa.columna) poscabezY = 0;
                        }

                        if (muros[poscabezX, poscabezY])
                        {
                            if (cerca[poscabezX, poscabezY] == 0)
                            {
                                cerca[poscabezX, poscabezY] = indice1;
                                cambX1[indice3] = poscabezX;
                                cambY1[indice3] = poscabezY;
                                indice3++;
                                s++;
                            }
                        }
                    }
                }

                for (int g1 = 0; g1 < indice3; g1++)
                {
                    cambX[g1] = cambX1[g1];
                    cambY[g1] = cambY1[g1];
                }

                indice2 = indice3 - 1;
                indice3 = 0;
                indice1++;
            }

            int min = mapa.fila * mapa.columna - 1;
            int min1 = 0;
            for (int j = 0; j < huevos; j++)
            {
                if (j != i)
                {
                    if (HuevoPic[j].Visible == true)
                    {
                        if (cerca[huevosX[j], huevosY[j]] <= min)
                        {
                            min = cerca[huevosX[j], huevosY[j]];
                            min1 = j;
                        }
                    }
                }
            }

            puntuacion = puntuacion + (min1 + 1) * 100;
            label1.Text = "Puntuación: " + puntuacion + "";
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cerrarform1)
            {
                main.Close();
            }
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        public void PintarMover()
        {
            if (direccion == "up")
            {
                Bitmap vieja = (Bitmap) Snake.Properties.Resources.cabezaArriba;
                Bitmap nueva = new Bitmap(vieja, mapa.dimensionC(600) - 1, mapa.dimensionC(600) - 1);
                Serp[0].Image = nueva;
            }

            if (direccion == "right")
            {
                Bitmap vieja = (Bitmap) Snake.Properties.Resources.cabezaDerecha;
                Bitmap nueva = new Bitmap(vieja, mapa.dimensionC(600) - 1, mapa.dimensionC(600) - 1);
                Serp[0].Image = nueva;
            }

            if (direccion == "down")
            {
                Bitmap vieja = (Bitmap) Snake.Properties.Resources.cabezaAbajo;
                Bitmap nueva = new Bitmap(vieja, mapa.dimensionC(600) - 1, mapa.dimensionC(600) - 1);
                Serp[0].Image = nueva;
            }

            if (direccion == "left")
            {
                Bitmap vieja = (Bitmap) Snake.Properties.Resources.cabezaIzquierda;
                Bitmap nueva = new Bitmap(vieja, mapa.dimensionC(600) - 1, mapa.dimensionC(600) - 1);
                Serp[0].Image = nueva;
            }

            for (int i = 0; i <= indice; i++)
            {
                Serp[i].BringToFront();
                Serp[i].Location = new Point(100 + mapa.locationX(600) + serpY[i] * mapa.dimensionC(600) + 1,
                    80 + mapa.locationY(600) + serpX[i] * mapa.dimensionC(600) + 1);
            }

            for (int i = 0; i < huevos; i++)
            {
                if (huevosVisible[i])
                {
                    HuevoPic[i].BringToFront();
                    HuevoPic[i].Location = new Point(100 + mapa.locationX(600) + huevosY[i] * mapa.dimensionC(600) + 1,
                        80 + mapa.locationY(600) + huevosX[i] * mapa.dimensionC(600) + 1);
                }
                else
                {
                    HuevoPic[i].Visible = false;
                }
            }
        }
    }
}