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
        private JituAnalytics jituAnalytics = JituAnalytics.GetInstance();

        // user constructor
        public User(string id, string name, string email, string password, int credit=0, List<Courses> courses=null)
        {
            this.id = id;
            this.name = name;
            this.email = email;
            this.password = password;
            this.credit = credit;
            this.courses = courses ?? new List<Courses>();
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

        string courseIds = Courses.Count > 0 ? ";" + string.Join(",", Courses.Select(course => course.Id)) : string.Empty;
        string data = $"{Id},{Name},{Email},{Password},{Credit}{courseIds}";

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
                Console.WriteLine($"Course: {course.Name}, Id: {course.Id}, Credit: {course.Rating}");
            }
        }
        else
        {
            Console.WriteLine($"{Name} has no courses.");
        }
    }

    public void PrintFullUser()
    {
        Console.WriteLine($"User Details for {Name} (ID: {Id}):");
        Console.WriteLine($"Email: {Email}");
        Console.WriteLine($"Credit: {Credit}");

        if (Courses.Count > 0)
        {
            Console.WriteLine("Courses:");
            foreach (Courses course in Courses)
            {
                Console.WriteLine($"- Course: {course.Name}, Ratting: {course.Rating} out of 5, Price: {course.Amount} Ksh");
            }
        }
        else
        {
            Console.WriteLine("No courses purchased.");
        }
    }


    }
}

