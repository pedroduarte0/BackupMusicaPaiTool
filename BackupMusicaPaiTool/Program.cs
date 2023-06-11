using System.IO;
using System.Diagnostics;

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
            string[] files = Directory.GetFiles(path, "*.wav", SearchOption.TopDirectoryOnly);

            // For each file, memorize the last write time to a variable and restore it.
            foreach (string file in files)
            {
                // Get file last write time
                FileInfo fileInfo = new(file);
                DateTime lastWriteTime = fileInfo.LastWriteTime;

                // Convert to mp4
                string convertedFile = ConvertToMp4(fileInfo, "/home/pedro/Temp/audioconversions");

                // also  https://stackoverflow.com/questions/54456493/ffmpeg-keep-original-file-date
                // Display converted file name
                // TODO 

                // Set file last write time to today
                File.SetLastWriteTime(convertedFile, lastWriteTime);
            }
        }
        
        private static string ConvertToMp4(FileInfo inputFileInfo, string outputFolder)
        {
            // Prepare the process to run
            ProcessStartInfo start = new();

            string outputFile = Path.Combine(outputFolder,
                Path.ChangeExtension(inputFileInfo.Name, ".m4a"));// Path.GetFileNameWithoutExtension(inputFileInfo.Name) + ".m4a";

            string arguments = $@"-loglevel warning -stats -hide_banner -nostdin -i ""{inputFileInfo.FullName}"" -vn -sn -map 0:a:? -c:a: alac -map_metadata 0  -threads 4 -y ""{outputFile}""";

            // Enter in the command line arguments, everything you would enter after the executable name itself
            start.Arguments = arguments; 
            // Enter the executable to run, including the complete path
            start.FileName = "/usr/bin/ffmpeg";
            // Do you want to show a console window?
            start.WindowStyle = ProcessWindowStyle.Normal;
            start.CreateNoWindow = true;
            int exitCode;

            // Run the external process & wait for it to finish
            using (Process proc = Process.Start(start))
            {
                proc.WaitForExit();

                // Retrieve the app's exit code
                exitCode = proc.ExitCode;
            }
            return outputFile;
        }
    }
}