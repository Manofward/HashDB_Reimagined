using System.Reflection.PortableExecutable;

namespace BlazorApp1.Controller
{
    public class Output_Controller
    {
        public string Login_Output(bool result)
        {
            if (result)
            {
                return "You are now Logged in.";
            }
            else
            {
                return "Username or Password is Wrong.";
            }
        }

        public string Logout_Output()
        {
            return "You are now Logged out.";
        }

    }
}
