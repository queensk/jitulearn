namespace models
{
    public class Admin
    {
        private string name;
        private string password;
        private string email;
        private JituAnalytics jituAnalytics = JituAnalytics.GetInstance();

        // admin constructor

        public Admin(string name, string password, string email)
        {
            this.name = name;
            this.password = password;
            this.email = email;
        }

        // getters and setters
        public string Name { get => name; set => name = value; }

        public string Password { get => password; set => password = value; }

        public string Email { get => email; set => email = value; }
    }
}