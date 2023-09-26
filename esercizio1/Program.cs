using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;

namespace esercizio1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("hello world");
            string connStr = "data source=.\\SQLEXPRESS; initial catalog=orders; User ID=sa; Password=sa;";
            SqlConnection con = new SqlConnection(connStr);
            using (con)
            {

                Console.WriteLine($"connessione = {con}");
                con.Open();
                Console.WriteLine("connessione aperta");
                string q = "select count(*) from orders";
                var cmd = new SqlCommand(q, con);
                var n = cmd.ExecuteScalar();
                Console.WriteLine($"ci sono {n} ordini");

                cmd = new SqlCommand("select * from orders", con);
                using (var orders = cmd.ExecuteReader())

                {
                    while (orders.Read())
                    {
                        Console.WriteLine("{0} {1}", orders["orderid"], orders["customer"]);
                    }
                }
                //ricerca utenti tramite injection
                Console.WriteLine("digita nome utente");
                string user = Console.ReadLine();
                cmd = new SqlCommand($"select * from orders where customer = '{user}'", con);
                SqlParameter par = new SqlParameter("@user", SqlDbType.NVarChar, 50);
                Console.WriteLine("con parametro");
                cmd.Parameters.Add(par);
                par.Value = user;
                using (var orders = cmd.ExecuteReader())
                {
                    while (orders.Read())
                    {
                        Console.WriteLine("->{0} {1}", orders["orderid"], orders["customer"]);
                    }
                }
                //modifica dati del prezzo di un certo tipo di utenti
                SqlTransaction tr = null;
                try
                {
                tr = con.BeginTransaction(); //transazione 
                Console.WriteLine("UPDATE");
                cmd = new SqlCommand($"update orderitems set price=price+100 where orderid=@order", con, tr);
                cmd.Parameters.Add(new SqlParameter("@order", 1));
                Console.WriteLine($"ho modificato {cmd.ExecuteNonQuery()} righe");
                tr.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    tr.Rollback();
                }


                Console.WriteLine("UPDATE");
                cmd = new SqlCommand($"update orderitems set price=price+100 where orderid=@order", con);
                cmd.Parameters.Add(new SqlParameter("@order", 1));
                Console.WriteLine($"ho modificato {cmd.ExecuteNonQuery()} righe");
                //ADAPTER
                SqlDataAdapter adapter = new SqlDataAdapter("select * from customers", con);
                DataSet model = new DataSet();
                adapter.Fill(model, "customers");
                Console.WriteLine("carico il dataset del customer");
                foreach (DataRow c in model.Tables["customers"].Rows)
                {
                    Console.WriteLine("{0} {1}", c["customer"], c["country"]);
                }
            }
            Console.ReadLine();
            
        }
    }
}
