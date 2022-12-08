using System;
using System.Drawing;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form2 : Form
    {
        bool _cerrarform1 = true;
        bool _startGame;
        int _puntuacion;
        int _crecer;
        string _direccion = "";
        int _indice;
        int[] _serpX = new int[10000];
        int[] _serpY = new int[10000];
        int[] _huevosX = new int[Compartir_Class.huevos];
        int[] _huevosY = new int[Compartir_Class.huevos];
        bool[] _huevosVisible = new bool[Compartir_Class.huevos];

        int _huevos;
        int _canthuevosAct;
        int _velocidad;
        Graphics _g;
        Mapa_Class _mapa = new Mapa_Class();
        bool[,] _snake = new bool[10000, 10000];
        bool[,] _muros = new bool[10000, 10000];
        int _muros1 = 0;
        // PictureBox Act = new PictureBox();
        PictureBox[] _serp = new PictureBox[10000];
        PictureBox[] _huevoPic = new PictureBox[Compartir_Class.huevos];
        Form1 _main;

        public Form2(Form1 newform)
        {
            _main = newform;
            InitializeComponent();
            pictureBox1.Visible = false;
            Juego();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
        }

        private void Label3_Click(object sender, EventArgs e)
        {
            Form2 hola = new Form2(_main);
            _cerrarform1 = false;
            hola.Show();
            this.Close();
        }


        private void Label2_Click(object sender, EventArgs e)
        {
            _main.Show();
            _cerrarform1 = false;
            this.Close();
        }

        private void Form2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                if (!_startGame)
                {
                    _startGame = true;
                    _direccion = "up";
                    timer1.Start();
                    _crecer++;
                }

                if (direcAct() != "down")
                {
                    _direccion = "up";
                }
            }

            if (e.KeyCode == Keys.Right)
            {
                if (!_startGame)
                {
                    _startGame = true;
                    _direccion = "right";
                    timer1.Start();
                    _crecer++;
                }

                if (direcAct() != "left")
                {
                    _direccion = "right";
                }
            }

            if (e.KeyCode == Keys.Down)
            {
                if (!_startGame)
                {
                    _startGame = true;
                    _direccion = "down";
                    timer1.Start();
                    _crecer++;
                }

                if (direcAct() != "up")
                {
                    _direccion = "down";
                }
            }

            if (e.KeyCode == Keys.Left)
            {
                if (!_startGame)
                {
                    _startGame = true;
                    _direccion = "left";
                    timer1.Start();
                    _crecer++;
                }

                if (direcAct() != "right")
                {
                    _direccion = "left";
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
            label1.Text = "Puntuación: " + _puntuacion + "";
            _mapa.fila = Compartir_Class.fila;
            _mapa.columna = Compartir_Class.columna;
            _velocidad = Compartir_Class.velocidad;
            timer1.Interval = 1000 / _velocidad;
            _huevos = Compartir_Class.huevos;
            for (int i = 0; i < _mapa.fila; i++)
            {
                for (int j = 0; j < _mapa.columna; j++)
                {
                    _snake[i, j] = Compartir_Class.snake[i, j];
                    _muros[i, j] = Compartir_Class.snake[i, j];
                    if (!_muros[i, j])
                    {
                        _muros1++;
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
            _startGame = true;

            Moverse();
        }

        public void Construir_Mapa()
        {
            PictureBox pic = new PictureBox();

            pic.Size = new Size(_mapa.dimensionX(600), _mapa.dimensionY(600));
            pic.Location = new Point(100 + _mapa.locationX(600), 80 + _mapa.locationY(600));
            pic.Paint += Pintar;
            this.Controls.Add(pic);
        }

        private void Pintar(object sender, PaintEventArgs e)
        {
            _g = e.Graphics;
            Pen p = new Pen(Color.Black, 1);
            for (int i = 0; i <= _mapa.columna; i++)
            {
                _g.DrawLine(p, i * _mapa.dimensionC(600), 0, i * _mapa.dimensionC(600), _mapa.fila * _mapa.dimensionC(600));
            }

            for (int i = 0; i <= _mapa.fila; i++)
            {
                _g.DrawLine(p, 0, i * _mapa.dimensionC(600), _mapa.dimensionC(600) * _mapa.columna,
                    i * _mapa.dimensionC(600));
            }

            for (int i = 0; i < _mapa.fila; i++)
            {
                for (int j = 0; j < _mapa.columna; j++)
                {
                    if (!_muros[i, j])
                    {
                        SolidBrush brush = new SolidBrush(Color.Brown);
                        _g.FillRectangle(brush, j * _mapa.dimensionC(600) + 1, i * _mapa.dimensionC(600) + 1,
                            _mapa.dimensionC(600) - 1, _mapa.dimensionC(600) - 1);
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
            for (int x = 0; x < _mapa.fila; x++)
            {
                for (int y = 0; y < _mapa.columna; y++)
                {
                    if (_snake[x, y])
                    {
                        bool e = true;
                        for (int j = 0; j < _huevos; j++)
                        {
                            if (_huevosX[j] == x && _huevosY[j] == y)
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
            _serpX[0] = coorX[i];
            _serpY[0] = coorY[i];
            _snake[coorX[i], coorY[i]] = false;
            PintarSerpiente(_indice);
            PintarMover();
        }

        public void PintarSerpiente(int n)
        {
            PictureBox pic1 = new PictureBox();
            pic1.Size = new Size(_mapa.dimensionC(600) - 1, _mapa.dimensionC(600) - 1);
            if (n == 0)
            {
                Bitmap vieja = (Bitmap) Snake.Properties.Resources.cabezaArriba;
                Bitmap nueva = new Bitmap(vieja, _mapa.dimensionC(600) - 1, _mapa.dimensionC(600) - 1);
                pic1.Image = nueva;
            }
            else
            {
                Bitmap vieja = (Bitmap) Snake.Properties.Resources.CuerpoSerp;
                Bitmap nueva = new Bitmap(vieja, _mapa.dimensionC(600) - 1, _mapa.dimensionC(600) - 1);
                pic1.Image = nueva;
            }

            pic1.BringToFront();
            this.Controls.Add(pic1);
            _serp[n] = pic1;
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
            for (int x = 0; x < _mapa.fila; x++)
            {
                for (int y = 0; y < _mapa.columna; y++)
                {
                    if (_snake[x, y])
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
                                if (x + direcX[j] < 0) poscabezX = _mapa.fila - 1;
                                if (x + direcX[j] >= _mapa.fila) poscabezX = 0;
                                if (y + direcY[j] < 0) poscabezY = _mapa.columna - 1;
                                if (y + direcY[j] >= _mapa.columna) poscabezY = 0;
                            }

                            if (!_snake[poscabezX, poscabezY])
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
            if (contlibre >= _huevos)
            {
                if (_huevos > iteartor)
                {
                    cant = iteartor;
                    cant1 = _huevos - iteartor;
                }
                else
                {
                    cant = _huevos;
                    cant1 = 0;
                }
            }
            else
            {
                cant = iteartor;
                cant1 = iteartor1;
            }

            _canthuevosAct = cant + cant1;

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
                    _huevosX[canthuevo] = coorX[d];
                    _huevosY[canthuevo] = coorY[d];

                    _huevoPic[canthuevo].Visible = true;
                    _huevosVisible[canthuevo] = true;

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
                    _huevosX[canthuevo] = coorX1[d];
                    _huevosY[canthuevo] = coorY1[d];

                    _huevoPic[canthuevo].Visible = true;
                    _huevosVisible[canthuevo] = true;

                    canthuevo++;
                    c++;
                }

                crono++;
            }
        }

        public void PintarHuevos()
        {
            for (int i = 0; i < _huevos; i++)
            {
                PictureBox pic2 = new PictureBox();
                Bitmap vieja = (Bitmap) Snake.Properties.Resources.huevo;
                Bitmap nueva = new Bitmap(vieja, _mapa.dimensionC(600) - 1, _mapa.dimensionC(600) - 1);
                pic2.Image = nueva;
                pic2.Size = new Size(_mapa.dimensionC(600) - 1, _mapa.dimensionC(600) - 1);
                pic2.Paint += NumerarHuevos;
                this.Controls.Add(pic2);
                _huevoPic[i] = pic2;
                _huevoPic[i].Visible = false;
            }
        }

        private void NumerarHuevos(object sender, PaintEventArgs e)
        {
            var a = sender;
            string text = "";
            bool mayor10 = false;
            for (int i = 0; i < _huevos; i++)
            {
                if (a == _huevoPic[i])
                {
                    text = (i + 1) + "";
                    if (i + 1 >= 10) mayor10 = true;
                }
            }

            using (Font myFont = new Font("Arial", (50 * _mapa.dimensionC(600)) / 150))
            {
                if (mayor10)
                {
                    mayor10 = false;
                    e.Graphics.DrawString(text, myFont, Brushes.Black,
                        new PointF((25 * _mapa.dimensionC(600)) / 150, (40 * _mapa.dimensionC(600)) / 150));
                }
                else
                {
                    e.Graphics.DrawString(text, myFont, Brushes.Black,
                        new PointF((45 * _mapa.dimensionC(600)) / 150, (40 * _mapa.dimensionC(600)) / 150));
                }
            }
        }

        public string direcAct()
        {
            if (_serpX[0] - _serpX[1] == -1 && _serpY[0] - _serpY[1] == 0)
            {
                return "up";
            }

            if (_serpX[0] - _serpX[1] == _mapa.fila - 1 && _serpY[0] - _serpY[1] == 0)
            {
                return "up";
            }

            if (_serpX[0] - _serpX[1] == 0 && _serpY[0] - _serpY[1] == 1)
            {
                return "right";
            }

            if (_serpX[0] - _serpX[1] == 0 && _serpY[0] - _serpY[1] == -_mapa.columna + 1)
            {
                return "right";
            }

            if (_serpX[0] - _serpX[1] == 1 && _serpY[0] - _serpY[1] == 0)
            {
                return "down";
            }

            if (_serpX[0] - _serpX[1] == -_mapa.fila + 1 && _serpY[0] - _serpY[1] == 0)
            {
                return "down";
            }

            if (_serpX[0] - _serpX[1] == 0 && _serpY[0] - _serpY[1] == -1)
            {
                return "left";
            }

            if (_serpX[0] - _serpX[1] == 0 && _serpY[0] - _serpY[1] == _mapa.columna - 1)
            {
                return "left";
            }

            return "";
        }

        public void Moverse()
        {
            int movX = 0;
            int movY = 0;
            if (_direccion == "up")
            {
                movX = -1;
                movY = 0;
            }

            if (_direccion == "right")
            {
                movX = 0;
                movY = 1;
            }

            if (_direccion == "down")
            {
                movX = 1;
                movY = 0;
            }

            if (_direccion == "left")
            {
                movX = 0;
                movY = -1;
            }

            int poscabezX = 0;
            int poscabezY = 0;
            if (Dentro_Matriz(_serpX[0] + movX, _serpY[0] + movY))
            {
                poscabezX = _serpX[0] + movX;
                poscabezY = _serpY[0] + movY;
            }
            else
            {
                poscabezX = _serpX[0] + movX;
                poscabezY = _serpY[0] + movY;
                if (_serpX[0] + movX < 0) poscabezX = _mapa.fila - 1;
                if (_serpX[0] + movX >= _mapa.fila) poscabezX = 0;
                if (_serpY[0] + movY < 0) poscabezY = _mapa.columna - 1;
                if (_serpY[0] + movY >= _mapa.columna) poscabezY = 0;
            }

            if (_snake[poscabezX, poscabezY])
            {
                for (int i = 0; i <= _indice; i++)
                {
                    _snake[_serpX[i], _serpY[i]] = true;
                }

                int cambX = 0;
                int cambY = 0;
                int cambio = 0;
                for (int i = 0; i <= _indice; i++)
                {
                    if (i == 0)
                    {
                        cambX = _serpX[i];
                        cambY = _serpY[i];
                        _serpX[i] = poscabezX;
                        _serpY[i] = poscabezY;
                    }
                    else
                    {
                        cambio = cambX;
                        cambX = _serpX[i];
                        _serpX[i] = cambio;
                        cambio = cambY;
                        cambY = _serpY[i];
                        _serpY[i] = cambio;
                    }

                    _snake[_serpX[i], _serpY[i]] = false;
                }

                if (_crecer != 0)
                {
                    _indice++;
                    _serpX[_indice] = cambX;
                    _serpY[_indice] = cambY;
                    PintarSerpiente(_indice);
                    _snake[cambX, cambY] = false;
                    _crecer--;
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
            for (int i = 0; i < _huevos; i++)
            {
                if (_huevosX[i] == _serpX[0] && _huevosY[i] == _serpY[0])
                {
                    if (_huevoPic[i].Visible == true)
                    {
                        _huevosVisible[i] = false;
                        _canthuevosAct--;

                        _crecer = i + 1;
                        if (_canthuevosAct == 0)
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
            if (x >= _mapa.fila || y >= _mapa.columna) return false;
            return true;
        }

        public void Puntuacion(int i)
        {
            int[] direcX = {0, -1, 0, 1};
            int[] direcY = {1, 0, -1, 0};
            int[,] cerca = new int[_mapa.fila, _mapa.columna];
            int s = 0;
            for (int x1 = 0; x1 < _mapa.fila; x1++)
            {
                for (int y1 = 0; y1 < _mapa.columna; y1++)
                {
                    cerca[x1, y1] = 0;
                }
            }

            cerca[_huevosX[i], _huevosY[i]] = _mapa.fila * _mapa.columna;
            int x;
            int y;
            int[] cambX = new int[10000];
            int[] cambY = new int[10000];
            int[] cambX1 = new int[10000];
            int[] cambY1 = new int[10000];
            cambX[0] = _huevosX[i];
            cambY[0] = _huevosY[i];

            int indice1 = 1;
            int indice2 = 0;
            int indice3 = 0;


            while (s != _mapa.fila * _mapa.columna - _muros1 - 1)
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
                            if (x + direcX[j] < 0) poscabezX = _mapa.fila - 1;
                            if (x + direcX[j] >= _mapa.fila) poscabezX = 0;
                            if (y + direcY[j] < 0) poscabezY = _mapa.columna - 1;
                            if (y + direcY[j] >= _mapa.columna) poscabezY = 0;
                        }

                        if (_muros[poscabezX, poscabezY])
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

            int min = _mapa.fila * _mapa.columna - 1;
            int min1 = 0;
            for (int j = 0; j < _huevos; j++)
            {
                if (j != i)
                {
                    if (_huevoPic[j].Visible == true)
                    {
                        if (cerca[_huevosX[j], _huevosY[j]] <= min)
                        {
                            min = cerca[_huevosX[j], _huevosY[j]];
                            min1 = j;
                        }
                    }
                }
            }

            _puntuacion = _puntuacion + (min1 + 1) * 100;
            label1.Text = "Puntuación: " + _puntuacion + "";
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_cerrarform1)
            {
                _main.Close();
            }
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        public void PintarMover()
        {
            if (_direccion == "up")
            {
                Bitmap vieja = (Bitmap) Snake.Properties.Resources.cabezaArriba;
                Bitmap nueva = new Bitmap(vieja, _mapa.dimensionC(600) - 1, _mapa.dimensionC(600) - 1);
                _serp[0].Image = nueva;
            }

            if (_direccion == "right")
            {
                Bitmap vieja = (Bitmap) Snake.Properties.Resources.cabezaDerecha;
                Bitmap nueva = new Bitmap(vieja, _mapa.dimensionC(600) - 1, _mapa.dimensionC(600) - 1);
                _serp[0].Image = nueva;
            }

            if (_direccion == "down")
            {
                Bitmap vieja = (Bitmap) Snake.Properties.Resources.cabezaAbajo;
                Bitmap nueva = new Bitmap(vieja, _mapa.dimensionC(600) - 1, _mapa.dimensionC(600) - 1);
                _serp[0].Image = nueva;
            }

            if (_direccion == "left")
            {
                Bitmap vieja = (Bitmap) Snake.Properties.Resources.cabezaIzquierda;
                Bitmap nueva = new Bitmap(vieja, _mapa.dimensionC(600) - 1, _mapa.dimensionC(600) - 1);
                _serp[0].Image = nueva;
            }

            for (int i = 0; i <= _indice; i++)
            {
                _serp[i].BringToFront();
                _serp[i].Location = new Point(100 + _mapa.locationX(600) + _serpY[i] * _mapa.dimensionC(600) + 1,
                    80 + _mapa.locationY(600) + _serpX[i] * _mapa.dimensionC(600) + 1);
            }

            for (int i = 0; i < _huevos; i++)
            {
                if (_huevosVisible[i])
                {
                    _huevoPic[i].BringToFront();
                    _huevoPic[i].Location = new Point(100 + _mapa.locationX(600) + _huevosY[i] * _mapa.dimensionC(600) + 1,
                        80 + _mapa.locationY(600) + _huevosX[i] * _mapa.dimensionC(600) + 1);
                }
                else
                {
                    _huevoPic[i].Visible = false;
                }
            }
        }
    }
}