using Contacting.Domain.Shared;
using Contacting.Infrastructure;
using Utility.Csv;
using Utility.Extensions;
using Utility.Polly;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Contacting.Domain.Persons;
using Bogus;
using PersonEntity = Contacting.Domain.Persons.Person;
using Contacting.Domain.Shared.Persons;

namespace Contacting.Infrastructure
{
    public class ContactingContextSeed
    {
        public async Task SeedAsync(ContactingContext context, IOptions<AppSettings> settings, ILogger<ContactingContextSeed> logger)
        {
            var policy = PollyRetry<ContactingContextSeed>.CreatePolicy(logger, nameof(ContactingContextSeed));

            await policy.ExecuteAsync(async () =>
            {
                AddPersons(context);
                AddPersonContacts(context);

                await context.SaveChangesAsync();
            });
        }
        private void AddPersons(ContactingContext context)
        {
            var faker = new Faker("tr");

            if (context.Persons.Any())
                return;

            for (int i = 0; i < 10; i++)
            {
                context.Persons.Add(new PersonEntity(Guid.NewGuid(), faker.Name.FirstName(), faker.Name.LastName(), faker.Company.CompanyName()));
            }

            context.SaveChanges();
        }

        private void AddPersonContacts(ContactingContext context)
        {
            if (context.PersonContacts.Any())
                return;

            var faker = new Faker("tr");

            var persons = context.Persons.ToList();

            foreach (var person in persons)
            {
                context.PersonContacts.Add(new PersonContact(Guid.NewGuid(), person.Id, ContactType.Email, faker.Internet.Email()));
                context.PersonContacts.Add(new PersonContact(Guid.NewGuid(), person.Id, ContactType.Location, faker.Address.City()));
                context.PersonContacts.Add(new PersonContact(Guid.NewGuid(), person.Id, ContactType.Phone, faker.Phone.PhoneNumber()));
            }


            context.SaveChanges();
        }
    }
}
