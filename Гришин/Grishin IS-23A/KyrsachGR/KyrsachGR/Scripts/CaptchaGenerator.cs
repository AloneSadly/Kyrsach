using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KyrsachGR
{
    public class CaptchaGenerator
    {
        private static readonly char[] Chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789".ToCharArray();

        private static readonly Random Rnd = new Random();

        public string Text { get; private set; }

        /// <summary>
        /// Генерирует новую капчу и возвращает её изображение.
        /// </summary>
        public Bitmap Generate(int width = 150, int height = 50)
        {
            char[] code = new char[4];
            for (int i = 0; i < code.Length; i++)
            {
                code[i] = Chars[Rnd.Next(Chars.Length)];
            }
            Text = new string(code);

            Bitmap bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);

                for (int i = 0; i < 15; i++)
                {
                    g.DrawLine(new Pen(Color.LightGray, 1),
                        Rnd.Next(width), Rnd.Next(height),
                        Rnd.Next(width), Rnd.Next(height));
                }

                for (int i = 0; i < 100; i++)
                {
                    bmp.SetPixel(Rnd.Next(width), Rnd.Next(height), Color.Gray);
                }

                for (int i = 0; i < code.Length; i++)
                {
                    Font font = new Font("Arial", Rnd.Next(16, 22), FontStyle.Bold | FontStyle.Italic);

                    float x = 10 + (i * 30) + Rnd.Next(-5, 5);
                    float y = Rnd.Next(5, 15);

                    g.TranslateTransform(x, y);
                    g.RotateTransform(Rnd.Next(-15, 15));
                    g.DrawString(code[i].ToString(), font, Brushes.Black, 0, 0);

                    g.ResetTransform();
                }
            }

            return bmp;
        }
    }
}
