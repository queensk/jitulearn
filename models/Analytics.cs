namespace models
{
    public class JituAnalytics
    {
        // Static field to hold the single instance
        private static JituAnalytics _instance = null;

        // Properties
        public int Users { get; set; }
        public int Admin { get; set; }
        public int Courses { get; set; }
        public int Purchased { get; set; }

        private JituAnalytics()
        {
            string currentDir = Directory.GetCurrentDirectory();
            string filepath = Path.Combine(currentDir, "Data", "analytics.txt");

            if (File.Exists(filepath))
            {
                using (StreamReader reader = new StreamReader(filepath))
                {
                    string line = reader.ReadLine();
                    if (line != null)
                    {
                        string[] values = line.Split(',');
                        Users = int.Parse(values[0]);
                        Admin = int.Parse(values[1]);
                        Courses = int.Parse(values[2]);
                        Purchased = int.Parse(values[3]);
                    }
                }
            }
            else
            {
                Users = 0;
                Admin = 0;
                Courses = 0;
                Purchased = 0;
                string newData = $"{Users},{Admin},{Courses},{Purchased}";
                File.WriteAllText(filepath, newData);
            }
        }


        public static JituAnalytics GetInstance()
        {
            if (_instance == null)
            {
                _instance = new JituAnalytics();
            }
            return _instance;
        }

        public void Save()
        {
            string currentDir = Directory.GetCurrentDirectory();
            string filepath = Path.Combine(currentDir, "Data", "analytics.txt");

            string newData = $"{Users},{Admin},{Courses},{Purchased}";

            using (StreamWriter writer = new StreamWriter(filepath))
            {
                writer.WriteLine(newData);
            }
        }

        public void print()
        {
            Console.WriteLine(this);
        }

        public override string ToString()
        {
            return $"Users: {Users}, Admin: {Admin}, Courses: {Courses}, Purchased: {Purchased}" ;
        }
    }
}
