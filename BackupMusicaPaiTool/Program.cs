using System.IO;

namespace BackupMusicaPaiTool
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // If no arguments are passed, show message and exit
            if (args.Length == 0)
            {
                Console.WriteLine("No arguments passed.");
                return;
            }

            // get first argument from args
            string path = args[0];

            // If path is not a directory, show message and exit
            if (!Directory.Exists(path))
            {
                Console.WriteLine("Path is not a directory.");
                return;
            }

            // Get all files from path with search pattern
            string[] files = Directory.GetFiles(path, "*.wav", SearchOption.AllDirectories);

            // For each file, memorize the last write time to a variable and restore it.
            foreach (string file in files)
            {
                // Get file last write time
                FileInfo fileInfo = new(file);
                DateTime lastWriteTime = fileInfo.LastWriteTime;

                Console.WriteLine($"File: {file} - Last write time: {lastWriteTime}");

                // Convert to mp4 here
                // TODO 

                // Display converted file name
                // TODO 

                // Set file last write time to today
                File.SetLastWriteTime(file, lastWriteTime);
            }
        }
    }
}