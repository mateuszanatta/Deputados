
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
        private readonly IMongoCollection<Models.Congressperson> _deputados;

        public CongresspersonService(ICongresspersonDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _deputados = database.GetCollection<Models.Congressperson>(settings.CongresspersonCollectionName);
        }

        public IEnumerable<Models.Congressperson> Get() => 
            _deputados.Find(deputado => true).ToEnumerable<Models.Congressperson>();

        public Models.Congressperson Get(string id) =>
            _deputados.Find<Models.Congressperson>(deputado => deputado.IdDatabase == id).FirstOrDefault();

        public Models.Congressperson GetByIdDeputado(int idDeputado) =>
            _deputados.Find<Models.Congressperson>(deputado => deputado.IdCongressperson == idDeputado).FirstOrDefault();

        public CongresspersonStatistics GetDeputadoStatistics(int idDeputado)
        {
            Models.Congressperson deputado = GetByIdDeputado(idDeputado);

            var expensensByMonth = ComputeDeputadoExpensesByMonth(deputado);

            var expensensByYear = ComputeDeputadoExpensesByYear(expensensByMonth);

            return new CongresspersonStatistics { ExpensesByYear = expensensByYear, ExpensesByMonth = expensensByMonth };
        }

        private static IEnumerable<ExpensensByMonth> ComputeDeputadoExpensesByMonth(Models.Congressperson deputado) =>
            deputado.Expenses.GroupBy(expense => new { expense.Year, expense.Month })
                                                    .Select(_ => new ExpensensByMonth
                                                    {
                                                        Year = _.First().Year,
                                                        Month = _.First().Month,
                                                        Value = _.Sum(s => s.GrossAmount + s.NonRefundableAmount)
                                                    }).OrderBy(expense => expense.Year).ThenBy(expense => expense.Month );

        private static IEnumerable<ExpensensByYear> ComputeDeputadoExpensesByYear(IEnumerable<ExpensensByMonth> expensensByMonth) =>
            expensensByMonth.GroupBy(expense => expense.Year)
                                                    .Select(_ => new ExpensensByYear
                                                    {
                                                        Year = _.First().Year,
                                                        Value = _.Sum(s => s.Value)
                                                    }).OrderBy(expense => expense.Year);

        public void Insert(Models.Congressperson deputado)
        {
            if (GetByIdDeputado(deputado.IdCongressperson) == null)
                _deputados.InsertOne(deputado);
        }

        public void InsertMany(IEnumerable<Models.Congressperson> deputados)
        {
            var deputadosInDb = Get();
            var deputadosNotInDb = deputados.Except(deputadosInDb, new IdDeputadosComparer());
            _deputados.InsertMany(deputadosNotInDb);
        }

        public void Update(Models.Congressperson deputado)
        {
            _deputados.ReplaceOne(_ => _.IdDatabase == deputado.IdDatabase, deputado);
        }

        public void Remove(Models.Congressperson deputado)
        {
            _deputados.DeleteOne(_ => _.IdDatabase == deputado.IdDatabase);
        }

        public void Remove(string id)
        {
            _deputados.DeleteOne(deputado => deputado.IdDatabase == id);
        }
    }

    public class IdDeputadosComparer : IEqualityComparer<Models.Congressperson>
    {
        public bool Equals(Models.Congressperson x, Models.Congressperson y)
        {
            if (ReferenceEquals(x, y)) return true;

            if (x is null || y is null)
                return false;

            return x.IdCongressperson == y.IdCongressperson;
        }

        public int GetHashCode([DisallowNull] Models.Congressperson deputado)
        {
            if (deputado is null) return 0;

            int hashIdDeputado = deputado.IdCongressperson.GetHashCode();

            return hashIdDeputado;
        }
    }
}
