using Contacting.Application.Commands;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Contacting.Application.Validations
{
    public class DeletePersonContactCommandValidator : AbstractValidator<DeletePersonContactCommand>
    {
        public DeletePersonContactCommandValidator(ILogger<DeletePersonContactCommandValidator> logger)
        {
            RuleFor(command => command.PersonId)
                .NotEmpty()
                .WithMessage("Field is required");

            RuleFor(command => command.PersonContactId)
                .NotEmpty()
                .WithMessage("Field is required");

            logger.LogTrace("----- INSTANCE CREATED - {ClassName}", GetType().Name);
        }
    }
}
