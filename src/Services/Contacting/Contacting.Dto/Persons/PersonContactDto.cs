﻿using Contacting.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using Contacting.Domain.SeedWork;
using Contacting.Domain.Events;
using Contacting.Domain.Shared.Persons;

namespace Contacting.Domain.Auctions
{
    public record PersonContactDto
    {
        public Guid Id { get; set; }
        public ContactType ContactType { get; set; }
        public string Content { get; set; }
    }
}