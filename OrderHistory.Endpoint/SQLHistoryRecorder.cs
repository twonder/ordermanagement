using BaseMessages.Events;
using NServiceBus;
using OrderEntry.Events;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;

namespace OrderHistory.Endpoint
{
    public class HistoryRecorder// : IHandleMessages<IOrderEvent>
    {
        public void Handle(IOrderEvent message)
        {
            var fullName = message.GetType().FullName.Replace("__impl", "");
            Console.WriteLine("Recording history: " + message.OrderId);
            Console.WriteLine(fullName);
            Console.WriteLine("----------------------------------------------");

            // define INSERT query with parameters
            string query = "INSERT INTO dbo.EventStream (OrderId, Type, Occurred, Data) " +
                           "VALUES (@OrderId, @Type, @Occurred, @Data) ";

            // create connection and command
            using (SqlConnection cn = new SqlConnection(@"data source=.\SQLEXPRESS;Database=OrderManagement.OrderHistory;Integrated Security=SSPI"))
            using (SqlCommand cmd = new SqlCommand(query, cn))
            {
                // define parameters and their values
                cmd.Parameters.Add("@OrderId", SqlDbType.VarChar, 50).Value = message.OrderId;
                cmd.Parameters.Add("@Type", SqlDbType.VarChar, 50).Value = fullName;
                cmd.Parameters.Add("@Occurred", SqlDbType.DateTime).Value = message.Occurred;
                cmd.Parameters.Add("@Data", SqlDbType.Text).Value = new JavaScriptSerializer().Serialize(message);

                // open connection, execute INSERT, close connection
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
    }
}
