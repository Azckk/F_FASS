using Common.AspNetCore.Extensions;
using Common.NETCore.Helpers;
using Microsoft.Extensions.Caching.Distributed;
using SkiaSharp;

namespace FASS.Web.Api.Utility
{
    public static class Captcha
    {
        private readonly static string[] _fonts = { "Arial", "Calibri", "Times New Roman", "Verdana" };
        private readonly static SKColor[] _colors = { SKColors.Black, SKColors.Red, SKColors.DarkBlue, SKColors.Green, SKColors.Orange, SKColors.Brown, SKColors.DarkCyan, SKColors.Purple };

        public static (string, byte[]) Create(int codeNum = 4, int width = 0, int height = 36, int lineNum = 8, int lineWidth = 4, int pointNum = 16, int pointWidth = 8)
        {
            var codes = GuidHelper.GetRandomId(codeNum);
            var image = new SKBitmap(width == 0 ? codes.Length * 25 : width, height);
            var canvas = new SKCanvas(image);
            canvas.Clear(SKColors.White);
            var codeStyle = new SKPaint();
            foreach (var code in codes)
            {
                var dot = new SKPoint(image.Height / 2f, image.Height / 2f);
                var angle = Random.Shared.Next(-45, 45);
                canvas.Translate(dot.X, dot.Y);
                canvas.RotateDegrees(angle);

                codeStyle.Color = _colors[Random.Shared.Next(_colors.Length)];
                var typeface = SKTypeface.FromFamilyName(_fonts[Random.Shared.Next(_fonts.Length)], SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright);
                var font = new SKFont(typeface, image.Height / 1.5f);
                canvas.DrawText(code.ToString(), 0, image.Height / 4f, SKTextAlign.Center, font, codeStyle);

                canvas.RotateDegrees(-angle);
                canvas.Translate(0, -dot.Y);
            }
            canvas.ResetMatrix();
            var lineStyle = new SKPaint();
            for (int i = 0; i < lineNum; i++)
            {
                var x0 = Random.Shared.Next(image.Width);
                var y0 = Random.Shared.Next(image.Height);
                var x1 = Random.Shared.Next(image.Width);
                var y1 = Random.Shared.Next(image.Height);
                lineStyle.Color = new SKColor((uint)Random.Shared.Next());
                lineStyle.StrokeWidth = Random.Shared.Next(lineWidth);
                canvas.DrawLine(x0, y0, x1, y1, lineStyle);
            }
            canvas.ResetMatrix();
            var pointStyle = new SKPaint();
            for (int i = 0; i < pointNum; i++)
            {
                var x = Random.Shared.Next(image.Width);
                var y = Random.Shared.Next(image.Height);
                pointStyle.Color = new SKColor((uint)Random.Shared.Next());
                pointStyle.StrokeWidth = Random.Shared.Next(pointWidth);
                canvas.DrawPoint(x, y, pointStyle);
            }
            var data = SKImage.FromBitmap(image).Encode(SKEncodedImageFormat.Png, 100);
            return (codes, data.ToArray());
        }

        public static (string, byte[]) Create(IDistributedCache distributedCache, string cacheKey)
        {
            var (code, bytes) = Create();
            distributedCache.Set(cacheKey, SecurityHelper.HashSHA256(code.ToUpper()));
            return (cacheKey, bytes);
        }

        public static bool Check(IDistributedCache distributedCache, string cacheKey, string code)
        {
            var cacheCode = distributedCache.Get<string>(cacheKey);
            return cacheCode == SecurityHelper.HashSHA256(code.ToUpper());
        }
    }
}
