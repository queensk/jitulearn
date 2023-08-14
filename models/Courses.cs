using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace models
{
    public class Courses
    {
        private string v;

        public Courses(string id, string name, string title, int rating, int amount)
        {
            this.Id = id;
            this.Name = name;
            this.Title = title;
            this.Rating = rating;
            this.Amount = amount;
        }

        [Key]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Title { get; set; }

        [Range(0, 5)]
        public int Rating { get; set; }

        public int Amount { get; set; }

        // create file if not exists
        // public void CreateFile()
        // {
        //     string currentDir = Directory.GetCurrentDirectory();
        //     string filepath = Path.Combine(currentDir,  "Data", "courses.txt");
        //     if (!File.Exists(filepath))
        //     {
        //         File.Create(filepath);
        //     }
        // }

        // print an object
        public void Print()
        {
            Console.WriteLine(this);
        }

        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Title: {Title}, Rating: {Rating}, Amount: {Amount}";
        }

        // Save data to file
        public void Save()
        {
            string currentDir = Directory.GetCurrentDirectory();
            string filepath = Path.Combine(currentDir,  "Data", "courses.txt");
            if (!File.Exists(filepath))
            {
                File.Create(filepath);
            }

            // Create a string that contains the data
            string data = String.Join(",", Id, Name, Title, Rating, Amount);

            // Read the file and check if it contains the same data
            bool duplicate = false;
            string[] lines = File.ReadAllLines(filepath);
            foreach (string line in lines)
            {
                if (line == data)
                {
                    duplicate = true;
                    break;
                }
            }

            // Write the data to the file only if it is not a duplicate
            if (!duplicate)
            {
                using (FileStream fs = new FileStream(filepath, FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(data);
                    }
                }
            }
        }
        
        public static void Update(string Id, String Name, String Title, int Rating, int Amount){
            string currentDir = Directory.GetCurrentDirectory();
            string filepath = Path.Combine(currentDir,  "Data", "courses.txt");
            List<Courses> courses = File.ReadAllLines(filepath)
                                        .Select(line => line.Split(","))
                                        .Select(data => new Courses(data[0], data[1], data[2], int.Parse(data[3]), int.Parse(data[4])))
                                        .ToList();
            Courses course = courses.Find(c => c.Id == Id);
            if (course != null)
            {
                course.Name = Name;
                course.Title = Title;
                course.Rating = Rating;
                course.Amount = Amount;
            }
            File.WriteAllLines(filepath, courses.Select(c => $"{c.Id},{c.Name},{c.Title},{c.Rating},{c.Amount}"));
        }

        public static void Delete(string Id){
            string currentDir = Directory.GetCurrentDirectory();
            string filepath = Path.Combine(currentDir,  "Data", "courses.txt");
            List<Courses> courses = File.ReadAllLines(filepath)
                                        .Select(line => line.Split(","))
                                        .Select(data => new Courses(data[0], data[1], data[2], int.Parse(data[3]), int.Parse(data[4])))
                                        .ToList();
            courses.RemoveAll(c => c.Id == Id);
            File.WriteAllLines(filepath, courses.Select(c => $"{c.Id},{c.Name},{c.Title},{c.Rating},{c.Amount}"));
        }

        public void Patch(){
            string currentDir = Directory.GetCurrentDirectory();
            string filepath = Path.Combine(currentDir,  "Data", "courses.txt");
            string[] lines = File.ReadAllLines(filepath);
            string data = String.Join(",", Id, Name, Title, Rating, Amount);
            for (int i = 0; i < lines.Length; i++)
            {
                string[] line = lines[i].Split(",");
                if (line[0] == Id)
                {
                    string updatedLine = String.Join(",", Id, Name, Title, Rating, Amount);
                    lines[i] = updatedLine;
                    break;
                }
            }
            File.WriteAllLines(filepath, lines);
        }
        public static List<Courses> GetAll()
        {
            string currentDir = Directory.GetCurrentDirectory();
            string filepath = Path.Combine(currentDir,  "Data", "courses.txt");
            string[] lines = File.ReadAllLines(filepath);
            List<Courses> courses = new List<Courses>();
            foreach (string line in lines)
            {
                string[] data = line.Split(",");
                courses.Add(new Courses(data[0], data[1], data[2], int.Parse(data[3]), int.Parse(data[4])));
            }
            return courses;
        }
        public static Courses GetById(string id)
        {
            string currentDir = Directory.GetCurrentDirectory();
            string filepath = Path.Combine(currentDir,  "Data", "courses.txt");
            string[] lines = File.ReadAllLines(filepath);
            foreach (string line in lines)
            {
                string[] data = line.Split(",");
                if (data[0] == id)
                {
                    return new Courses(data[0], data[1], data[2], int.Parse(data[3]), int.Parse(data[4]));
                }
            }
            return null;
        }
        
        public Courses GetByName(string name)
        {
            string currentDir = Directory.GetCurrentDirectory();
            string filepath = Path.Combine(currentDir,  "Data", "courses.txt");
            string[] lines = File.ReadAllLines(filepath);
            foreach (string line in lines)
            {
                string[] data = line.Split(",");
                if (data[1] == name)
                {
                    return new Courses(data[0], data[1], data[2], int.Parse(data[3]), int.Parse(data[4]));
                }
            }
            return null;
        }

        public Courses GetByTitle(string title)
        {
            string currentDir = Directory.GetCurrentDirectory();
            string filepath = Path.Combine(currentDir,  "Data", "courses.txt");
            string[] lines = File.ReadAllLines(filepath);
            foreach (string line in lines)
            {
                string[] data = line.Split(",");
                if (data[2] == title)
                {
                    return new Courses(data[0], data[1], data[2], int.Parse(data[3]), int.Parse(data[4]));
                }
            }
            return null;
        }

        // Get courses by amount and return a list of courses with the same amount
        public List<Courses> GetCoursesByAmount(int amount)
        {
            string currentDir = Directory.GetCurrentDirectory();
            string filepath = Path.Combine(currentDir,  "Data", "courses.txt");
            List<Courses> coursesList = new List<Courses>();

            foreach (string line in File.ReadLines(filepath))
            {
                string[] data = line.Split(",");
                
                if (int.Parse(data[4]) == amount)
                {
                    Courses course = new Courses(data[0], data[1], data[2], int.Parse(data[3]), int.Parse(data[4]));
                    
                    coursesList.Add(course);
                }
            }
            return coursesList;
        }

        // // search course by name return the top 5 course with like search name
        // public Courses SerchCourse(string name)
        // {
        //     string currentDir = Directory.GetCurrentDirectory();
        //     string filepath = Path.Combine(currentDir,  "Data", "courses.txt");
        //     string[] lines = File.ReadAllLines(filepath);
        //     foreach (string line in lines)
        //     {
        //         string[] data = line.Split(",");
        //         if (data[1] == name)
        //         {
        //             return new Courses(data[0], data[1], data[2], int.Parse(data[3]), int.Parse(data[4]));
        //         }
        //     }
        //     return null;
        // }

        // Search course by name and return the top 5 courses with like search name
        public List<Courses> SearchCourse(string name)
        {
            string currentDir = Directory.GetCurrentDirectory();
            string filepath = Path.Combine(currentDir,  "Data", "courses.txt");
            
            List<Courses> coursesList = new List<Courses>();

            foreach (string line in File.ReadLines(filepath))
            {
                string[] data = line.Split(",");
                Courses course = new Courses(data[0], data[1], data[2], int.Parse(data[3]), int.Parse(data[4]));
                
                coursesList.Add(course);
            }

            var query = (from course in coursesList
                         where course.Name.Contains(name)
                         select course).Take(5);
            return query.ToList();
        }
    }
}
