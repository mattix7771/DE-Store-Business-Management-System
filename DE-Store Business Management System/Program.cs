using DataAccessLayer;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DE_Store_Business_Management_System
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to DE-Store Business Management System");
            Console.WriteLine("Here's a list of commands you can use");
            Console.WriteLine("");

            DataAccessLayer.Database x = new DataAccessLayer.Database();
            x.DatabaseInitialisationAsync();

            string input = Console.ReadLine();

            //switch (input)
            //{
            //    case ""
            //}
        }
    }
}
