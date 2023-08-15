using JituServices;
using models;

namespace AdminServices
{
    public class AdminService{

        private static JituService jituService = new JituService();
        private JituAnalytics jituAnalytics = JituAnalytics.GetInstance();


        public void loginAdmin(string? username, string? password){
            string currentDir = Directory.GetCurrentDirectory();
            string filepath = Path.Combine(currentDir, "Data", "admin.txt");
            // create file if not exists
            if (!File.Exists(filepath))
            {
                File.Create(filepath).Close();
            }

            bool loginSuccess = false;
            using (FileStream fs = new FileStream(filepath, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    string? line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string?[] data = line.Split(',');
     
                        if (data[0] == username && data[2] == password)
                        {
                            loginSuccess = true;
                            break;
                        }
                    }
                }
            }

            if (loginSuccess)
            {
                Console.Clear();
                Console.WriteLine("Login Successful");
                showAdminOptions();
            }
            else
            {
                Console.WriteLine("Login Failed try again");
                jituService.loginAdmin();
            }
        }
        public void showAdminOptions()
        {
            Console.WriteLine("Select an option:");
            Console.WriteLine("1. View Courses");
            Console.WriteLine("2. Add a new Course");
            Console.WriteLine("3. Delete a Course");
            Console.WriteLine("4. Update a Course");
            Console.WriteLine("5. View Analytics");
            Console.WriteLine("6. Exit");
            string? option = Console.ReadLine();
            int adminActivityOption = jituService.ValidateOption(option, 1, 6);
            if (adminActivityOption == 1)
            {
                switch (option)
                {
                    case "1":
                        // jituService.viewCourses();
                        Console.Clear();
                        Console.WriteLine("Courses List");
                        List<Courses> allCourses = Courses.GetAll();
                        foreach (Courses course in allCourses)
                        {
                            course.Print();
                        }
                        Console.WriteLine("1. show other options");
                        string otherOption = Console.ReadLine();
                        int otherOptionInt = jituService.ValidateOption(otherOption, 1, 1);
                        if (otherOptionInt == 1)
                        {
                            showAdminOptions();
                        }
                        break;
                    case "2":
                        // jituService.addCourse();
                        Console.Clear();
                        Console.WriteLine("Enter course name");
                        string? courseName = Console.ReadLine();
                        Console.WriteLine("Enter course title");
                        string? title = Console.ReadLine();
                        Console.WriteLine("Enter course ratting (1-5)");
                        string? ratting = Console.ReadLine();
                        int rattingInt = jituService.ValidateOption(ratting, 1, 5);
                        Console.WriteLine("Enter course fee");
                        string courseFeeString = Console.ReadLine();
                        int courseFeeInt = jituService.ValidateOption(courseFeeString, 1, 100000);
                        if (rattingInt ==1 && courseFeeInt == 1)
                        {
                            int courseFee = Convert.ToInt32(courseFeeString);
                            int rattingValue = Convert.ToInt32(ratting);
                            addCourse(courseName, title, rattingValue, courseFee);
                            showAdminOptions();
                        }
                        break;
                    case "3":
                        // jituService.deleteCourse();
                        Console.Clear();
                        // list all courses and ask for course id to delete
                        Console.WriteLine("Courses List");
                        List<Courses> allCourses1 = Courses.GetAll();
                        foreach (Courses course in allCourses1)
                        {
                            course.Print();
                        }
                        Console.WriteLine("Enter course id to delete");
                        string? courseId = Console.ReadLine();
                        string? courseId1 = jituService.ValidateUserInput(courseId, "Enter valid course id");
                        Courses.Delete(courseId1);
                        if (jituAnalytics != null)
                        {
                            jituAnalytics.Courses--;
                            jituAnalytics.Save();
                        }
                        Console.WriteLine("Course deleted successfully");
                        Console.WriteLine("1. show other options");
                        string otherOption1 = Console.ReadLine();
                        int otherOptionInt1 = jituService.ValidateOption(otherOption1, 1, 1);
                        if (otherOptionInt1 == 1)
                        {
                            showAdminOptions();
                        }
                        break;
                    case "4":
                        // jituService.updateCourse();
                        Console.Clear();
                        Console.WriteLine("Courses List");
                        List<Courses> allCourses2 = Courses.GetAll();
                        foreach (Courses course in allCourses2)
                        {
                            course.Print();
                        }
                        Console.WriteLine("Enter course id to update");
                        string? courseId2 = Console.ReadLine(); 
                        string? courseId3 = jituService.ValidateUserInput(courseId2, "Enter valid course id");
                        Courses.GetById(courseId3).Print();
                        Console.WriteLine("Enter new course name");
                        string? courseName1 = Console.ReadLine();
                        Console.WriteLine("Enter new course title");
                        string? title1 = Console.ReadLine();
                        Console.WriteLine("Enter new course ratting (1-5)");
                        string? ratting1 = Console.ReadLine();
                        int rattingInt1 = jituService.ValidateOption(ratting1, 1, 5);
                        Console.WriteLine("Enter new course fee");
                        string courseFeeString1 = Console.ReadLine();
                        int courseFeeInt1 = jituService.ValidateOption(courseFeeString1, 1, 100000);
                        if (rattingInt1 == 1 && courseFeeInt1 == 1)
                        {
                            int courseFee = Convert.ToInt32(courseFeeString1);
                            int rattingValue = Convert.ToInt32(ratting1);
                            Courses.Update(courseId3, courseName1, title1, rattingValue, courseFee);
                            Console.WriteLine("Course updated successfully");
                            Console.WriteLine("1. show other options");
                            string otherOption2 = Console.ReadLine();
                            int otherOptionInt2 = jituService.ValidateOption(otherOption2, 1, 1);
                            if (otherOptionInt2 == 1)
                            {
                                showAdminOptions();
                            }
                        }
                        break;
                    case "5":
                        // jituService.viewAnalytics();
                        jituAnalytics.print();
                        Console.WriteLine("1. show other options");
                        string otherOption3 = Console.ReadLine();
                        int otherOptionInt3 = jituService.ValidateOption(otherOption3, 1, 1);
                        if (otherOptionInt3 == 1)
                        {
                            showAdminOptions();
                        }
                        else{
                            Console.WriteLine("invalid option");
                            showAdminOptions();
                        }
                        break;
                    case "6":
                        Console.WriteLine("Exiting");
                        Environment.Exit(0);
                        break;
                    default:
                        showAdminOptions();
                        break;
                }
            }
            
        }
        public void registerAdmin(string? userName, string userEmail, string? password){
            string currentDir = Directory.GetCurrentDirectory();
            string filepath = Path.Combine(currentDir, "Data", "admin.txt");
            // create file if not exists
            if (!File.Exists(filepath))
            {
                File.Create(filepath).Close();
            }

            using (FileStream fs = new FileStream(filepath, FileMode.Append))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(userName + "," + userEmail + "," + password);
                }
            }
        }

        public void showCoures(){
        }

        public void addCourse(string courseName, string title, int ratting, int courseFee){
            string courseId = Guid.NewGuid().ToString();
            Courses courses = new Courses(courseId, courseName, title, ratting, courseFee);
            courses.Save();
            courses.Print();
            if (jituAnalytics != null)
            {
                jituAnalytics.Courses++;
                jituAnalytics.Save();
            }
            Console.WriteLine("Course added successfully");
            // display the course details
        }



    }
}