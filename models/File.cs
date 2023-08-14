namespace models
{
    public class FileStorage
    {
        private readonly string FilePath;

        public FileStorage(string filePath)
        {
            FilePath = filePath;
        }

        public void CreateFile(string filepath)
        {
            if (!File.Exists(filepath))
            {
                File.Create(filepath);
            }
        }

        public string[] ReadFile(string filepath)
        {
            return File.ReadAllLines(filepath);
        }

        // Write an array of strings to a file, overwriting any existing content
        public void WriteFile(string filepath, string[] lines)
        {
            File.WriteAllLines(filepath, lines);
        }

        // Append an array of strings to the end of a file
        public void AppendFile(string filepath, string[] lines)
        {
            using (FileStream fs = new FileStream(filepath, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    foreach (string line in lines)
                    {
                        sw.WriteLine(line);
                    }
                }
            }
        }

        // Check if a file exists and return a boolean value
        public bool FileExists(string filepath)
        {
            return File.Exists(filepath);
        }

    }
}