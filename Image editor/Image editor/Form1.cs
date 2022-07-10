using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Image_editor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = new Bitmap(100,100);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Load_Own_Image();
        }
        private async void button2_Click(object sender, EventArgs e)
        {
            
            switch (comboBox1.SelectedIndex)
            {
                case 0: await Task.Run(() => { pictureBox1.Image = PutMaskOnImage(new Bitmap(pictureBox1.Image), new int[,] { { -1, 2, -1 }, { 2, 5, 2 }, { -1, 2, -1 } }, 4); });
                    break;
                case 1: await Task.Run(() => { pictureBox1.Image = PutMaskOnImage(new Bitmap(pictureBox1.Image), new int[,] { { -1, -1, -1 }, { -1, 9, -1 }, { -1, -1, -1 } }); });
                    break;
                case 2: await Task.Run(() => { pictureBox1.Image = SobelEdgeDetect(new Bitmap(pictureBox1.Image)); });
                    break;
                case 3: await Task.Run(() => { pictureBox1.Image = PutMaskOnImage(new Bitmap(pictureBox1.Image), new int[,] { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } }); });
                    break;
                default: return;
            }
            
        }

        private void Load_Own_Image()
        {
            //openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
            }
        }

        /// <summary>
        /// С помощью маски Собеля ищет края объектов на рисунке
        /// </summary>
        /// <param name="original">рисунок</param>
        /// <returns>рисунок с наложенной маской Собеля</returns>
        private Bitmap SobelEdgeDetect(Bitmap original)
        {
            Bitmap bitmap = original;
            int width = original.Width;
            int height = original.Height;
            int[,] gx = new int[,] { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
            int[,] gy = new int[,] { { 1, 2, 1 }, { 0, 0, 0 }, { -1, -2, -1 } };

            int[,] allPixR = new int[width, height];
            int[,] allPixG = new int[width, height];
            int[,] allPixB = new int[width, height];

            int limit = 128 * 128;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    allPixR[i, j] = original.GetPixel(i, j).R;
                    allPixG[i, j] = original.GetPixel(i, j).G;
                    allPixB[i, j] = original.GetPixel(i, j).B;
                }
            }

            int new_rx = 0, new_ry = 0;
            int new_gx = 0, new_gy = 0;
            int new_bx = 0, new_by = 0;
            int rc, gc, bc;
            for (int i = 1; i < original.Width - 1; i++)
            {
                for (int j = 1; j < original.Height - 1; j++)
                {

                    new_rx = 0;
                    new_ry = 0;
                    new_gx = 0;
                    new_gy = 0;
                    new_bx = 0;
                    new_by = 0;
                    rc = 0;
                    gc = 0;
                    bc = 0;

                    for (int wi = -1; wi < 2; wi++)
                    {
                        for (int hw = -1; hw < 2; hw++)
                        {
                            rc = allPixR[i + hw, j + wi];
                            new_rx += gx[wi + 1, hw + 1] * rc;
                            new_ry += gy[wi + 1, hw + 1] * rc;

                            gc = allPixG[i + hw, j + wi];
                            new_gx += gx[wi + 1, hw + 1] * gc;
                            new_gy += gy[wi + 1, hw + 1] * gc;

                            bc = allPixB[i + hw, j + wi];
                            new_bx += gx[wi + 1, hw + 1] * bc;
                            new_by += gy[wi + 1, hw + 1] * bc;
                        }
                    }
                    if (new_rx * new_rx + new_ry * new_ry > limit || new_gx * new_gx + new_gy * new_gy > limit || new_bx * new_bx + new_by * new_by > limit)
                        bitmap.SetPixel(i, j, Color.Black);

                    //bb.SetPixel (i, j, Color.FromArgb(allPixR[i,j],allPixG[i,j],allPixB[i,j]));
                    else
                        bitmap.SetPixel(i, j, Color.Transparent);
                }
            }
            return bitmap;

        }

        /// <summary>
        ///накладывает маску 3х3 на изображение
        /// </summary>
        /// <param name="original"> изображение</param>
        /// <param name="mask">маска 3х3</param>
        /// <param name="strength">сила с которой маска действует</param>
        /// <returns>изображение с нанесенной маской</returns>
        private Bitmap PutMaskOnImage(Bitmap original, int[,] mask, int strength = 1)
        {
            Bitmap bitmap = original;
            int width = original.Width;
            int height = original.Height;

            int[,] allPixR = new int[width, height];
            int[,] allPixG = new int[width, height];
            int[,] allPixB = new int[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    allPixR[i, j] = original.GetPixel(i, j).R;
                    allPixG[i, j] = original.GetPixel(i, j).G;
                    allPixB[i, j] = original.GetPixel(i, j).B;
                }
            }

            int new_rx;
            int new_gx;
            int new_bx;
            int rc, gc, bc;
            for (int i = 1; i < original.Width - 1; i++)
            {
                for (int j = 1; j < original.Height - 1; j++)
                {

                    new_rx = 0;
                    new_gx = 0;
                    new_bx = 0;

                    

                    for (int wi = -1; wi < 2; wi++)
                    {
                        for (int hw = -1; hw < 2; hw++)
                        {
                            new_rx += mask[wi + 1, hw + 1] * allPixR[i + hw, j + wi];
                            new_gx += mask[wi + 1, hw + 1] * allPixG[i + hw, j + wi];
                            new_bx += mask[wi + 1, hw + 1] * allPixB[i + hw, j + wi];  
                        }
                    }
                    new_rx = Math.Clamp(new_rx/ strength, 0, 255);
                    new_gx = Math.Clamp(new_gx/ strength, 0, 255);
                    new_bx = Math.Clamp(new_bx/ strength, 0, 255);
                    bitmap.SetPixel(i, j, Color.FromArgb(new_rx, new_gx, new_bx));
                }
            }
            return bitmap;
        }


    }
}
