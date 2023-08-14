namespace models
{
    public class User
    {
        private string id;
        private string name;
        private string email;
        private string password;
        private int credit;
        private List<Courses> courses;

        // user constructor
        public User(string id, string name, string email, string password, int credit=0, List<Courses> courses=null)
        {
            this.id = id;
            this.name = name;
            this.email = email;
            this.password = password;
            this.credit = credit;
            this.courses = new List<Courses>();
        }

        // getters and setters
        public string Name { get => name; set => name = value; }

        public string Email { get => email; set => email = value; }

        public string Password { get => password; set => password = value; }

        public string Id { get => id; set => id = value; }

        public int Credit { get => credit; set => credit = value; }

        public List<Courses> Courses { get => courses; set => courses = value; }

        public override string ToString()
        {
            return $"{Id},{Name},{Email},{Password},{Credit}, {Courses}";
        }

        public void print(){
            Console.WriteLine(this);
        }

    public void Save()
    {
        string currentDir = Directory.GetCurrentDirectory();
        string filepath = Path.Combine(currentDir, "Data", "user.txt");

        string courseIds = ";" + string.Join(",", Courses.Select(course => course.Id));
        string data = $"{Id},{Name},{Email},{Password},{Credit},{courseIds}";

        bool updated = false;

        string[] lines = File.ReadAllLines(filepath);

        for (int i = 0; i < lines.Length; i++)
        {
            string[] fields = lines[i].Split(',');

            if (fields[0] == Id.ToString())
            {
                lines[i] = data;
                updated = true;
                break;
            }
        }

        if (!updated)
        {
            using (FileStream fs = new FileStream(filepath, FileMode.Append, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine(data);
            }
        }
        else
        {
            File.WriteAllLines(filepath, lines);
        }
    }

    // Add a method to the user class that prints the courses
    public void PrintCourses()
    {
        if (Courses.Count > 0)
        {
            Console.WriteLine($"Courses for {Name}:");
            foreach (Courses course in Courses)
            {
                course.Print();
            }
        }
        else
        {
            Console.WriteLine($"{Name} has no courses.");
        }
    }
    }
}

