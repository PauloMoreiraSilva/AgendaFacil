using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace PI4Sem.Infra
{
    /// <summary>
    /// Transforma uma página HTML em imagem BMP
    /// </summary>
    public static class WebsiteThumbnailImageGenerator
    {
        /// <summary>
        /// Retorna um thumbnail para a URL solicitada (html -> bmp).
        /// </summary>
        /// <param name="Url">URL.</param>
        /// <param name="BrowserWidth">largura do browser.</param>
        /// <param name="BrowserHeight">altura do browser.</param>
        /// <param name="ThumbnailWidth">largura da imagem.</param>
        /// <param name="ThumbnailHeight">altura da imagem.</param>
        /// <returns>Bitmap da página.</returns>
        public static Bitmap GetWebSiteThumbnail(string Url, int BrowserWidth, int BrowserHeight, int ThumbnailWidth, int ThumbnailHeight)
        {
            WebsiteThumbnailImage thumbnailGenerator = new WebsiteThumbnailImage(Url, BrowserWidth, BrowserHeight, ThumbnailWidth, ThumbnailHeight);
            return thumbnailGenerator.GenerateWebSiteThumbnailImage();
        }

        /// <summary>
        /// Implementa a tradução de página HTML para imagem
        /// </summary>
        private class WebsiteThumbnailImage
        {
            /// <summary>
            /// Inicializa uma instância da classe
            /// </summary>
            /// <param name="Url">URL</param>
            /// <param name="BrowserWidth">largura do browser.</param>
            /// <param name="BrowserHeight">altura do browser.</param>
            /// <param name="ThumbnailWidth">largura da imagem.</param>
            /// <param name="ThumbnailHeight">altura da imagem.</param>
            public WebsiteThumbnailImage(string Url, int BrowserWidth, int BrowserHeight, int ThumbnailWidth, int ThumbnailHeight)
            {
                this.Url = Url;
                this.BrowserWidth = BrowserWidth;
                this.BrowserHeight = BrowserHeight;
                this.ThumbnailHeight = ThumbnailHeight;
                this.ThumbnailWidth = ThumbnailWidth;
            }

            /// <summary>
            /// URL da página a ser lida
            /// </summary>
            public string Url { get; set; }

            /// <summary>
            /// Retorna a imagem
            /// </summary>
            public Bitmap ThumbnailImage { get; private set; }

            /// <summary>
            /// Largura da imagem
            /// </summary>
            public int ThumbnailWidth { get; set; }

            /// <summary>
            /// Altura da imagem
            /// </summary>
            public int ThumbnailHeight { get; set; }

            /// <summary>
            /// Largura do browser
            /// </summary>
            public int BrowserWidth { get; set; }

            /// <summary>
            /// Altura do browser
            /// </summary>
            public int BrowserHeight { get; set; }

            /// <summary>
            /// Gera a imagem
            /// </summary>
            /// <returns>Bitmap.</returns>
            public Bitmap GenerateWebSiteThumbnailImage()
            {
                Thread m_thread = new Thread(new ThreadStart(MyGenerateWebSiteThumbnailImage));
                m_thread?.SetApartmentState(ApartmentState.STA);
                m_thread?.Start();
                m_thread?.Join();
                return ThumbnailImage;
            }

            /// <summary>
            /// Método interno para geração de bitmap a partir de uma página
            /// </summary>
            private void MyGenerateWebSiteThumbnailImage()
            {
                WebBrowser m_WebBrowser = new WebBrowser
                {
                    ScrollBarsEnabled = false
                };
                m_WebBrowser.Navigate(Url);
                m_WebBrowser.DocumentCompleted += WebBrowser_DocumentCompleted;
                while (m_WebBrowser.ReadyState != WebBrowserReadyState.Complete)
                {
                    Application.DoEvents();
                }

                m_WebBrowser.Dispose();
            }

            /// <summary>
            /// evento que define que o documento (página html) foi completada
            /// </summary>
            /// <param name="sender"> sender.</param>
            /// <param name="e">evento.</param>
            private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
            {
                using WebBrowser m_WebBrowser = (WebBrowser)sender;
                m_WebBrowser.ClientSize = new Size(BrowserWidth, BrowserHeight);
                m_WebBrowser.ScrollBarsEnabled = false;
                ThumbnailImage = new Bitmap(m_WebBrowser.Bounds.Width, m_WebBrowser.Bounds.Height);

                ThumbnailImage.SetResolution(600.0f, 600.0f);

                m_WebBrowser.BringToFront();
                m_WebBrowser.DrawToBitmap(ThumbnailImage, m_WebBrowser.Bounds);
            }
        }
    }
}