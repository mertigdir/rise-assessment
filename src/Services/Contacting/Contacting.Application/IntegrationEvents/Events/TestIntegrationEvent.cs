﻿using Contacting.Domain.Auctions;
using Contacting.Dto.Auctions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contacting.Application.IntegrationEvents.Events
{
    public record TestIntegrationEvent
    {
        public Guid Id { get; set; }
    }
}
