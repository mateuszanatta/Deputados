
using MongoDB.Driver;
using Deputados.Models;
using System.Collections.Generic;
using Deputados.Services.Interfaces;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using Deputados.Models.DTO;

namespace Deputados.Services
{
    public class DeputadosService : IDeputadosService
    {
        private readonly IMongoCollection<Deputado> _deputados;

        public DeputadosService(IDeputadosDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _deputados = database.GetCollection<Deputado>(settings.DeputadosCollectionName);
        }

        public IEnumerable<Deputado> Get() => 
            _deputados.Find(deputado => true).ToEnumerable<Deputado>();

        public Deputado Get(string id) =>
            _deputados.Find<Deputado>(deputado => deputado.IdDatabase == id).FirstOrDefault();

        public Deputado GetByIdDeputado(int idDeputado) =>
            _deputados.Find<Deputado>(deputado => deputado.IdDeputado == idDeputado).FirstOrDefault();

        public DeputadoStatistics GetDeputadoStatistics(int idDeputado)
        {
            Deputado deputado = GetByIdDeputado(idDeputado);

            var expensensByMonth = ComputeDeputadoExpensesByMonth(deputado);

            var expensensByYear = ComputeDeputadoExpensesByYear(expensensByMonth);

            return new DeputadoStatistics { ExpensesByYear = expensensByYear, ExpensesByMonth = expensensByMonth };
        }

        private static IEnumerable<ExpensensByMonth> ComputeDeputadoExpensesByMonth(Deputado deputado) =>
            deputado.Expenses.GroupBy(expense => new { expense.Ano, expense.Mes })
                                                    .Select(_ => new ExpensensByMonth
                                                    {
                                                        Year = _.First().Ano,
                                                        Month = _.First().Mes,
                                                        Value = _.Sum(s => s.ValorDocumento + s.ValorGlosa)
                                                    }).OrderBy(expense => expense.Year).ThenBy(expense => expense.Month );

        private static IEnumerable<ExpensensByYear> ComputeDeputadoExpensesByYear(IEnumerable<ExpensensByMonth> expensensByMonth) =>
            expensensByMonth.GroupBy(expense => expense.Year)
                                                    .Select(_ => new ExpensensByYear
                                                    {
                                                        Year = _.First().Year,
                                                        Value = _.Sum(s => s.Value)
                                                    }).OrderBy(expense => expense.Year);

        public void Insert(Deputado deputado)
        {
            if (GetByIdDeputado(deputado.IdDeputado) == null)
                _deputados.InsertOne(deputado);
        }

        public void InsertMany(IEnumerable<Deputado> deputados)
        {
            var deputadosInDb = Get();
            var deputadosNotInDb = deputados.Except(deputadosInDb, new IdDeputadosComparer());
            _deputados.InsertMany(deputadosNotInDb);
        }

        public void Update(Deputado deputado)
        {
            _deputados.ReplaceOne(_ => _.IdDatabase == deputado.IdDatabase, deputado);
        }

        public void Remove(Deputado deputado)
        {
            _deputados.DeleteOne(_ => _.IdDatabase == deputado.IdDatabase);
        }

        public void Remove(string id)
        {
            _deputados.DeleteOne(deputado => deputado.IdDatabase == id);
        }
    }

    public class IdDeputadosComparer : IEqualityComparer<Deputado>
    {
        public bool Equals(Deputado x, Deputado y)
        {
            if (ReferenceEquals(x, y)) return true;

            if (x is null || y is null)
                return false;

            return x.IdDeputado == y.IdDeputado;
        }

        public int GetHashCode([DisallowNull] Deputado deputado)
        {
            if (deputado is null) return 0;

            int hashIdDeputado = deputado.IdDeputado.GetHashCode();

            return hashIdDeputado;
        }
    }
}
