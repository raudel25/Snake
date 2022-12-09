﻿using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        private PictureBox _act = new PictureBox();

        private bool _generarObs;
        private string _jugador = "";

        private readonly Mapa_Class _mapa = new Mapa_Class();
        private int _mouseX;
        private int _mouseY;
        private int _muros;
        private bool _nombrevalido = true;
        private bool _pintarObs;
        private readonly bool[,] _snake = new bool[100, 100];
        private Graphics g;

        public Form1()
        {
            InitializeComponent();
            Inicio();
        }

        public void Inicio_Visible(bool visible)
        {
            label5.Visible = visible;
            label6.Visible = false;
            label7.Visible = visible;
            textBox1.Visible = visible;
            pictureBox1.Visible = visible;
        }

        public void Inicio()
        {
            pictureBox1.Size = new Size(400, 400);
            pictureBox1.Location = new Point(200, 45);
            label5.BringToFront();
            var vieja = (Bitmap)pictureBox1.Image;
            var nueva = new Bitmap(vieja, 400, 400);
            pictureBox1.Image = nueva;
            Inicio_Visible(true);
            Generar_Mapa_Visible(false);
            TextsLabel();

            label5.Location = new Point(350, 400);
            textBox1.Location = new Point(325, 450);
            textBox1.Size = new Size(150, 30);
            label6.Location = new Point(240, 500);
            label7.Location = new Point(240, 500);
            label7.Size = new Size(330, 30);
        }

        private void TextsLabel()
        {
            label5.Text = "Jugador";
            label6.Text = "Mapa Random";
            label7.Text = "Generar Mapa";
        }

        public void Generar_Mapa_Visible(bool visible)
        {
            label1.Visible = visible;
            label2.Visible = visible;
            label3.Visible = visible;
            label4.Visible = visible;
            label9.Visible = visible;
            label10.Visible = visible;
            label11.Visible = visible;
            label12.Visible = visible;
            numericUpDown1.Visible = visible;
            numericUpDown2.Visible = visible;
            numericUpDown3.Visible = visible;
            numericUpDown4.Visible = visible;
            label8.Visible = visible;
        }

        public void GenerarMapa()
        {
            Inicio_Visible(false);
            Generar_Mapa_Visible(true);
            Construir_Mapa();
            ActSnake();
        }

        public void Juego()
        {
            Compartir_Class.snake = _snake;
            Compartir_Class.fila = _mapa.fila;
            Compartir_Class.columna = _mapa.columna;
            Compartir_Class.velocidad = Convert.ToInt32(numericUpDown4.Value);
            Compartir_Class.huevos = Convert.ToInt32(numericUpDown3.Value);
            var newform = new Form2(this);
            newform.Show();
            Hide();
        }

        private void Label7_Click(object sender, EventArgs e)
        {
            if (_nombrevalido)
            {
                GenerarMapa();
            }
            else
            {
                Inicio_Visible(false);
                _act.Visible = true;
                Generar_Mapa_Visible(true);
            }
        }


        private void Label9_Click(object sender, EventArgs e)
        {
            if (numericUpDown4.Value <= 0)
            {
                MessageBox.Show("La velocidad no es correcta");
                numericUpDown4.Value = 1;
            }

            if (numericUpDown3.Value <= 0)
            {
                MessageBox.Show("La cantidad de huevos no es correcta");
                numericUpDown3.Value = 1;
            }

            if (Convert.ToInt32(numericUpDown1.Value) < 4 || Convert.ToInt32(numericUpDown2.Value) < 4)
            {
                MessageBox.Show("La cantidad de filas o columnas no es correcta");
                numericUpDown1.Value = 4;
                numericUpDown2.Value = 4;
            }

            if (_generarObs)
            {
                MessageBox.Show("Debe terminar de generar los obstáculos");
            }
            else
            {
                if (MapaValido(_snake, _mapa.fila, _mapa.columna, Convert.ToInt32(numericUpDown3.Value)))
                    Juego();
                else MessageBox.Show("El mapa no es válido");
            }
        }

        private void Label10_Click(object sender, EventArgs e)
        {
            NuevoMapa();
            if (numericUpDown4.Value <= 0)
            {
                MessageBox.Show("La velocidad no es correcta");
                numericUpDown4.Value = 1;
            }

            if (numericUpDown3.Value <= 0)
            {
                MessageBox.Show("La cantidad de huevos no es correcta");
                numericUpDown3.Value = 1;
            }

            if (Convert.ToInt32(numericUpDown1.Value) < 4 || Convert.ToInt32(numericUpDown2.Value) < 4)
            {
                MessageBox.Show("La cantidad de filas o columnas no es correcta");
                numericUpDown1.Value = 4;
                numericUpDown2.Value = 4;
            }

            if (_generarObs)
            {
                MessageBox.Show("Debe terminar de generar los obstáculos");
            }
            else
            {
                openFileDialog1.Filter = "Archivo de Programa|*txt;";
                var directorio = "";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    directorio = openFileDialog1.FileName;
                else return;

                var inf = File.ReadAllLines(directorio);
                if (Mapacargado(inf))
                {
                    numericUpDown1.Value = int.Parse(inf[0]);
                    _mapa.fila = int.Parse(inf[0]);
                    numericUpDown2.Value = int.Parse(inf[1]);
                    _mapa.columna = int.Parse(inf[1]);
                    numericUpDown3.Value = int.Parse(inf[2]);
                    numericUpDown4.Value = int.Parse(inf[3]);
                    ActSnake();
                    _muros = int.Parse(inf[4]);

                    for (var i = 1; i <= _muros; i++)
                    {
                        var separar = inf[i + 4].Split(' ');
                        _snake[int.Parse(separar[0]), int.Parse(separar[1])] = false;
                    }

                    _act.Visible = false;
                    Construir_Mapa();
                }
                else
                {
                    MessageBox.Show("El mapa cargado no es correcto");
                }
            }
        }

        private void Label11_Click(object sender, EventArgs e)
        {
            if (numericUpDown4.Value <= 0)
            {
                MessageBox.Show("La velocidad no es correcta");
                numericUpDown4.Value = 1;
            }

            if (numericUpDown3.Value <= 0)
            {
                MessageBox.Show("La cantidad de huevos no es correcta");
                numericUpDown3.Value = 1;
            }

            if (Convert.ToInt32(numericUpDown1.Value) < 4 || Convert.ToInt32(numericUpDown2.Value) < 4)
            {
                MessageBox.Show("La cantidad de filas o columnas no es correcta");
                numericUpDown1.Value = 4;
                numericUpDown2.Value = 4;
            }

            if (_generarObs)
            {
                MessageBox.Show("Debe terminar de generar los obstáculos");
            }
            else
            {
                _jugador = textBox1.Text;
                if (_jugador == "")
                {
                    _nombrevalido = false;
                    MessageBox.Show("Para guardar el mapa debe introducir su nombre");
                    Generar_Mapa_Visible(false);
                    Inicio_Visible(true);
                    label6.Visible = false;
                    _act.Visible = false;
                }
                else
                {
                    if (MapaValido(_snake, _mapa.fila, _mapa.columna, Convert.ToInt32(numericUpDown3.Value)))
                    {
                        var directorio = "";
                        saveFileDialog1.Filter = "Archivos de Programa (.txt)|*txt;*jpge";
                        saveFileDialog1.FileName = textBox1.Text;
                        if (saveFileDialog1.ShowDialog() == DialogResult.OK) directorio = saveFileDialog1.FileName;

                        var inf = new string[5 + _muros];
                        inf[0] = numericUpDown1.Value + "";
                        inf[1] = numericUpDown2.Value + "";
                        inf[2] = numericUpDown3.Value + "";
                        inf[3] = numericUpDown4.Value + "";
                        inf[4] = _muros + "";
                        var j = 5;
                        for (var x = 0; x < _mapa.fila; x++)
                        for (var y = 0; y < _mapa.columna; y++)
                            if (!_snake[x, y])
                            {
                                inf[j] = x + " " + y + "";
                                j++;
                            }

                        File.WriteAllLines(directorio + ".txt", inf);
                    }
                    else
                    {
                        MessageBox.Show("El mapa no es válido");
                    }
                }
            }
        }

        private void Label12_Click(object sender, EventArgs e)
        {
            NuevoMapa();
        }

        private void NuevoMapa()
        {
            if (_generarObs)
            {
                MessageBox.Show("Debe terminar de generar los obstáculos");
            }
            else
            {
                numericUpDown1.Value = 4;
                numericUpDown2.Value = 4;
                _act.Visible = false;
                ActSnake();
                Construir_Mapa();
            }
        }

        public void Construir_Mapa()
        {
            var pic = new PictureBox();
            pic.Location = new Point(150, 70);
            pic.Paint += Pintar;
            pic.MouseDown += ObtenerCoordenadas;
            _act = pic;
            ActMapa();
            Controls.Add(pic);
        }

        private void Pintar(object sender, PaintEventArgs e)
        {
            g = e.Graphics;
            var p = new Pen(Color.Black, 1);
            for (var i = 0; i <= _mapa.columna; i++)
                g.DrawLine(p, i * _mapa.dimensionC(500), 0, i * _mapa.dimensionC(500),
                    _mapa.fila * _mapa.dimensionC(500));

            for (var i = 0; i <= _mapa.fila; i++)
                g.DrawLine(p, 0, i * _mapa.dimensionC(500), _mapa.dimensionC(500) * _mapa.columna,
                    i * _mapa.dimensionC(500));

            for (var i = 0; i < _mapa.fila; i++)
            for (var j = 0; j < _mapa.columna; j++)
                if (!_snake[i, j])
                {
                    var brush = new SolidBrush(Color.Brown);
                    g.FillRectangle(brush, j * _mapa.dimensionC(500) + 1, i * _mapa.dimensionC(500) + 1,
                        _mapa.dimensionC(500) - 1, _mapa.dimensionC(500) - 1);
                }

            if (_pintarObs)
            {
                var brush = new SolidBrush(Color.Brown);
                var brush1 = new SolidBrush(Color.Lime);

                var x = _mouseX / _mapa.dimensionC(500);
                var y = _mouseY / _mapa.dimensionC(500);

                _pintarObs = false;

                if (_snake[y, x])
                {
                    g.FillRectangle(brush, x * _mapa.dimensionC(500) + 1, y * _mapa.dimensionC(500) + 1,
                        _mapa.dimensionC(500) - 1, _mapa.dimensionC(500) - 1);
                    _snake[y, x] = false;
                }
                else
                {
                    g.FillRectangle(brush1, x * _mapa.dimensionC(500) + 1, y * _mapa.dimensionC(500) + 1,
                        _mapa.dimensionC(500) - 1, _mapa.dimensionC(500) - 1);
                    _snake[y, x] = true;
                }
            }
        }

        private void ObtenerCoordenadas(object sender, MouseEventArgs e)
        {
            if (_generarObs)
            {
                if (numericUpDown1.Value == _mapa.fila && numericUpDown2.Value == _mapa.columna)
                {
                    _mouseX = e.X;
                    _mouseY = e.Y;
                    _act.Visible = false;
                    _muros++;
                    Construir_Mapa();

                    _pintarObs = true;
                }
                else
                {
                    MessageBox.Show("Debe terminar de generar los obstáculos");
                    numericUpDown1.Value = _mapa.fila;
                    numericUpDown2.Value = _mapa.columna;
                }
            }
        }

        public void ActMapa()
        {
            _mapa.fila = Convert.ToInt32(numericUpDown1.Value);
            _mapa.columna = Convert.ToInt32(numericUpDown2.Value);
            _act.Size = new Size(_mapa.dimensionX(500), _mapa.dimensionY(500));
            _act.Location = new Point(150 + _mapa.locationX(500), 70 + _mapa.locationY(500));
        }

        public void ActSnake()
        {
            _muros = 0;
            for (var i = 0; i < _mapa.fila; i++)
            for (var j = 0; j < _mapa.columna; j++)
                _snake[i, j] = true;
        }

        private void NumericUpDown1_2_ValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(numericUpDown1.Value) < 4 || Convert.ToInt32(numericUpDown2.Value) < 4)
            {
                MessageBox.Show("La cantidad de filas o columnas no es correcta");
                numericUpDown1.Value = 4;
                numericUpDown2.Value = 4;
            }

            _act.Visible = false;
            ActMapa();
            ActSnake();
            Construir_Mapa();
        }

        private void Label8_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(numericUpDown1.Value) < 4 || Convert.ToInt32(numericUpDown2.Value) < 4)
            {
                MessageBox.Show("La cantidad de filas o columnas no es correcta");
                numericUpDown1.Value = 4;
                numericUpDown2.Value = 4;
            }

            _generarObs = !_generarObs;
            if (label8.Text == "Generar Obstáculos")
                label8.Text = "Terminar";
            else label8.Text = "Generar Obstáculos";
        }

        private void NumericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown4.Value <= 0)
            {
                MessageBox.Show("La velocidad no es correcta");
                numericUpDown4.Value = 1;
            }
        }

        private void NumericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown3.Value <= 0)
            {
                MessageBox.Show("La cantidad de comida no es correcta");
                numericUpDown3.Value = 1;
            }
        }

        public bool MapaValido(bool[,] snake2, int mapafila, int mapacolumna, int huevos)
        {
            int[] direcX = { 0, 1, 0, -1 };
            int[] direcY = { -1, 0, 1, 0 };
            var cont1 = 0;
            var contlibre = 0;
            var numero = new int[mapafila, mapacolumna];
            for (var x = 0; x < mapafila; x++)
            for (var y = 0; y < mapacolumna; y++)
                numero[x, y] = 0;

            for (var x = 0; x < mapafila; x++)
            for (var y = 0; y < mapacolumna; y++)
                if (snake2[x, y])
                {
                    contlibre++;
                    if (cont1 == 0)
                    {
                        numero[x, y] = 1;
                        cont1++;
                    }
                    else
                    {
                        for (var j = 0; j < 4; j++)
                        {
                            int poscabezX;
                            int poscabezY;
                            if (Dentro(x + direcX[j], y + direcY[j], mapafila, mapacolumna))
                            {
                                poscabezX = x + direcX[j];
                                poscabezY = y + direcY[j];
                            }
                            else
                            {
                                poscabezX = x + direcX[j];
                                poscabezY = y + direcY[j];
                                if (x + direcX[j] < 0) poscabezX = mapafila - 1;
                                if (x + direcX[j] >= mapafila) poscabezX = 0;
                                if (y + direcY[j] < 0) poscabezY = mapacolumna - 1;
                                if (y + direcY[j] >= mapacolumna) poscabezY = 0;
                            }

                            if (numero[poscabezX, poscabezY] == 1)
                            {
                                numero[x, y] = 1;

                                break;
                            }
                        }
                    }
                }

            for (var x = mapafila - 1; x >= 0; x--)
            for (var y = mapacolumna - 1; y >= 0; y--)
                if (snake2[x, y])
                    for (var j = 0; j < 4; j++)
                    {
                        int poscabezX;
                        int poscabezY;
                        if (Dentro(x + direcX[j], y + direcY[j], mapafila, mapacolumna))
                        {
                            poscabezX = x + direcX[j];
                            poscabezY = y + direcY[j];
                        }
                        else
                        {
                            poscabezX = x + direcX[j];
                            poscabezY = y + direcY[j];
                            if (x + direcX[j] < 0) poscabezX = mapafila - 1;
                            if (x + direcX[j] >= mapafila) poscabezX = 0;
                            if (y + direcY[j] < 0) poscabezY = mapacolumna - 1;
                            if (y + direcY[j] >= mapacolumna) poscabezY = 0;
                        }

                        if (numero[poscabezX, poscabezY] == 1)
                        {
                            numero[x, y] = 1;

                            break;
                        }
                    }

            for (var y = 0; y < _mapa.columna; y++)
            for (var x = 0; x < _mapa.fila; x++)
                if (snake2[x, y])
                    for (var j = 0; j < 4; j++)
                    {
                        int poscabezX;
                        int poscabezY;
                        if (Dentro(x + direcX[j], y + direcY[j], mapafila, mapacolumna))
                        {
                            poscabezX = x + direcX[j];
                            poscabezY = y + direcY[j];
                        }
                        else
                        {
                            poscabezX = x + direcX[j];
                            poscabezY = y + direcY[j];
                            if (x + direcX[j] < 0) poscabezX = mapafila - 1;
                            if (x + direcX[j] >= mapafila) poscabezX = 0;
                            if (y + direcY[j] < 0) poscabezY = mapacolumna - 1;
                            if (y + direcY[j] >= mapacolumna) poscabezY = 0;
                        }

                        if (numero[poscabezX, poscabezY] == 1)
                        {
                            numero[x, y] = 1;

                            break;
                        }
                    }

            for (var y = mapacolumna - 1; y >= 0; y--)
            for (var x = mapafila - 1; x >= 0; x--)
                if (snake2[x, y])
                    for (var j = 0; j < 4; j++)
                    {
                        int poscabezX;
                        int poscabezY;
                        if (Dentro(x + direcX[j], y + direcY[j], mapafila, mapacolumna))
                        {
                            poscabezX = x + direcX[j];
                            poscabezY = y + direcY[j];
                        }
                        else
                        {
                            poscabezX = x + direcX[j];
                            poscabezY = y + direcY[j];
                            if (x + direcX[j] < 0) poscabezX = mapafila - 1;
                            if (x + direcX[j] >= mapafila) poscabezX = 0;
                            if (y + direcY[j] < 0) poscabezY = mapacolumna - 1;
                            if (y + direcY[j] >= mapacolumna) poscabezY = 0;
                        }

                        if (numero[poscabezX, poscabezY] == 1)
                        {
                            numero[x, y] = 1;

                            break;
                        }
                    }

            cont1 = 0;
            for (var x = 0; x < mapafila; x++)
            for (var y = 0; y < mapacolumna; y++)
                if (numero[x, y] == 1)
                    cont1++;

            if (cont1 - huevos < 1) return false;
            if (cont1 == contlibre) return true;
            return false;
        }

        public bool Dentro(int x, int y, int mapafila, int mapacolumna)
        {
            if (x < 0 || y < 0) return false;
            if (x >= mapafila || y >= mapacolumna) return false;
            return true;
        }

        public bool Mapacargado(string[] inf)
        {
            if (!int.TryParse(inf[0], out _)) return false;
            if (!int.TryParse(inf[1], out _)) return false;
            if (!int.TryParse(inf[2], out _)) return false;
            if (!int.TryParse(inf[3], out _)) return false;
            if (!int.TryParse(inf[4], out _)) return false;
            if (int.Parse(inf[0]) < 4 || int.Parse(inf[0]) > 100) return false;
            if (int.Parse(inf[1]) < 4 || int.Parse(inf[1]) > 100) return false;
            if (int.Parse(inf[2]) < 0 || int.Parse(inf[2]) > 100) return false;
            if (int.Parse(inf[3]) < 0 || int.Parse(inf[3]) > 100) return false;
            if (int.Parse(inf[4]) < 0) return false;
            if (int.Parse(inf[1]) * int.Parse(inf[0]) - int.Parse(inf[2]) - int.Parse(inf[4]) < 0) return false;
            
            var snake3 = new bool[int.Parse(inf[0]), int.Parse(inf[1])];
            for (var x = 0; x < int.Parse(inf[0]); x++)
            for (var y = 0; y < int.Parse(inf[1]); y++)
                snake3[x, y] = true;

            var muros = int.Parse(inf[4]);
            for (var j = 1; j <= muros; j++)
            {
               var  separar = inf[j + 4].Split(' ');
                if (!int.TryParse(separar[0], out _)) return false;
                if (!int.TryParse(separar[1], out _)) return false;
                if (!Dentro(int.Parse(separar[0]), int.Parse(separar[1]), int.Parse(inf[0]), int.Parse(inf[1])))
                    return false;
                snake3[int.Parse(separar[0]), int.Parse(separar[1])] = false;
            }


            if (!MapaValido(snake3, int.Parse(inf[0]), int.Parse(inf[1]), int.Parse(inf[2]))) return false;
            return true;
        }
    }
}