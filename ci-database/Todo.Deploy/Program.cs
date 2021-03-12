using DbUp;
using Npgsql;
using System;

namespace Todo.Deploy
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting the CI to deploy DB changes");
            // This will load the content of .env file and create related environment variables
            DotNetEnv.Env.Load();
            Console.WriteLine(Environment.CurrentDirectory);
            // Connection string for deploying the database (high-privileged account as it needs to be able to CREATE/ALTER/DROP)
            var connectionString = Environment.GetEnvironmentVariable("ConnectionString");
            //var connectionString = "Host=localhost;Username=postgres;Password=Passw0rd;Database=todo;";

            // Password for the user that will be used by the API to connect to Azure SQL (low-privileged account)
            var backEndUserPassword = Environment.GetEnvironmentVariable("BackEndUserPassword");

            var csb = new  NpgsqlConnectionStringBuilder(connectionString);
            Console.WriteLine($"Deploying database: {csb.Database}");

            Console.WriteLine("Testing connection...");
            var conn = new NpgsqlConnection(csb.ToString());
            conn.Open();
            conn.Close();

            Console.WriteLine("Starting deployment...");
            var dbup = DeployChanges.To
                .PostgresqlDatabase(csb.ConnectionString)
                .WithScriptsFromFileSystem("./sql")
                .JournalToPostgresqlTable("public", "$__dbup_journal")
                .WithVariable("BackEndUserPassword", backEndUserPassword)
                .LogToConsole()
                .Build();


            var result = dbup.PerformUpgrade();

            if (!result.Successful)
            {
                Console.WriteLine(result.Error);
                //Add own 5000 error code to exit
                Environment.Exit(5000);
            }

            Console.WriteLine("Success!");
            Environment.Exit(-1);
        }
    }
}
