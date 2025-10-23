using BlazorApp1.Controller;
using DuckDB.NET.Data;
using Hashing.src.Hasher;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace ConnectDB
{
    public class Connection
    {
        // adding objects for the 2FA
        private Dictionary<string, string> _verification_Codes = new Dictionary<string, string>();
        private Random _random = new Random();

        // creating objects for later use
        private readonly ICust _cust;
        public DuckDBConnection _connection;

        // giving the objects from above values
        public Connection(ICust cust)
        {
            _cust = cust;
            _connection = DB_Connection();
        }

        // This function opens the connection to the db
        public DuckDBConnection DB_Connection()
        {
            var Create_Connection = new DuckDBConnection("Data Source=TestDB.sql");
            Create_Connection.Open();

            // this code is only if the sql doesnt exist anymor
            
            using var command = Create_Connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Accounts (
                User_ID INTEGER PRIMARY KEY,
                UserEmail VARCHAR NOT NULL,
                Username VARCHAR NOT NULL, 
                HashedPassword VARCHAR NOT NULL, 
                Salt VARCHAR NOT NULL
            );";

            command.ExecuteNonQuery();
            
            return Create_Connection;
        }

        // this function saves the user to the db
        public bool Save_User_To_DB(string username, string password, string email)
        {
            try
            {
                if (_connection.State != System.Data.ConnectionState.Open)
                {
                    _connection.Open();
                }

                using var command = _connection.CreateCommand();

                // this is for incrementing the user_Id
                command.CommandText = "SELECT COALESCE(MAX(User_Id), 0) + 1 FROM Accounts";
                int newUserId = Convert.ToInt32(command.ExecuteScalar());

                // this is for creating the hash for the save later
                string salt = _cust.Salt(32);
                string hashed_Password = _cust.Hash(password, salt, 32);

                // this is for inserting the salt and hashed password to the db
                command.CommandText = "INSERT INTO Accounts (User_Id, UserEmail, Username, HashedPassword, Salt) VALUES ($user_Id,  $email, $username, $hashed_Password, $salt);";
                command.Parameters.Clear();
                command.Parameters.Add(new DuckDBParameter("user_Id", newUserId));
                command.Parameters.Add(new DuckDBParameter("email", email));
                command.Parameters.Add(new DuckDBParameter("username", username));
                command.Parameters.Add(new DuckDBParameter("hashed_Password", hashed_Password));
                command.Parameters.Add(new DuckDBParameter("salt", salt));

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Couldn't create Account:" + ex.Message);
                return false;
            }

            return true;
        }

        // this is the function to read the values from the db and give it out
        public List<string> Read_Values(int? user_Id = null, string? username = null)
        {
            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            var result = new List<string>();
            try
            {
                using var command = _connection.CreateCommand();

                // this is a query used to read the values
                command.CommandText = @"
                    SELECT 
                        *
                    FROM 
                        Accounts 
                    WHERE 
                        ($user_Id IS NULL OR User_ID = $user_Id) AND
                        ($username IS NULL OR Username = $username OR $username = '');";
                command.Parameters.Clear();
                command.Parameters.Add(new DuckDBParameter("user_Id", (object)user_Id ?? DBNull.Value));
                command.Parameters.Add(new DuckDBParameter("username", (object)username ?? DBNull.Value));

                // this is for the reading process and saving the result to a variable 
                using var reader = command.ExecuteReader();

                Console.WriteLine("Users in the database:");

                while (reader.Read())
                {
                    var line = new List<string>();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        line.Add($"{reader.GetName(i)}:     {reader.GetValue(i)}");
                    }

                    result.Add(string.Join(", ", line));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Processing Failed: {ex.Message}");
                return result;
            }

            return result;
        }

        public bool Try_Login(string username, string password, string verification_Code)
        {
            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            using var command = _connection.CreateCommand();

            // this is for inserting the salt and hashed password to the db
            command.CommandText = "SELECT HashedPassword FROM Accounts WHERE Username = $username;";
            command.Parameters.Clear();
            command.Parameters.Add(new DuckDBParameter("username", username));

            using var reader = command.ExecuteReader();
            if (!reader.Read())
            {
                return false;
            }

            // this will get the string which we do want
            string stored_Hash = reader.GetString(0);

            // this will give back if the password is the same as in the db
            if (_cust.Verify(password, stored_Hash))
            {
                if (string.IsNullOrEmpty(verification_Code))
                {
                    // Generate a unique verification code
                    string unique_Code = Generate_Unique_Code();
                    _verification_Codes[username] = unique_Code;

                    // Send the unique code to the user (implement your sending logic here)
                    Send_Verification_Code(username, unique_Code);

                    // Indicate that the user needs to provide the verification code
                    return false; // Indicate that 2FA is required
                }
                else
                {
                    // Check the provided verification code
                    if (_verification_Codes.TryGetValue(username, out string stored_Code) && stored_Code == verification_Code)
                    {
                        // 2FA successful, remove the code after successful verification
                        _verification_Codes.Remove(username);

                        return true; // Login successful
                    }
                    else
                    {
                        // 2FA failed
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        private string Generate_Unique_Code()
        {
            return _random.Next(100000, 999999).ToString();
        }

        private void Send_Verification_Code(string username, string unique_Code)
        {
            string userEmail = GetUser_Email(username);
            if (string.IsNullOrEmpty(userEmail))
            {
                Console.WriteLine($"No email found for user: {username}");
                return;
            }

            using (var client = new SmtpClient("smtp.your_email_provider", 587)) // Use your SMTP server and port
            {
                client.Credentials = new NetworkCredential("Enter_Your_Email", "Enter_your_password"); // Your email and password
                client.EnableSsl = true; // Enable SSL if required

                // Create the email message
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("Enter_your_Email_Adress"), // Your email address
                    Subject = "Your Verification Code",
                    Body = $"Your verification code is: {unique_Code}",
                    IsBodyHtml = false,
                };
                mailMessage.To.Add(userEmail);

                // Send the email
                try
                {
                    client.Send(mailMessage);
                    Console.WriteLine($"Verification code sent to {userEmail}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send verification code: {ex.Message}");
                }
            }
        }

        private string GetUser_Email(string username)
        {
            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            using var command = _connection.CreateCommand();
            command.CommandText = "SELECT UserEmail FROM Accounts WHERE Username = $username;";
            command.Parameters.Clear();
            command.Parameters.Add(new DuckDBParameter("username", username));

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return reader.GetString(0); // Return the user's email
            }

            return null; // No email found
        }
    }
}

