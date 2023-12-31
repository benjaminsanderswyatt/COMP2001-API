using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Coursework2001.Models
{
    public class Authenticate
    {
        private static string connectionString = "Data Source=dist-6-505.uopnet.plymouth.ac.uk;Initial Catalog=COMP2001_BSanderswyatt;User ID=BSanderswyatt;Password=HmoN123*;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public static string CreateProfile(CreateUser user)
        {
            return "";
        }
            
    }

}