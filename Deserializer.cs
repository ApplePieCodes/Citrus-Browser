using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Xml.Linq;

namespace Citrus_Browser.Lemoaid_Classes
{
    public class Deserializer
    {
        /// <summary>
        /// Deserialize the webpage content into a Document object.
        /// </summary>
        /// <param name="webpage">The webpage content as a string.</param>
        /// <returns>A Document object or null if deserialization fails.</returns>
        public static Document Deserialize(string webpage)
        {
            try
            {
                XDocument parsed = XDocument.Parse(webpage);
                InfoHeader info = ProcessInfoHeader(parsed);
                Page page = ProcessPage(parsed);
                return new Document(info, page);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during deserialization: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Process the info header section of the XML document.
        /// </summary>
        /// <param name="parsed">The parsed XDocument.</param>
        /// <returns>An InfoHeader object.</returns>
        private static InfoHeader ProcessInfoHeader(XDocument parsed)
        {
            try
            {
                string pagename = parsed.Root?.Element("info")?.Element("title")?.Value ?? "Lemonaid Webpage";
                int pageversion = int.TryParse(parsed.Root?.Element("info")?.Element("version")?.Value, out int version) ? version : 1;
                string authorname = parsed.Root?.Element("info")?.Element("author")?.Value ?? "John Doe";
                string lang = parsed.Root?.Element("info")?.Element("language")?.Value ?? "en";
                SolidColorBrush backgroundcolor = Tools.ParseColor(parsed.Root?.Element("info")?.Element("background")?.Value ?? "White") ?? Tools.ParseColor("White");

                return new InfoHeader(pagename, pageversion, authorname, lang, backgroundcolor);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing info header: {ex.Message}");
                return new InfoHeader("Lemonaid Webpage", 1, "John Doe", "en", Tools.ParseColor("White"));
            }
        }

        /// <summary>
        /// Process the page section of the XML document.
        /// </summary>
        /// <param name="parsed">The parsed XDocument.</param>
        /// <returns>A Page object containing the tags.</returns>
        private static Page ProcessPage(XDocument parsed)
        {
            Page page = new Page();

            try
            {
                foreach (var element in parsed.Root?.Element("page")?.Elements())
                {
                    switch (element.Name.LocalName)
                    {
                        case "text":
                            page.tags.Add(ProcessTextTag(element));
                            break;
                        // Add cases for other tag types as needed
                        default:
                            Console.WriteLine($"Unknown tag type: {element.Name.LocalName}");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing page content: {ex.Message}");
            }

            return page;
        }

        /// <summary>
        /// Process a text tag element.
        /// </summary>
        /// <param name="element">The XElement representing the text tag.</param>
        /// <returns>A TextTag object.</returns>
        private static TextTag ProcessTextTag(XElement element)
        {
            try
            {
                string name = element.Attribute("name")?.Value; // This can be null
                int fontSize = int.TryParse(element.Attribute("size")?.Value, out int size) ? size : 12;
                FontFamily family = new FontFamily(element.Attribute("font")?.Value ?? "Arial");
                SolidColorBrush foreground = Tools.ParseColor(element.Attribute("foreground")?.Value ?? "Black") ?? Tools.ParseColor("Black");
                SolidColorBrush background = Tools.ParseColor(element.Attribute("background")?.Value ?? "Transparent") ?? Tools.ParseColor("Transparent");

                Tools.HorizontalAlignment alignment = element.Attribute("horizontalalignment")?.Value.ToLower() switch
                {
                    "left" => Tools.HorizontalAlignment.Left,
                    "center" => Tools.HorizontalAlignment.Center,
                    "right" => Tools.HorizontalAlignment.Right,
                    _ => Tools.HorizontalAlignment.Left
                };

                return new TextTag(element.Value, name, fontSize, family, foreground, background, alignment);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing text tag: {ex.Message}");
                return new TextTag(string.Empty, null, 12, new FontFamily("Arial"), Tools.ParseColor("Black"), Tools.ParseColor("Transparent"), Tools.HorizontalAlignment.Left);
            }
        }
    }
}
