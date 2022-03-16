using Contacting.Application.Commands;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Contacting.Application.Validations
{
    public class DeletePersonCommandValidator : AbstractValidator<DeletePersonCommand>
    {
        public DeletePersonCommandValidator(ILogger<DeletePersonCommandValidator> logger)
        {
            RuleFor(command => command.PersonId)
                .NotEmpty()
                .WithMessage("Field is required");

            logger.LogTrace("----- INSTANCE CREATED - {ClassName}", GetType().Name);
        }
    }
}
