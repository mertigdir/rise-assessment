using Contacting.Dto.Persons;
using Dapper;
using Npgsql;
using System;
using System.Linq;
using System.Threading.Tasks;
using Utility.Paginations;

namespace Contacting.Application.Queries
{
    public class PersonQueries : IPersonQueries
    {
        private string _connectionString = string.Empty;

        public PersonQueries(string constr)
        {
            _connectionString = !string.IsNullOrWhiteSpace(constr) ? constr : throw new ArgumentNullException(nameof(constr));
        }

        public async Task<PaginationResult<PersonDto>> GetPersonsAsync(
            Guid? id = null,
            string name = null,
            string surname = null,
            string company = null,
            int skip = 0,
            int take = 10
            )
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                var templateQuery =
                    @"SELECT COUNT(0) Count
                      FROM contacting.persons {0};

                     SELECT *
                      FROM contacting.persons AS p
                      {0}
                      ORDER BY p.id
                      OFFSET @PageSize ROWS FETCH NEXT @Size ROWS ONLY;
                      ";

                var dynamicParams = new DynamicParameters();
                string whereClause = "WHERE 1=1 ";
                if (id.HasValue)
                {
                    whereClause += "AND p.id=@Id";
                    dynamicParams.Add("Id", id);
                }

                if (!string.IsNullOrWhiteSpace(name))
                {
                    whereClause += " AND p.name LIKE '%@Name%'";
                    dynamicParams.Add("Name", name);
                }

                if (!string.IsNullOrWhiteSpace(surname))
                {
                    whereClause += " AND p.surname  LIKE '%@Surname%'";
                    dynamicParams.Add("Surname", surname);
                }

                if (!string.IsNullOrWhiteSpace(company))
                {
                    whereClause += " AND p.company LIKE '%@Company%'";
                    dynamicParams.Add("Company", company);
                }

                dynamicParams.Add("PageSize", skip);
                dynamicParams.Add("Size", take);

                var multi = await connection.QueryMultipleAsync(string.Format(templateQuery, whereClause), dynamicParams);
                var totalCount = multi.Read<int>().Single();
                var results = multi.Read<PersonDto>().ToList();

                return new PaginationResult<PersonDto>()
                {
                    Results = results,
                    TotalCount = totalCount
                };
            }
        }

        public async Task<PersonDto> GetPersonWithContactsAsync(Guid id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                string templateQuery = @"SELECT * FROM contacting.persons AS p WHERE p.id=@Id;
                                         SELECT * FROM contacting.personcontacts AS pc WHERE pc.person_id=@Id;";

                var multi = await connection.QueryMultipleAsync(templateQuery, new { Id = id });

                var person = await multi.ReadSingleOrDefaultAsync<PersonDto>();
                if (person != null)
                    person.Contacts = (await multi.ReadAsync<PersonContactDto>()).ToList();

                return person;
            }
        }
    }
}