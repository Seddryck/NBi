using System.IO;
using System.Reflection;

namespace NBi.Xml.Testing;

public class FileOnDisk
{
    private static readonly object locker = new ();

    /// <summary>
    /// Copies the contents of input to output. Doesn't close either stream.
    /// </summary>
    public static void CopyStream(Stream input, Stream output)
    {
        byte[] buffer = new byte[8 * 1024];
        int len;
        while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
        {
            output.Write(buffer, 0, len);
        }
    }

    /// <summary>
    /// Create a temporary file based on the embedded file
    /// </summary>
    /// <param name="filename">QueryFile with extension to create</param>
    /// <returns>The fullpath of the file created</returns>
    public static string CreatePhysicalFile(string filename, string resource)
    {
        //if filename starts by a directory separator remove it
        if (filename.StartsWith(Path.DirectorySeparatorChar.ToString()))
            filename = filename.Substring(1);

        //Build the fullpath for the file to read
        var fullpath = GetDirectoryPath() + filename;

        //create the directory if needed
        if (!Directory.Exists(Path.GetDirectoryName(fullpath)))
            Directory.CreateDirectory(Path.GetDirectoryName(fullpath) ?? string.Empty);

        lock (locker)
        {
            //delete it if already existing
            if (File.Exists(fullpath))
                File.Delete(fullpath);

            // A Stream is needed to read the XLS document.
            using (var stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream(resource)
                                           ?? throw new FileNotFoundException())
            {
                if (stream == null)
                    throw new FileNotFoundException(resource);

                //Open another stream to persist the file on disk
                using (Stream file = File.OpenWrite(fullpath))
                {
                    CopyStream(stream, file);
                }
            }
        }
        return fullpath;
    }

    /// <summary>
    /// Returns the Directory into which the test are executed
    /// </summary>
    /// <returns>An existing temporary path to execute the tests</returns>
    public static string GetDirectoryPath()
    {
        //Build the fullpath for the file to read
        return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\";
    }
}
