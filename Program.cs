// See https://aka.ms/new-console-template for more information

using UserService.Models;
using JituServices;

Console.Clear();
Console.WriteLine("Welcome to  TheJitu learn");
JituService jituUserService = new JituService();

jituUserService.ShowServices();
