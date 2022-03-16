
using Contacting.Domain.Persons;
using Contacting.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contacting.Infrastructure.EntityConfigurations
{
    public class PersonEntityTypeConfiguration
        : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> configuration)
        {
            configuration.ToTable("persons", ContactingContext.DEFAULT_SCHEMA);

            configuration.HasKey(o => o.Id);

            configuration.Ignore(b => b.DomainEvents);

            configuration.Property(o => o.Id).ValueGeneratedNever();

            configuration.HasMany(x => x.Contacts)
                .WithOne()
                .HasForeignKey(x => x.PersonId)
                .IsRequired();

            configuration.Navigation(x => x.Contacts).AutoInclude();

        }
    }
}
