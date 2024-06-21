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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void DownloadPage(object sender, RoutedEventArgs e)
        {
            string url = urlBar.Text; //Get the url from the urlBar

            if ((string.IsNullOrWhiteSpace(url) || !Uri.IsWellFormedUriString(url, UriKind.Absolute)) && url.EndsWith(".aid")) //Check if the string is a URL AND is a Lemonaid File
            {
                MessageBox.Show("Please enter a valid URL.", "Invalid URL", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            goButton.IsEnabled = false; // Disable the Go Button While Page Loads

            using HttpClient client = new HttpClient(); //Create an HTTP Client
            try
            {
                HttpResponseMessage response = await client.GetAsync(url); //Get Webpage Content
                response.EnsureSuccessStatusCode(); //Make Sure Response is Success

                string content = await response.Content.ReadAsStringAsync(); // Get Content of response.

                Document doc = Deserializer.Deserialize(content); //Convert Page To Tags

                if (doc == null) //If deserialization Failed
                {
                    throw new Exception("Failed to deserialize the content.");
                }

                DisplayDocumentContent(doc); //Display Deserialized Content
            }
            catch (HttpRequestException ex) //If HTTP Error Occured
            {
                MessageBox.Show($"HTTP error occurred: {ex.Message}", "HTTP Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (TaskCanceledException ex) // If Request Timed Out
            {
                MessageBox.Show("Request timed out. Please try again.", "Timeout Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex) // Unspecified Error
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally //Re-Enable The Go Button
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
            gridView.Children.Clear(); //Clear The Page
            gridView.Background = doc.info.background; // Set The Page Background

            foreach (var item in doc.content.tags) //For Each Item in the tags object array
            {
                if (item is TextTag textTag) //if the tag is a TextTag
                {
                    HorizontalAlignment horizontalAlignment = textTag.horizontalAlignment switch // Set the Horizontal Alignment
                    {
                        Tools.HorizontalAlignment.Left => HorizontalAlignment.Left,
                        Tools.HorizontalAlignment.Center => HorizontalAlignment.Center,
                        Tools.HorizontalAlignment.Right => HorizontalAlignment.Right,
                        _ => HorizontalAlignment.Left
                    };

                    var textBlock = new TextBlock //Create a textBlock
                    {
                        Text = textTag.text, //Set the text
                        Name = textTag.name, //Set a name to be referenced
                        FontSize = textTag.fontSize, // Set the font size
                        FontFamily = textTag.fontFamily,//Set the font Family
                        Foreground = textTag.foreground,// Set the foreground color
                        Background = textTag.background,//Set the background color
                        HorizontalAlignment = horizontalAlignment //Set the Horizontal Alignment
                    };
                    gridView.Children.Add(textBlock); // Add The TextBlock To the GridView
                }
                else if (item is ImageTag imageTag) //If the tag is an ImageTag
                {
                    HorizontalAlignment horizontalAlignment = imageTag.horizontalAlignment switch //Set the horizontal alignment
                    {
                        Tools.HorizontalAlignment.Left => HorizontalAlignment.Left,
                        Tools.HorizontalAlignment.Center => HorizontalAlignment.Center,
                        Tools.HorizontalAlignment.Right => HorizontalAlignment.Right,
                        _ => HorizontalAlignment.Left
                    };
                    ImageBrush imageBrush = new ImageBrush(); //Create a temporary imagebrush
                    imageBrush.ImageSource = imageTag.image; // Set the Image Source to the image
                    var imageView = new Rectangle // Create a rectangle
                    {
                        Name = imageTag.name, //Set a name to be referenced
                        Fill = imageBrush, //Set the image
                        Opacity = imageTag.opacity, //Set the opacity
                        HorizontalAlignment = horizontalAlignment, // Set the horizontal alignment
                        Width = imageTag.width / 2, // Set the width
                        Height = imageTag.height / 2, // Set the height
                        RadiusX = imageTag.CornerRadius, // Set the Corner Radius
                        RadiusY = imageTag.CornerRadius // Ditto
                    };
                    gridView.Children.Add(imageView); // Add the rectangle to the GridView
                }

                // Handle other tag types as needed
            }
        }
    }
}
