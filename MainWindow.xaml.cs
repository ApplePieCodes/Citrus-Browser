using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Citrus_Browser.Lemoaid_Classes;
using Citrus_Browser.Lemonaid_Classes;

namespace Citrus_Browser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Async method to handle downloading the page.
        /// </summary>
        private async void DownloadPage(object sender, RoutedEventArgs e)
        {
            string url = urlBar.Text;

            if (string.IsNullOrWhiteSpace(url) || !Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                MessageBox.Show("Please enter a valid URL.", "Invalid URL", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            goButton.IsEnabled = false;

            using HttpClient client = new HttpClient();
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

                Document doc = Deserializer.Deserialize(content);

                if (doc == null)
                {
                    throw new Exception("Failed to deserialize the content.");
                }

                DisplayDocumentContent(doc);
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"HTTP error occurred: {ex.Message}", "HTTP Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (TaskCanceledException ex)
            {
                MessageBox.Show("Request timed out. Please try again.", "Timeout Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                goButton.IsEnabled = true;
            }
        }

        /// <summary>
        /// Displays the document content on the UI.
        /// </summary>
        /// <param name="doc">The document object containing content to display.</param>
        private void DisplayDocumentContent(Document doc)
        {
            gridView.Children.Clear();
            gridView.Background = doc.info.background;

            foreach (var item in doc.content.tags)
            {
                if (item is TextTag textTag)
                {
                    HorizontalAlignment horizontalAlignment = textTag.horizontalAlignment switch
                    {
                        Tools.HorizontalAlignment.Left => HorizontalAlignment.Left,
                        Tools.HorizontalAlignment.Center => HorizontalAlignment.Center,
                        Tools.HorizontalAlignment.Right => HorizontalAlignment.Right,
                        _ => HorizontalAlignment.Left
                    };

                    var textBlock = new TextBlock
                    {
                        Text = textTag.text,
                        Name = textTag.name,
                        FontSize = textTag.fontSize,
                        FontFamily = textTag.fontFamily,
                        Foreground = textTag.foreground,
                        Background = textTag.background,
                        HorizontalAlignment = horizontalAlignment
                    };
                    gridView.Children.Add(textBlock);
                }
                else if (item is ImageTag imageTag)
                {
                    HorizontalAlignment horizontalAlignment = imageTag.horizontalAlignment switch
                    {
                        Tools.HorizontalAlignment.Left => HorizontalAlignment.Left,
                        Tools.HorizontalAlignment.Center => HorizontalAlignment.Center,
                        Tools.HorizontalAlignment.Right => HorizontalAlignment.Right,
                        _ => HorizontalAlignment.Left
                    };
                    ImageBrush imageBrush = new ImageBrush();
                    imageBrush.ImageSource = imageTag.image;
                    var rect = new Rectangle();
                    var imageView = new Rectangle
                    {
                        Name = imageTag.name,
                        Fill = imageBrush,
                        Opacity = imageTag.opacity,
                        HorizontalAlignment = horizontalAlignment,
                        Width = imageTag.width / 2,
                        Height = imageTag.height / 2,
                        RadiusX = imageTag.CornerRadius,
                        RadiusY = imageTag.CornerRadius
                    };
                    gridView.Children.Add(imageView);
                }

                // Handle other tag types as needed
            }
        }
    }
}
