using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbOrdini
{
    public class User
    {
        public string username { get; set; }
        public string password { get; set; }

        public static void Login(string a, string b, SqlConnection c)
        {
            using (c)
            {
                //login 
                Console.WriteLine($"connessione = {c}");
                c.Open();
                Console.WriteLine("connessione aperta");
                Console.WriteLine("effettua il login per accedere al db");
                
                var cmd = new SqlCommand($"select * from users where username=@username and password=@password", c);
                cmd.Parameters.Add(new SqlParameter("username", a));
                cmd.Parameters.Add(new SqlParameter("password", b));
            }
            Console.ReadLine();



        }

        public static void CreaUtente(string a, string b, SqlConnection c)
        {
            using (c)
            {
                Console.WriteLine($"connessione = {c}");
                c.Open();
                Console.WriteLine("connessione aperta");
                var cmd = new SqlCommand($"select * from users where username=@username and password=@password", c);
                //creazione utente 
                Console.WriteLine("crea nuovo utente");
                cmd = new SqlCommand($"insert into utenti values ({a}, {b})", c);

                cmd.Parameters.Add(a);
                cmd.Parameters.Add(b);
            }
            Console.ReadLine();

        }

        public static void ListaOrdini(string a, string b, SqlConnection c)
        {
            Console.WriteLine($"connessione = {c}");
            c.Open();
            Console.WriteLine("connessione aperta");

            var cmd = new SqlCommand("select * from orders");

            using (var orders = cmd.ExecuteReader())

            {
                while (orders.Read())
                {
                    Console.WriteLine("{0} {1}", orders["orderid"], orders["costumer"]);
                }
            }
            Console.ReadLine();

        }

        public static void DettagliOrdine(string a, string b, SqlConnection c)
        {
            using (c)
            {
                string a = ("select count(*) from orderitems");
                var cmd = new SqlCommand(a, c);
                Console.WriteLine($"ci sono {a} righe di ordini ");
                cmd = new SqlCommand("select * from orderitems", c);
                using (var orderitems = cmd.ExecuteReader())

                {
                    while (orderitems.Read())
                    {
                        Console.WriteLine("{0} {1}", orderitems["orderid"], orderitems["item"], orderitems["qty"], orderitems["price"]);
                    }
                }
                Console.ReadLine();



            }
        }

    }


    internal class Program
    {
        static void Main(string[] args)
        {
            string connStr = "data source=.\\SQLEXPRESS; initial catalog=orders; User ID=sa; Password=sa;";
            SqlConnection con = new SqlConnection(connStr);
            Console.WriteLine($"connessione = {con}");
            con.Open();
            Console.WriteLine("connessione aperta");
            string username = Console.ReadLine();
            string password = Console.ReadLine();
            User.Login(username, password, con);
            User.CreaUtente(username, password, con);
            User.ListaOrdini(username, password, con);
            User.DettagliOrdine(username, password, con);
       
            
            Console.ReadLine();
            



        }
    }
}
