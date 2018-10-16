using PaymentService.Api.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentService.Domain
{
    public class BankStatementFile
    {

        public BankStatementFile(string path, DateTimeOffset importDate)
        {
            Path = path;
            FileName = ConstructFileNameFromDate(importDate);
        }

        public string Path { get; private set; }
        public string FileName { get; private set; }

        public string FullPath
        {
            get
            {
                return System.IO.Path.Combine(Path, FileName);
            }
        }

        public string ProcessedFullPath
        {
            get
            {
                return System.IO.Path.Combine(Path, $"_processed_{FileName}");
            }
        }

        public bool Exists()
        {
            return File.Exists(FullPath);
        }

        public List<BankStatement> Read()
        {
            try {
                using (var reader = new StreamReader(FullPath)) {
                    List<BankStatement> statements = new List<BankStatement>();

                    reader.ReadLine();
                    while (!reader.EndOfStream)
                    {
                        statements.Add(ReadRow(reader.ReadLine()));
                    }
                    return statements;
                }
            } catch (FileNotFoundException ex)
            {
                //log.error("Bank statement file not found. Looking for  " + Path, ex);
                throw new BankStatementsFileNotFound(ex);
            }
            catch (IOException ex)
            {
                //log.error("Error while processing file " + path, ex);
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
