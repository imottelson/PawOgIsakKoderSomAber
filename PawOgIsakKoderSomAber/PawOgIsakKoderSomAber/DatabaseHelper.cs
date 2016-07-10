using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace PawOgIsakKoderSomAber
{
    public static class DatabaseHelper
    {
        public static void SaveNetworkToDatabase(NeuralNetwork network, int? epoch=null)
        {
            var sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString =
                @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Git repository\MachineLearning\PawOgIsakKoderSomAber\Database\Database\NetworkDatabase.mdf"";Integrated Security=True";
            sqlConnection.Open();


            string serializedNetwork = JsonConvert.SerializeObject(network);

            //var serializedNetwork = network.SerializeObject();
            SqlCommand cmd = new SqlCommand("INSERT INTO Network (Strain,timestamp,Network,Epoch) VALUES (@Strain,@timestamp,@Network,@Epoch)",sqlConnection);

            //cmd.Parameters.AddWithValue("ID", new Guid());
            cmd.Parameters.AddWithValue("Strain", network.uid);
            cmd.Parameters.AddWithValue("timestamp", DateTime.Now);
            cmd.Parameters.AddWithValue("Network", serializedNetwork);
            cmd.Parameters.AddWithValue("Epoch", epoch);

            cmd.ExecuteNonQuery();

            sqlConnection.Close();
        }



    }
}
