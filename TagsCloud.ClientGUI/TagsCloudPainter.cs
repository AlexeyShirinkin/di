﻿using System;
using System.Drawing;
using System.Windows.Forms;
using TagsCloud.ClientGUI.Infrastructure;
using TagsCloud.Core;

namespace TagsCloud.ClientGUI
{
    public class TagsCloudPainter
    {
        private readonly FontSetting font;
        private readonly PictureBoxImageHolder pictureBox;
        private readonly Palette palette;
        private readonly PathSettings pathSettings;
        private readonly SpiralSettings spiralSettings;

        public TagsCloudPainter(PictureBoxImageHolder pictureBox, Palette palette,
            FontSetting font, SpiralSettings spiralSettings, PathSettings pathSettings)
        {
            this.pictureBox = pictureBox;
            this.palette = palette;
            this.font = font;
            this.spiralSettings = spiralSettings;
            this.pathSettings = pathSettings;
        }

        public void Paint()
        {
            using (var graphics = pictureBox.StartDrawing())
            {
                var imageSize = pictureBox.GetImageSize();
                graphics.FillRectangle(new SolidBrush(palette.BackgroundColor), 0, 0,
                    imageSize.Width, imageSize.Height);

                var words = TagsHelper.GetWords(pathSettings.PathToText, pathSettings.PathToBoringWords,
                    pathSettings.PathToDictionary, pathSettings.PathToAffix);
                var rectangles = TagsHelper.GetRectangles(imageSize, words,
                    spiralSettings.SpiralParameter, font.MainFont.Size);

                for (var i = 0; i < rectangles.Count; ++i)
                {
                    var drawFormat = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    graphics.DrawRectangle(new Pen(Color.White), rectangles[i]);

                    using (var currentFont = new Font(font.MainFont.FontFamily,
                        (int) (font.MainFont.Height / 2 * Math.Log(words[i].Item2 + 1)),
                        font.MainFont.Style))
                    {
                        graphics.DrawString(words[i].Item1, currentFont,
                            new SolidBrush(palette.ForeColor), rectangles[i], drawFormat);
                        currentFont.Dispose();
                    }
                }
            }

            pictureBox.Refresh();
            Application.DoEvents();
        }
    }
}