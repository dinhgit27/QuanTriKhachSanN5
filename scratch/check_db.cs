using System;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json");
var config = builder.Build();
string connectionString = config.GetConnectionString("DefaultConnection");

using (var conn = new SqlConnection(connectionString))
{
    conn.Open();
    var cmd = new SqlCommand("SELECT name FROM sys.tables", conn);
    using (var reader = cmd.ExecuteReader())
    {
        Console.WriteLine("Tables in Database:");
        while (reader.Read())
        {
            Console.WriteLine("- " + reader["name"]);
        }
    }
}
