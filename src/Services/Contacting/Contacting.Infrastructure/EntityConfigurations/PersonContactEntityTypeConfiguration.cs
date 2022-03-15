
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
    public class PersonContactEntityTypeConfiguration
        : IEntityTypeConfiguration<PersonContact>
    {
        public void Configure(EntityTypeBuilder<PersonContact> configuration)
        {
            configuration.ToTable("personcontacts", ContactingContext.DEFAULT_SCHEMA);

            configuration.HasKey(o => o.Id);

            configuration.Ignore(b => b.DomainEvents);

            configuration.Property(o => o.Id).ValueGeneratedNever();
 
        }
    }
}
