using System;
using models;
using System.IO;
using JituServices;
using System.Diagnostics;

namespace UserService.Models
{
    public class JituUserService
    {
        private static JituService jituService = new JituService();
        private static User LogInUser;
        private JituAnalytics jituAnalytics = JituAnalytics.GetInstance();
        public void checkeUserExists(string username)
        {
            string currentDir = Directory.GetCurrentDirectory();
            string filepath = Path.Combine(currentDir, "Data", "user.txt");

            bool userExists = false;
            using (FileStream fs = new FileStream(filepath, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] data = line.Split(',');
                        if (data[0] == username)
                        {
                            userExists = true;
                            break;
                        }
                    }
                }
            }

            if (userExists)
            {
                Console.WriteLine("User Exists");
            }
            else
            {
                Console.WriteLine("User does not exist");
            }
        }


        public void registerUser(string? username, string? email, string? password)
        {
            string userId = Guid.NewGuid().ToString();
            User user = new(userId, username, email, password);
            user.Save();
            if (jituAnalytics != null)
            {
                jituAnalytics.Users++;
                jituAnalytics.Save();
            }
            // if success regretter take the user to log in
            Console.WriteLine("user Registered successful");
            Console.WriteLine("Please Login");
            Console.WriteLine("Enter Username");
            string? username1 = Console.ReadLine();
            Console.WriteLine("Enter Password");
            string? password1 = Console.ReadLine();
            // call login user method 
            LoginUser(username1, password1);    
        
        }
    public void LoginUser(string? username, string? password)
    {
        string currentDir = Directory.GetCurrentDirectory();
        string filepath = Path.Combine(currentDir, "Data", "user.txt");

        bool loginSuccess = false;
        string[] lines = File.ReadAllLines(filepath);

        foreach (string line in lines)
        {
            string[] data = line.Split(';');
            if (data.Length > 0)
            {
                string[] userData = data[0].Split(',');

                if (userData.Length >= 5 && userData[1] == username && userData[3] == password)
                {
                    List<Courses> userCourses = new List<Courses>();

                    if (data.Length > 1 && !string.IsNullOrEmpty(data[1]))
                    {
                        string[] courseIds = data[1].Split(',');
                        foreach (string courseId in courseIds)
                        {
                            userCourses.Add(Courses.GetById(courseId));
                        }
                    }
                    LogInUser = new User(userData[0], userData[1], userData[2], userData[3], int.Parse(userData[4]), userCourses);
                    Console.Clear();
                    Console.WriteLine("Login Successful");
                    showUserMenu();
                    loginSuccess = true;
                    break;
                }
            }
        }

        if (!loginSuccess)
        {
            Console.Clear();
            Console.WriteLine("Login Failed");
            jituService.ShowServices();
        }
    }




        public void showUserMenu()
        {
            Console.WriteLine("1. View All courses");
            Console.WriteLine("2. Purchased Courses");
            Console.WriteLine("3. Logout");
            string? userChoice = Console.ReadLine();
            int userChoiceValid = jituService.ValidateOption(userChoice, 1, 3);
            if (userChoiceValid == 1)
            {
                switch(userChoice)
                {
                    case "1":
                        // show all courses
                        Console.Clear();
                        showAllCourses();
                        break;
                    case "2":
                        // show purchased courses
                        showUsersPurchasedCourses();
                        break;
                    case "3":
                        // logout
                        Console.Clear();
                        logout();
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid Option");
                        showUserMenu();
                        break;
                }
            }
            else{
                Console.Clear();
                Console.WriteLine("Invalid Option");
                showUserMenu();
            }

        }
        public void showAllCourses()
        {
            Console.Clear();
            Console.WriteLine("All Courses");
            List<Courses> allCourses = Courses.GetAll();
            for (int i = 0; i < allCourses.Count; i++)
            {   
                Console.WriteLine($"{i + 1}. Name: {allCourses[i].Name}, Amount: {allCourses[i].Amount}, Rating: {allCourses[i].Rating}");
            }
            Console.WriteLine($"Enter Course from 1 to {allCourses.Count}");
            string? courseChoice = Console.ReadLine();
            int courseChoiceValid = jituService.ValidateOption(courseChoice, 1, allCourses.Count);
            if (courseChoiceValid == 1)
            {
                // show course purchase menu
                // show selected course details
                allCourses[int.Parse(courseChoice) - 1].Print();
                Console.WriteLine("Do you want to purchase this course?");
                Console.WriteLine("1. Yes");
                Console.WriteLine("2. No");
                // if yes check the user balance if bala is less deposit until the blanca is enough
                while(LogInUser.Credit < allCourses[int.Parse(courseChoice) - 1].Amount)
                {
                    deposit();
                }
                string? purchaseChoice = Console.ReadLine();
                int purchaseChoiceValid = jituService.ValidateOption(purchaseChoice, 1, 2);
                if (purchaseChoiceValid == 1)
                {
                    showPurchasedCourses(allCourses, courseChoice);
                }else{
                    Console.WriteLine("invalid options");
                    showAllCourses();
                }

            }
        }

        public void showPurchasedCourses(List<Courses> allCourses, string courseChoice)
        {
            // Check user balance
            Courses selectedCourse = allCourses[int.Parse(courseChoice) - 1];
            if (LogInUser.Credit >= selectedCourse.Amount)
            {
                // purchase course
                LogInUser.Credit -= selectedCourse.Amount;
                Console.WriteLine(selectedCourse);
                LogInUser.Courses.Add(selectedCourse);
                LogInUser.Save();
                if (jituAnalytics != null)
                {
                    jituAnalytics.Purchased++;
                    jituAnalytics.Save();
                }
                LogInUser.PrintFullUser();
                Console.WriteLine("Course Purchased");
                Console.WriteLine("Your balance is: " + LogInUser.Credit);
                // purchaseCourse();
            } else{
                // deposit
                showDepositMenu();
            }
        }

        public void showPurchasedCourses(User loggedInUser)
        {
            Console.WriteLine("Purchased Courses:");
            foreach (Courses course in loggedInUser.Courses)
            {
                Console.WriteLine($"Name: {course.Name}, Amount: {course.Amount}, Rating: {course.Rating}");
            }
        }

        public void showDepositMenu()
        {
            Console.WriteLine("1. Deposit");
            Console.WriteLine("2. Check Balance");
            Console.WriteLine("3. Logout");
            string? depositChoice = Console.ReadLine();
            int depositChoiceValid = jituService.ValidateOption(depositChoice, 1, 2);
            if (depositChoiceValid == 1)
            {
                // deposit
                switch(depositChoice)
                {
                    case "1":
                        deposit();

                        break;
                    case "2":
                        // check balance
                        Console.WriteLine("Your balance is: " + LogInUser.Credit);
                        LogInUser.print();
                        showDepositMenu();
                        break;
                    case "3":
                        // logout
                        logout();
                        break;
                    default:
                        Console.WriteLine("Invalid Choice");
                        showDepositMenu();
                        break;
                }
                
            }
            else
            {
                // logout
                logout();
            }
        }

        public void logout()
        {
            Console.WriteLine("Logout Successful");
            LogInUser = null;
            Environment.Exit(0);

        }
        public void deposit()
        {
            Console.WriteLine("Enter Amount");
            string depositAmount = Console.ReadLine();
            int ValidateInput = jituService.ValidateOption(depositAmount, 1, int.MaxValue);
            if (ValidateInput == 1)
            {
                int depositAmountInt = int.Parse(depositAmount);
                LogInUser.Credit += depositAmountInt;
                LogInUser.Save();
                LogInUser.PrintFullUser();
                Console.WriteLine("Your balance is: " + LogInUser.Credit);
            }
            else{
                showDepositMenu();
            }
        }
        // public void purchaseCourse()
        // {
        //     // deduct user balance from course amount
        //     // add course to user purchased courses
        //     // save user data
        //     // show user purchased courses
        //     LogInUser.Credit -= LogInUser.Courses[0].Amount;
        //     LogInUser.Courses.Add(LogInUser.Courses[0]);

        // }

        public void showUsersPurchasedCourses()
        {
            Console.Clear();
            LogInUser.PrintFullUser();
            Console.WriteLine("1. Go back to the previous menu");
            string? choice = Console.ReadLine();
            int choiceValid = jituService.ValidateOption(choice, 1, 1);
            if (choiceValid == 1)
            {
                showUserMenu();
            }else{
             showUsersPurchasedCourses();
            }

        }

    }
}