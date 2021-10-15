using System;
using System.IO;
using System.Drawing;

public class ImageHelper
{
    /// <summary>
    /// Creates a System.Drawing.Icon object by reading the bytes from a file path into a memorystream.
    /// </summary>
    /// <param name="icoFilePath">The (absolute) full path to the icon (.ico) file.</param>
    /// <returns>System.Drawing.Icon object.</returns>
    public static Icon GetIconFromFile(string icoFilePath)
    {
        using (MemoryStream ms = new MemoryStream(File.ReadAllBytes(icoFilePath)))
        {
            return new Icon(ms);
        }
    }

    /// <summary>
    /// Extracts the "associated" icon from any given file.
    /// </summary>
    /// <param name="filePath">The (absolute) full path to the target file.</param>
    /// <returns>Attempts to extract a System.Drawing.Icon object from the provided file path.</returns>
    public static Icon IconFromFilePath(string filePath)
    {
        return Icon.ExtractAssociatedIcon(filePath);
    }
}
