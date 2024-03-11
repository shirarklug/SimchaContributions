using System.Data.SqlClient;

namespace GiftContributions.Data
{
    public class Simcha
    {
        public int SimchaId { get; set; }
        public string SimchaName { get; set; }
        public DateTime SimchaDate { get; set; }
        public decimal Total { get; set; }
    }

    public class Contributor
    {
        public int ContributorId { get; set; }
        public string ContributorFirstName { get; set; }
        public string ContributorLastName { get; set; }
        public string ContributorNumber { get; set; }
        public bool AlwaysInclude { get; set; }
        public decimal Total { get; set; }
    }

    public class Deposit
    {
        public int ContributorId { get; set; }
        public decimal DepositAmount { get; set; }
        public DateTime DepositDate { get; set; }
        public string Description { get; set; }
    }

    public class ContributionInclusion
    {
        public bool Include { get; set; }
        public decimal Amount { get; set; }
        public int ContributorId { get; set; }
    }

    public class GiftContributionsManager
    {
        private string _connectionString;

        public GiftContributionsManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Simcha> GetSimchos()
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Simchos";
            connection.Open();
            List<Simcha> simchos = new();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int simchaId = (int)reader["simchaId"];
                simchos.Add(new Simcha
                {
                    SimchaId = (int)reader["SimchaId"],
                    SimchaName = (string)reader["SimchaName"],
                    SimchaDate = (DateTime)reader["SimchaDate"],
                    Total = GetDepositSumForSimcha(simchaId)
                });
            }
            return simchos;
        }

        public void AddSimcha(string simchaName, DateTime simchaDate)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Simchos(SimchaName, SimchaDate) VALUES (@simchaName, @simchaDate)";
            cmd.Parameters.AddWithValue("@simchaName", simchaName);
            cmd.Parameters.AddWithValue("@simchaDate", simchaDate);
            connection.Open();
            cmd.ExecuteNonQuery();
        }

        public List<Contributor> GetContributors()
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Contributors";
            connection.Open();
            List<Contributor> contributors = new();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int contributorId = (int)reader["contributorId"];
                contributors.Add(new Contributor
                {
                    ContributorId = (int)reader["ContributorId"],
                    ContributorFirstName = (string)reader["ContributorFirstName"],
                    ContributorLastName = (string)reader["ContributorLastName"],
                    ContributorNumber = (string)reader["ContributorNumber"],
                    AlwaysInclude = (bool)reader["AlwaysInclude"],
                    Total = GetDepositSumForContributor(contributorId)
                });
            }

            return contributors;
        }

        public List<Deposit> GetDeposits(int contributorId)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Deposits WHERE ContributorId = @contributorId";
            cmd.Parameters.AddWithValue("@contributorId", contributorId);
            connection.Open();
            List<Deposit> deposits = new();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                deposits.Add(new Deposit
                {
                    ContributorId = (int)reader["ContributorId"],
                    DepositAmount = (decimal)reader["DepositAmount"],
                    DepositDate = (DateTime)reader["DepositDate"],
                    Description = (string)reader["Description"]
                });
            }
            return deposits;
        }

        public decimal GetDepositSumForSimcha(int simchaId)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT SUM(Amount) FROM [Contributor to Simcha] WHERE SimchaId = @simchaId";
            cmd.Parameters.AddWithValue("simchaId", simchaId);
            connection.Open();
            return (decimal)cmd.ExecuteScalar();
        }

        public decimal GetDepositSum()
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT SUM(DepositAmount) FROM Deposits";
            connection.Open();
            return (decimal)cmd.ExecuteScalar();
        }

        public decimal GetDepositSumForContributor(int contributorId)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT SUM(DepositAmount) FROM Deposits WHERE ContributorId = @contributorId";
            cmd.Parameters.AddWithValue("@contributorId", contributorId);
            connection.Open();
            return (decimal)cmd.ExecuteScalar();
        }

        public void AddContributor(Contributor c)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO Contributors(contributorFirstName, contributorLastName, contributorNumber, alwaysInclude) 
                               VALUES (@contributorFirstName, @contributorLastName, @contributorNumber, @alwaysInclude)";
            cmd.Parameters.AddWithValue("@contributorFirstName", c.ContributorFirstName);
            cmd.Parameters.AddWithValue("@contributorLastName", c.ContributorLastName);
            cmd.Parameters.AddWithValue("@contributorNumber", c.ContributorNumber);
            cmd.Parameters.AddWithValue("@alwaysInclude", c.AlwaysInclude);
            connection.Open();
            cmd.ExecuteNonQuery();
        }

        public void AddDeposit(Deposit d)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO Deposits(ContributorId, DepositAmount, DepositDate, Description)
                                VALUES(@ContibutorId, @DepositAmount, @DepositDate, @Description)";
            cmd.Parameters.AddWithValue("@ContibutorId", d.ContributorId);
            cmd.Parameters.AddWithValue("@DepositAmount", d.DepositAmount);
            cmd.Parameters.AddWithValue("@DepositDate", d.DepositDate);
            cmd.Parameters.AddWithValue("@Description", d.Description);
            connection.Open();
            cmd.ExecuteNonQuery();
        }

        public void AddContribution(decimal amount, int contributorId, int simchaId)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO [Contributor to Simcha](SimchaId, ContributorId, Amount)
                                VALUES(@SimchaId, @ContributorId, @Amount)";
            cmd.Parameters.AddWithValue("SimchaId", simchaId);
            cmd.Parameters.AddWithValue("@ContributorId", contributorId);
            cmd.Parameters.AddWithValue("@Amount", amount);
            connection.Open();
            cmd.ExecuteNonQuery();
            
        }

        public void MinusFromContributor(int contributorId, decimal amount, DateTime date)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO Deposits(ContributorId, DepositAmount, DepositDate, Description)
                            VALUES(@contributorId, @amount, @date, @description)";
            cmd.Parameters.AddWithValue("@contributorId", contributorId);
            cmd.Parameters.AddWithValue("@amount", -amount);
            cmd.Parameters.AddWithValue("@date", date);
            cmd.Parameters.AddWithValue("@description", "Contribution");
            connection.Open();
            cmd.ExecuteNonQuery();
        }

        public void DeleteContributions(int simchaId)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM [Contributor to Simcha] WHERE SimchaId = @simchaId";
            cmd.Parameters.AddWithValue("@simchaId", simchaId);
            connection.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
