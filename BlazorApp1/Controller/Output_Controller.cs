namespace BlazorApp1.Controller
{
    public class Output_Controller
    {
        // This code gives the output based on the result of the Login
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

        // This code will give the output based on the Register result
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
