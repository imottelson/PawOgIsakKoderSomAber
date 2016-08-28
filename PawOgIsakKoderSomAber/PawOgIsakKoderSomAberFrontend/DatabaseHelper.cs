using System;
using System.Data.SqlClient;
using Newtonsoft.Json;
using PawOgIsakKoderSomAberFrontend.Models.NN;

namespace PawOgIsakKoderSomAberFrontend
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
