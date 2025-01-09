using ConnectDB;

namespace BlazorApp1.Unit_Test
{
    public class Unit_Tests
    {

        public Unit_Tests()
        {

        }

        // function to test function of Connection.cs
        public bool Save_User_To_DB_Test()
        {
            try
            {
                Connection method_File = new Connection();

                var command = method_File._connection.CreateCommand();

                method_File.Save_User_To_DB("Testing", "test");
                
                command.CommandText = @"DELETE FROM Accounts WHERE Username = 'Test' OR Username = 'Testing';";
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fail: " + ex.Message);
                return false;
            }
            return true;
        }

        // function to test the Reading_Value function of Connection.cs
        public bool Reading_Values_Test()
        {
            try
            {
                Connection method_File = new Connection();

                List<string> test_result = method_File.Read_Values();
                Console.WriteLine(test_result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed because of: " + ex.Message);
                return false;
            }
            
            return true;
        }
    }
}
