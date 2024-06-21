using Citrus_Browser.Lemonaid_Classes;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Policy;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace Citrus_Browser.Lemoaid_Classes
{
    public class Deserializer
    {
        public static Document Deserialize(string webpage) // Deserialize the Document
        {
            try
            {
                XDocument parsed = XDocument.Parse(webpage); //Parse the Psudo-XML
                InfoHeader info = ProcessInfoHeader(parsed); // Create an Info Header
                Page page = ProcessPage(parsed); // Create A Page
                return new Document(info, page); // Return a Document
            }
            catch (Exception ex) // Something Broke
            {
                Console.WriteLine($"Error during deserialization: {ex.Message}"); // Log The Error
                return null; //Return NULL(Prompts an Error)
            }
        }
        private static InfoHeader ProcessInfoHeader(XDocument parsed) // Process The Info Header
        {
            try
            {
                string pagename = parsed.Root?.Element("info")?.Element("title")?.Value ?? "Lemonaid Webpage"; //Get the Pagename from the Title Tag. If NULL, Default to "Lemonaid Webpage"
                int pageversion = int.TryParse(parsed.Root?.Element("info")?.Element("version")?.Value, out int version) ? version : 1; // Just a Version Marker. If NULL, Default to 1.
                string authorname = parsed.Root?.Element("info")?.Element("author")?.Value ?? "Unknown"; //Get the author name. If NULL, Default to "Unknown".
                string lang = parsed.Root?.Element("info")?.Element("language")?.Value ?? "en"; //Get the page language. If NULL, Default to English.
                SolidColorBrush backgroundcolor = Tools.ParseColor(parsed.Root?.Element("info")?.Element("background")?.Value ?? "White") ?? Tools.ParseColor("White"); // Get the page background color. If NULL, default to White.

                return new InfoHeader(pagename, pageversion, authorname, lang, backgroundcolor); //Return an InfoHeader.
            }
            catch (Exception ex) // Something Broke
            {
                Console.WriteLine($"Error processing info header: {ex.Message}"); // Log the Error
                return new InfoHeader("Lemonaid Webpage", 1, "John Doe", "en", Tools.ParseColor("White")); //Return A Default Header. You dont need to include an info header, as a default will be used.
            }
        }
        private static Page ProcessPage(XDocument parsed) // Process the Page
        {
            Page page = new Page(); // Create a page to add tags to

            try
            {
                foreach (var element in parsed.Root?.Element("page")?.Elements()) //For all Elements in the page, process them to tags.
                {
                    switch (element.Name.LocalName)
                    {
                        case "text": //If its a text tag, process it as such
                            page.tags.Add(ProcessTextTag(element));
                            break;
                        case "image": //If its an image tag, process it as such
                            page.tags.Add(ProcessImageTag(element));
                            break;
                        default: //Log the Error and Check Next tag
                            Console.WriteLine($"Unknown tag type: {element.Name.LocalName}");
                            break;
                    }
                }
            }
            catch (Exception ex) //Something Broke, or there are no tags.
            {
                Console.WriteLine($"Error processing page content: {ex.Message}");
            }

            return page; //Return the page
        }

        private static ImageTag ProcessImageTag(XElement element) //Process an Image Tag
        {
            string name = element.Attribute("name")?.Value; // Set the name to be referenced later. Can Be Null If not going to be referenced
            using var client = new HttpClient(); //Create an HTTP Client to download the image.
            //Buncha HTTP Stuff
            HttpResponseMessage response = client.GetAsync(element.Value).Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"HTTP request failed with status code {response.StatusCode}");
            }

            byte[] content = response.Content.ReadAsByteArrayAsync().Result;
            BitmapImage image = Tools.byteArrayToImage(content); //Process the Byte Array to a JPEG Image.
            /*
             Parse A Buncha Strings to Doubles
            */
            double opacity;
            if (!double.TryParse(element.Attribute("opacity")?.Value, out opacity))
            {
                opacity = 1.0; // Default value if parsing fails
            }
            double width;
            if (!double.TryParse(element.Attribute("width")?.Value, out width))
            {
                width = image.PixelWidth; // Default value if parsing fails
            }
            double height;
            if (!double.TryParse(element.Attribute("height")?.Value, out height))
            {
                height = image.PixelHeight;
            }
            //Get Preferred Alignment
            Tools.HorizontalAlignment alignment = element.Attribute("horizontalalignment")?.Value.ToLower() switch
            {
                "left" => Tools.HorizontalAlignment.Left,
                "center" => Tools.HorizontalAlignment.Center,
                "right" => Tools.HorizontalAlignment.Right,
                _ => Tools.HorizontalAlignment.Left
            };
            // Parse yet another string to a double
            double radius;
            if (!double.TryParse(element.Attribute("radius")?.Value, out radius))
            {
                radius = 0;
            }
            return new ImageTag(name, image, opacity, width, height, alignment, radius); // Return an Image Tag
        }
        private static TextTag ProcessTextTag(XElement element) // Process a text tag
        {
            try
            {
                string name = element.Attribute("name")?.Value; //Name to be referenced
                int fontSize = int.TryParse(element.Attribute("size")?.Value, out int size) ? size : 12; // Parse a string to an INT(Change to double later), If NULL, default to 12
                FontFamily family = new FontFamily(element.Attribute("font")?.Value ?? "Arial"); // Get the font family. If NULL, Default to Arial
                SolidColorBrush foreground = Tools.ParseColor(element.Attribute("foreground")?.Value ?? "Black") ?? Tools.ParseColor("Black"); //Get the Foreground Color. If NULL, Default to Black
                SolidColorBrush background = Tools.ParseColor(element.Attribute("background")?.Value ?? "Transparent") ?? Tools.ParseColor("Transparent"); // Get the Background Color. If NULL, Default to Transparent

                Tools.HorizontalAlignment alignment = element.Attribute("horizontalalignment")?.Value.ToLower() switch //Process Horizontal Alignment
                {
                    "left" => Tools.HorizontalAlignment.Left,
                    "center" => Tools.HorizontalAlignment.Center,
                    "right" => Tools.HorizontalAlignment.Right,
                    _ => Tools.HorizontalAlignment.Left
                };

                return new TextTag(element.Value, name, fontSize, family, foreground, background, alignment); //Return TextTag
            }
            catch (Exception ex) //Oops
            {
                Console.WriteLine($"Error processing text tag: {ex.Message}"); // Log the Error
                return new TextTag(string.Empty, null, 12, new FontFamily("Arial"), Tools.ParseColor("Black"), Tools.ParseColor("Transparent"), Tools.HorizontalAlignment.Left); // Return Default TextTag
            }
        }
    }
}
