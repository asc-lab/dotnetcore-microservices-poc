using Dapper;
using PaymentService.Api.Queries.Dtos;
using PaymentService.Domain;
using PaymentService.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentService.Queries
{
    public class PolicyAccountQueries : IPolicyAccountQueries
    {
        private readonly string connectionString;

        public PolicyAccountQueries(string connectionString)
        {
            this.connectionString = !string.IsNullOrWhiteSpace(connectionString) ? connectionString : throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<IEnumerable<PolicyAccountDto>> FindAll()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var results = await connection.QueryAsync<PolicyAccount>(@"SELECT * FROM PolicyAccounts");

                if (results != null)
                {
                    return results.Select(x => new PolicyAccountDto
                    {
                        PolicyAccountNumber = x.PolicyAccountNumber,
                        PolicyNumber = x.PolicyNumber
                    });
                }

                return null;
            }
        }

        public async Task<PolicyAccountBalanceDto> FindByNumber(string accountNumber)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var sql = @"SELECT * FROM PolicyAccounts 
                            WHERE PolicyAccountNumber = @AccountNumber;
                            SELECT a.* FROM PolicyAccounts AS p
                            LEFT JOIN AccountingEntries AS a
                                ON p.Id = a.policyAccountId
                            WHERE p.PolicyAccountNumber = @AccountNumber";

                using (var multi = await connection.QueryMultipleAsync(sql, new { accountNumber }))
                {
                    var policyAccount = await multi.ReadFirstOrDefaultAsync<PolicyAccount>();
                    if (policyAccount == null)
                        return null;

                    multi.Read().ForEach(x =>
                    {
                        policyAccount.Entries.Add(AccountingMapper.ConvertToAccounting(x));
                    });

                    return new PolicyAccountBalanceDto
                    {
                        PolicyNumber = policyAccount.PolicyNumber,
                        PolicyAccountNumber = policyAccount.PolicyAccountNumber,
                        Balance = policyAccount.BalanceAt(DateTimeOffset.Now)
                    };
                }
            }
        }
    }
}
