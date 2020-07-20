using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using terminus.shared.models;

namespace terminus.dataaccess
{
    public class TrialBalanceServices : ITrialBalanceServices
    {

        //Database Connection

        private readonly SQLConnectionConfiguration _configuration;
        public TrialBalanceServices(SQLConnectionConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task<bool> TrialBalanceInsert(rptTrialBalanceVM TrialBalance)

        {
            using (var conn = new SqlConnection(_configuration.Value))

            {
                var parameters = new DynamicParameters();
                parameters.Add("accountDesc", TrialBalance, System.Data.DbType.String);
                parameters.Add("Amount", TrialBalance, System.Data.DbType.Decimal);
                //Ras SQL method.
                const string query = @"INSERT INTO TrialBalance (accountDesc,Amount) VALUES(@accountDesc,@Amount)";
                await conn.ExecuteAsync(query, new { TrialBalance.accountDesc, TrialBalance .amount}, commandType: System.Data.CommandType.Text);

            }
            return true;
        }

        public async Task<IEnumerable<rptTrialBalanceVM>> TrialBalanceList()
        {
            IEnumerable<rptTrialBalanceVM> TrialBalance;
            using (var conn = new SqlConnection(_configuration.Value))
            {
                TrialBalance = await conn.QueryAsync<rptTrialBalanceVM>("rptTrialBalance", commandType: System.Data.CommandType.StoredProcedure);

            }
            return TrialBalance;
        }
    }

}
