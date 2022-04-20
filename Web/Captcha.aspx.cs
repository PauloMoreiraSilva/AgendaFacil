using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using PI4Sem.DAL;

/// <summary>
/// Agenda Fácil by PI4Sem
/// </summary>
namespace PI4Sem.AgendaFacil
{
    /// <summary>
    /// Gerar imagem Captcha
    /// </summary>
    public partial class Captcha : PI4Sem.Infra.BasePage
    {
        /// <summary>
        /// Evento ao iniciar a página
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void Page_Load(object sender, System.EventArgs e)
        {
            // Para gerar números randômicos.
            Random random = new Random();
            AdmUsuario oAdmUsuario = new AdmUsuario();

            string s = oAdmUsuario.CriarSenha(4, 6);

            // Cria uma imagem bitmap de 32-bit.
            Bitmap bitmap = new Bitmap(200, 62, PixelFormat.Format32bppArgb);

            // Crie um objeto gráfico para o desenho.
            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(0, 0, 200, 62);

            // Preencha o fundo.
            HatchBrush hatchBrush = new HatchBrush(HatchStyle.SmallConfetti,
                                                   Color.DarkGray, Color.White);
            g.FillRectangle(hatchBrush, rect);

            // Configura a fonte do texto.
            SizeF size;
            float fontSize = rect.Height + 20;
            Font font;

            // Ajuste o tamanho da fonte até que o texto
            // se encaixe dentro da imagem.
            do
            {
                fontSize--;
                font = new Font(System.Drawing.FontFamily.GenericSerif.Name,
                                fontSize, FontStyle.Bold);
                size = g.MeasureString(s, font);
            }
            while (size.Width > rect.Width);

            // Configure o formato de texto.
            StringFormat format = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            // Cria uma deformação no formato dos números.
            GraphicsPath path = new GraphicsPath();
            path.AddString(s, font.FontFamily, (int)font.Style, font.Size, rect, format);
            const float v = 4F;
            PointF[] points =
                            {
                             new PointF(
                               random.Next(rect.Width) / v,
                               random.Next(rect.Height) / v),
                             new PointF(
                               rect.Width - (random.Next(rect.Width) / v),
                               random.Next(rect.Height) / v),
                             new PointF(
                               random.Next(rect.Width) / v,
                               rect.Height - (random.Next(rect.Height) / v)),
                             new PointF(
                               rect.Width - (random.Next(rect.Width) / v),
                               rect.Height - (random.Next(rect.Height) / v))
                            };
            Matrix matrix = new Matrix();
            matrix.Translate(0F, 0F);
            path.Warp(points, rect, matrix, WarpMode.Perspective, 0F);

            // Desenha o texto.
            hatchBrush = new HatchBrush(HatchStyle.LargeConfetti,
                                        Color.DarkGray, Color.DarkGray);
            g.FillPath(hatchBrush, path);

            // Coloca um pouco de partículas no fundo.
            int m = Math.Max(rect.Width, rect.Height);
            for (int i = 0; i < (int)(rect.Width * rect.Height / 30F); i++)
            {
                int x = random.Next(rect.Width);
                int y = random.Next(rect.Height);
                int w = random.Next(m / 50);
                int h = random.Next(m / 50);
                g.FillEllipse(hatchBrush, x, y, w, h);
            }

            //Cria uma session com o valor da imagem
            Session["CaptchaImageText"] = s;

            // Define a imagem.
            font.Dispose();
            hatchBrush.Dispose();
            g.Dispose();
            Response.ContentType = "image/GIF";
            bitmap.Save(Response.OutputStream, ImageFormat.Gif);
            bitmap.Dispose();
        }
    }
}