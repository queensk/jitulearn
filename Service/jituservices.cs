using System;
using UserService.Models;
using AdminServices;
using Microsoft.Extensions.DependencyInjection;

namespace JituServices
{
    public class JituService
    {
        public static JituUserService userService = new JituUserService();
        public static AdminService adminService = new AdminService();
        private IConsole console;

        public JituService(IConsole console)
        {
            this.console = console;
        }

        public JituService()
        {
        }

        public void ShowServices()
        {
            Console.WriteLine("Select an option:");
            Console.WriteLine("1. Register");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Exit");
            string? option = Console.ReadLine();
            var isValid = ValidateOption(option, 1, 3);
            switch (option)
            {
                case "1":
                    // Get user information
                    Console.WriteLine("Enter your name");
                    string name = Console.ReadLine();
                    Console.WriteLine("Enter your email");
                    string email = Console.ReadLine();
                    Console.WriteLine("Enter your password");
                    string password = Console.ReadLine();
                    userService.registerUser(name, email, password);
                    Console.Clear();
                    // Console.WriteLine("Registration successful!");
                    break;
                case "2":
                    // Implement login logic here
                    // show login for user and admin
                    Console.Clear();
                    ShowLoginOptions();
                    break;
                case "3":
                    Console.WriteLine("Exiting...");
                    break;
                default:
                    ShowServices();
                    break;
            }
        }

        public int ValidateOption(string? option, int? start, int? end)
        {
            if (!int.TryParse(option, out int opt))
            {
                Console.WriteLine("Invalid option");
                return 0;
            }
            else if (opt < start || opt > end)
            {
                Console.WriteLine("Invalid option");
                return 0;
            }
            else
            {
                return 1;
            }
        }

        public string ValidateUserInput(string? input, string? message){
            // check if input is string and not null
            if (input == null)
            {
                Console.WriteLine($"{message}");
                return "";
            }else{
                // remove Whitespace
                input = input.Trim();
                return input;
            }
        }


        public void ShowLoginOptions(){
            Console.WriteLine("1. User login");
            Console.WriteLine("2. Admin login");
            string? option = Console.ReadLine();
            ValidateOption(option, 1, 2);
            switch (option)
            {
                case "1":
                    // Implement login logic here
                    loginUser();
                    break;
                case "2":
                    // Implement login logic here
                    loginAdmin();
                    break;
                default:
                    ShowLoginOptions();
                    break;
                
            }
        }

        public void loginUser(){
            Console.Clear();
            Console.WriteLine("Enter your name");
            string? name1 = Console.ReadLine();
            Console.WriteLine("Enter your password");
            string? password1 = Console.ReadLine();
            userService.LoginUser(name1, password1);
        }

        public void loginAdmin(){
            Console.Clear();
            Console.WriteLine("Enter your name");
            string? nameAdmin = Console.ReadLine();
            Console.WriteLine("Enter your password");
            string? passwordAdmin = Console.ReadLine();
            adminService.loginAdmin(nameAdmin, passwordAdmin);
        }

        public interface IConsole
        {
            void WriteLine(string message);
            string? ReadLine();
        }

        public class ConsoleWrapper : IConsole
        {
            public void WriteLine(string message)
            {
                Console.WriteLine(message);
            }

            public string? ReadLine(){
                return Console.ReadLine();
            }

            public static void ConsoleClear(){
                Console.Clear();
            }
        }
    }


    
}
