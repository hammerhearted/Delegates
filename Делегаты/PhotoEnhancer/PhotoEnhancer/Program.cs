﻿using System;
using System.Windows.Forms;
using System.Drawing;

namespace PhotoEnhancer
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var mainForm = new MainForm();

            mainForm.AddFilter(new PixelFilter<LighteningParameters>(
                "Осветление/затемнение",
                (pixel, parameters) => pixel * parameters.Coefficient
                ));

            mainForm.AddFilter(new PixelFilter<EmptyParameters>(
                "Оттенки серого",
                (pixel, parameters) =>
                {
                    var chanel = 0.3 * pixel.R +
                                0.6 * pixel.G +
                                0.1 * pixel.B;

                    return new Pixel(chanel, chanel, chanel);
                }
                ));

            mainForm.AddFilter(new TransformFilter(
                "Отражение по горизонтали",
                size => size,
                (point, size) => new Point(size.Width - point.X - 1, point.Y)
                ));

            mainForm.AddFilter(new TransformFilter(
                "Поворот на 90° против ч. с.",
                size => new Size(size.Height, size.Width),
                (point, size) => new Point(size.Width - point.Y - 1, point.X)
                ));


            mainForm.AddFilter(new TransformFilter<RotationParameters>(
                "Свободное вращение", new RotateTransformer()));

            mainForm.AddFilter(new TransformFilter<PerspectiveParameters>(
                "Перспектива", new PerspectiveTransformer()));

            mainForm.AddFilter(new TransformFilter(
                "Замена чётных строк",
                size => size,
                (point, size) =>
                {
                    if (point.Y % 2 == 0)
                        return point;
                    else
                        return new Point(point.X, point.Y - 1);
                }));

            mainForm.AddFilter(new PixelFilter<SaturationParameters>("Насыщенность", (pixel, parameters) =>
            {
                var OriginalHue = Convertors.GetPixelHue(pixel);
                var OriginalSaturation = Convertors.GetPixelSaturation(pixel);
                var OriginalLightness = Convertors.GetPixelLightness(pixel);

                var newS = OriginalSaturation * (parameters as SaturationParameters).Coefficient;
                if (newS < 1 & newS > 0)
                    return Convertors.HSL2Pixel(OriginalHue, newS, OriginalLightness);
                else
                    return Convertors.HSL2Pixel(OriginalHue, Pixel.Trim(newS), OriginalLightness);
            }));


            Application.Run(mainForm);
        }
    }
}
