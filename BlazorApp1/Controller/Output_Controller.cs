namespace BlazorApp1.Controller
{
    public class Output_Controller
    {
        public string Login_Output(bool result)
        {
            if (result)
            {
                return "You Are now Logged in.";
            }
            else
            {
                return "Username or Password is wrong.";
            }
        }

        public string Register_Output(bool result)
        {
            if (result)
            {
                return "Registration success.";
            }
            else
            {
                return "Something is not quiet right.";
            }
        }
    }
}
