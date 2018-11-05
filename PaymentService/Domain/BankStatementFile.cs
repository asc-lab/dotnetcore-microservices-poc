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
        private readonly ILogger<BankStatementFile> logger;

        public BankStatementFile(string path, DateTimeOffset importDate, ILogger<BankStatementFile> logger)
        {
            FilePath = path ?? throw new ArgumentNullException(nameof(path));
            FileName = ConstructFileNameFromDate(importDate);
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string FilePath { get; private set; }

        public string FileName { get; private set; }

        public string FullPath
        {
            get
            {
                return Path.Combine(FilePath, FileName);
            }
        }

        public string ProcessedFullPath
        {
            get
            {
                return Path.Combine(FilePath, $"_processed_{FileName}");
            }
        }

        public bool Exists()
        {
            return File.Exists(FullPath);
        }

        public List<BankStatement> Read()
        {
            try {
                return File.ReadAllLines(FullPath)
                                        .Skip(1) // skip header column
                                        //.SelectTry(x => ReadRow(x))
                                        //.OnCaughtException(ex => { logger.LogError(ex.Message); return null; })
                                        //.Where(x => x != null)
                                        .Select(x => ReadRow(x))
                                        .ToList();
            } catch (FileNotFoundException ex)
            {
                logger.LogError("Bank statement file not found. Looking for  " + FilePath, ex);
                throw new BankStatementsFileNotFound(ex);
            }
            catch (IOException ex)
            {
                logger.LogError("Error while processing file " + FilePath, ex);
                throw new BankStatementsFileReadingError(ex);
            }
        }

        public void MarkProcessed()
        {
            File.Copy(FullPath, ProcessedFullPath);
        }

        public BankStatement ReadRow(string row)
        {
            var cells = row.Split(",");
            string accountingDate = cells[2];
            string accountNumber = cells[3];
            string amountAsString = cells[4];
            return new BankStatement(accountNumber, amountAsString, accountingDate);
        }

        public string ConstructFileNameFromDate(DateTimeOffset importDate)
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
