using finalExam_diplom_.classes.databaseTables;
using Npgsql;

namespace finalExam_diplom_.classes
{
    public class GetClientsRequestsTable()
    {
        public List<ClientsRequestTable> GetClientsRequests(string? search, int? statusId)
        {
            List<ClientsRequestTable> tableData = new List<ClientsRequestTable>();

            DataBaseConnection connection = new DataBaseConnection();

            using (NpgsqlConnection con = connection.getConnection())
            {
                con.Open();

                string sql = "SELECT cr.id, " +
                                        "CONCAT(c.first_name, ' ', c.last_name, ' ', c.middle_name) as fio," +
                                        "CONCAT(a.street, ', ', a.house_number, ', ', a.number_apartment) as address," +
                                        "cr.description, cr.status_id, s.name as status, cr.priority_id, p.name as priority, " +
                                        "cr.manager_id, d.employee_id as manager, " +
                                        "cr.master_id, CONCAT(e.first_name, ' ', e.last_name, ' ', e.middle_name) as master, cr.created_time, " +
                                        "cr.visit_time, cr.closed_time, cr.master_comment " +
                                        "FROM clients_requests cr " +
                                "LEFT JOIN clients c ON cr.client_id = c.id " +
                                "LEFT JOIN addresses a ON cr.apartment_id = a.id " +
                                "LEFT JOIN statuses s ON cr.status_id = s.id " +
                                "LEFT JOIN priorities p ON cr.priority_id = p.id " +
                                "LEFT JOIN employees e ON cr.master_id = e.id " +
                                "LEFT JOIN dispatchers d ON e.id = d.id";

                if (statusId.HasValue)
                {
                    sql += " WHERE cr.status_id = @status_id";
                }

                if (!string.IsNullOrEmpty(search))
                {
                    sql += " AND CONCAT(c.first_name, c.last_name, c.middle_name, a.street, a.house_number, a.number_apartment, cr.description, p.name) ILIKE @search";
                }

                using (NpgsqlCommand command = new NpgsqlCommand(sql, con))
                {
                    command.Parameters.AddWithValue("@search", $"%{search}%");
                    command.Parameters.AddWithValue("@status_id", statusId.Value);

                    NpgsqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        tableData.Add(new ClientsRequestTable(
                            reader.GetInt32(0),
                            reader[1].ToString(),
                            reader[2].ToString(),
                            reader[3].ToString(),
                            reader.GetInt32(4),
                            reader[5].ToString(),
                            reader.GetInt32(6),
                            reader[7].ToString(),
                            reader.IsDBNull(8) ? null : reader.GetInt32(8),
                            reader[9].ToString(),
                            reader.IsDBNull(10) ? null : reader.GetInt32(10),
                            reader[11].ToString(),
                            reader.IsDBNull(12) ? null : reader.GetDateTime(12),
                            reader.IsDBNull(13) ? null : reader.GetDateTime(13),
                            reader.IsDBNull(14) ? null : reader.GetDateTime(14),
                            reader[15].ToString()
                        ));
                    }

                    return tableData;
                }

            }
        }
    }

    public class GetEmployeeTable()
    {
        public List<EmployeesTable> GetEmployee (string? search) 
        {
            List<EmployeesTable> tableData = new List<EmployeesTable>();

            DataBaseConnection connection = new DataBaseConnection();

            using (NpgsqlConnection con = connection.getConnection())
            {
                con.Open();

                string sql = "SELECT " +
                                  "e.id, e.employee_number, CONCAT(e.first_name, ' ', e.last_name, ' ', e.middle_name) as fio, " +
                                  "e.age, e.position_id, p.name as position, e.experience, e.phone_number, e.status_id, s.name as status " +
                                "FROM employees e" +
                                " LEFT JOIN employees_status s ON e.status_id = s.id" +
                                " LEFT JOIN positions p ON e.position_id = p.id";

                if (!string.IsNullOrEmpty(search))
                {
                    sql += " WHERE CONCAT(e.employee_number, e.first_name, e.last_name, e.middle_name, p.name, s.name) ILIKE @search";
                }

                using (NpgsqlCommand command = new NpgsqlCommand(sql, con))
                {
                    command.Parameters.AddWithValue("@search", $"%{search}%");

                    NpgsqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        tableData.Add(new EmployeesTable(
                            reader.GetInt32(0),
                            reader[1].ToString(),
                            reader[2].ToString(),
                            reader.GetInt32(3),
                            reader.GetInt32(4),
                            reader[5].ToString(),
                            reader.GetInt32(6),
                            reader[7].ToString(),
                            reader.GetInt32(8),
                            reader[9].ToString()));
                    }
                    return tableData;
                }
            }
        }
    };

    public class GetStatuses()
    {
        public List<DictionaryItems> GetStatus()
        {
            return GetItems("SELECT id, name FROM statuses");
        }

        public List<DictionaryItems> GetPriority()
        {
            return GetItems("SELECT id, name FROM priorities");
        }

        public List<DictionaryItems> GetMaster()
        {
            return GetItems("SELECT id, CONCAT(first_name, ' ', last_name, ' ', middle_name) as name FROM employees" +
                " WHERE NOT position_id = 2 AND status_id = 1");
        }

        public List<DictionaryItems> GetManager()
        {
            return GetItems("SELECT d.id," +
                                    " CONCAT(e.first_name, ' ', e.last_name, ' ', e.middle_name) as name" +
                                    " FROM dispatchers d" +
                                    " LEFT JOIN employees e ON d.employee_id = e.id");
        }

        public List<DictionaryItems> GetItems(string sql)
        {
            List<DictionaryItems> tableData = new List<DictionaryItems>();

            DataBaseConnection connection = new DataBaseConnection();

            using (NpgsqlConnection con = connection.getConnection())
            {
                con.Open();

                using (NpgsqlCommand command = new NpgsqlCommand(sql, con))
                {

                    NpgsqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        tableData.Add(new DictionaryItems(
                            reader.GetInt32(0),
                            reader[1].ToString()));
                    }

                    return tableData;
                }
            }
        }
    }
}
