using Microsoft.Extensions.Logging;
using PaymentService.Api.Exceptions;
using PaymentService.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PaymentService.Domain
{
    public class BankStatementFile
    {
        public BankStatementFile(string path, DateTimeOffset importDate)
        {
            FilePath = path ?? throw new ArgumentNullException(nameof(path));
            FileName = ConstructFileNameFromDate(importDate);
        }

        private string FilePath { get; }

        private string FileName { get; }

        private string FullPath => Path.Combine(FilePath, FileName);

        private string ProcessedFullPath => Path.Combine(FilePath, $"_processed_{FileName}");

        public bool Exists()
        {
            return File.Exists(FullPath);
        }

        public List<BankStatement> Read()
        {
            try {
                return File.ReadAllLines(FullPath)
                                        .Skip(1) // skip header column
                                        .Select(ReadRow)
                                        .ToList();
            } 
            catch (FileNotFoundException ex)
            {
                throw new BankStatementsFileNotFound(ex);
            }
            catch (IOException ex)
            {    
                throw new BankStatementsFileReadingError(ex);
            }
        }

        public void MarkProcessed()
        {
            File.Copy(FullPath, ProcessedFullPath);
        }

        private BankStatement ReadRow(string row)
        {
            var cells = row.Split(",");
            string accountingDate = cells[2];
            string accountNumber = cells[3];
            string amountAsString = cells[4];
            return new BankStatement(accountNumber, amountAsString, accountingDate);
        }

        private string ConstructFileNameFromDate(DateTimeOffset importDate)
        {
            return $"bankStatements_{importDate.Year}_{importDate.Month}_{importDate.Day}.csv";
        }
    }

    public class BankStatement
    {
        public string AccountNumber { get; private set; }
        public decimal Amount { get; private set; }
        public DateTimeOffset AccountingDate { get; private set; }

        public BankStatement(string accountNumber, string amountAsString, string accountingDateAsIsoDateString)
        {
            AccountNumber = accountNumber;
            Amount = decimal.Parse(amountAsString);
            AccountingDate = DateTimeOffset.Parse(accountingDateAsIsoDateString);
        }
    }
}
