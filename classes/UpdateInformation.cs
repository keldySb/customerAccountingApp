using finalExam_diplom_.classes.databaseTables;
using Npgsql;

namespace finalExam_diplom_.classes
{
    public class UpdateInformation
    {
        public void UpdateClientRequest(ClientsRequestTable ClientData)
        {
            DataBaseConnection connection = new DataBaseConnection();

            using (NpgsqlConnection con = connection.getConnection())
            {
                con.Open();

                string sql = $"UPDATE clients_requests SET status_id = @status_id, priority_id = @priority_id, manager_id = @manager_id, master_id = @master_id, visit_time = @visit_time WHERE id = @id";

                using (NpgsqlCommand command = new NpgsqlCommand(sql, con))
                {
                    command.Parameters.AddWithValue("@id", ClientData.Id);
                    command.Parameters.AddWithValue("@status_id", ClientData.Status_id);
                    command.Parameters.AddWithValue("@priority_id", ClientData.Priority_id);
                    command.Parameters.AddWithValue("@manager_id", ClientData.Manager_id);
                    command.Parameters.AddWithValue("@master_id", ClientData.Master_id);
                    command.Parameters.AddWithValue("@visit_time", ClientData.Visit_time);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateEmployeeRequest(EmployeesTable EmpData)
        {
            DataBaseConnection connection = new DataBaseConnection();

            using (NpgsqlConnection con = connection.getConnection())
            {
                con.Open();

                string sql = $"UPDATE employees SET status_id = @status_id WHERE id = @id";

                using (NpgsqlCommand command = new NpgsqlCommand(sql, con))
                {
                    command.Parameters.AddWithValue("@id", EmpData.Id);
                    command.Parameters.AddWithValue("@status_id", EmpData.Status_Id);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
