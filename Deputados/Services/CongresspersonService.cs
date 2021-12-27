
using MongoDB.Driver;
using Congressperson.Models;
using System.Collections.Generic;
using Congressperson.Services.Interfaces;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using Congressperson.Models.DTO;

namespace Congressperson.Services
{
    public class CongresspersonService : ICongresspersonService
    {
        private readonly IMongoCollection<Models.Congressperson> _congresspeople;

        public CongresspersonService(ICongresspersonDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _congresspeople = database.GetCollection<Models.Congressperson>(settings.CongresspersonCollectionName);
        }

        public IEnumerable<Models.Congressperson> Get() => 
            _congresspeople.Find(congressperson => true).ToEnumerable<Models.Congressperson>();

        public Models.Congressperson Get(string id) =>
            _congresspeople.Find<Models.Congressperson>(congressperson => congressperson.IdDatabase == id).FirstOrDefault();

        public Models.Congressperson GetByIdCongressperson(int idCongressperson) =>
            _congresspeople.Find<Models.Congressperson>(congressperson => congressperson.IdCongressperson == idCongressperson).FirstOrDefault();

        public CongresspersonStatistics GetCongresspersonStatistics(int idCongressperson)
        {
            Models.Congressperson congressperson = GetByIdCongressperson(idCongressperson);

            var expensensByMonth = ComputeCongresspersonExpensesByMonth(congressperson);

            var expensensByYear = ComputeCongresspersonExpensesByYear(expensensByMonth);

            return new CongresspersonStatistics { ExpensesByYear = expensensByYear, ExpensesByMonth = expensensByMonth };
        }

        private static IEnumerable<ExpensensByMonth> ComputeCongresspersonExpensesByMonth(Models.Congressperson congressperson) =>
            congressperson.Expenses.GroupBy(expense => new { expense.Year, expense.Month })
                                                    .Select(_ => new ExpensensByMonth
                                                    {
                                                        Year = _.First().Year,
                                                        Month = _.First().Month,
                                                        Value = _.Sum(s => s.GrossAmount + s.NonRefundableAmount)
                                                    }).OrderBy(expense => expense.Year).ThenBy(expense => expense.Month );

        private static IEnumerable<ExpensensByYear> ComputeCongresspersonExpensesByYear(IEnumerable<ExpensensByMonth> expensensByMonth) =>
            expensensByMonth.GroupBy(expense => expense.Year)
                                                    .Select(_ => new ExpensensByYear
                                                    {
                                                        Year = _.First().Year,
                                                        Value = _.Sum(s => s.Value)
                                                    }).OrderBy(expense => expense.Year);

        public void Insert(Models.Congressperson congressperson)
        {
            if (GetByIdCongressperson(congressperson.IdCongressperson) == null)
                _congresspeople.InsertOne(congressperson);
        }

        public void InsertMany(IEnumerable<Models.Congressperson> congresspeople)
        {
            var congresspeopleInDb = Get();
            var congresspeopleNotInDb = congresspeople.Except(congresspeopleInDb, new IdCongresspeopleComparer());
            _congresspeople.InsertMany(congresspeopleNotInDb);
        }

        public void Update(Models.Congressperson congressperson)
        {
            _congresspeople.ReplaceOne(_ => _.IdDatabase == congressperson.IdDatabase, congressperson);
        }

        public void Remove(Models.Congressperson congressperson)
        {
            _congresspeople.DeleteOne(_ => _.IdDatabase == congressperson.IdDatabase);
        }

        public void Remove(string id)
        {
            _congresspeople.DeleteOne(congressperson => congressperson.IdDatabase == id);
        }
    }

    public class IdCongresspeopleComparer : IEqualityComparer<Models.Congressperson>
    {
        public bool Equals(Models.Congressperson x, Models.Congressperson y)
        {
            if (ReferenceEquals(x, y)) return true;

            if (x is null || y is null)
                return false;

            return x.IdCongressperson == y.IdCongressperson;
        }

        public int GetHashCode([DisallowNull] Models.Congressperson congressperson)
        {
            if (congressperson is null) return 0;

            int hashIdCongressperson = congressperson.IdCongressperson.GetHashCode();

            return hashIdCongressperson;
        }
    }
}
