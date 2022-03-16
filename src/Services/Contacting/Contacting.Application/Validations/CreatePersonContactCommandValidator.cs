using Contacting.Application.Commands;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Contacting.Application.Validations
{
    public class CreatePersonContactCommandValidator : AbstractValidator<CreatePersonContactCommand>
    {
        public CreatePersonContactCommandValidator(ILogger<CreatePersonContactCommandValidator> logger)
        {
            RuleFor(command => command.PersonId)
                .NotEmpty()
                .WithMessage("Field is required");

            RuleFor(command => command.Content)
                .NotEmpty()
                .WithMessage("Field is required");

            RuleFor(command => command.ContactType)
                .NotEmpty()
                .WithMessage("Field is required");

            logger.LogTrace("----- INSTANCE CREATED - {ClassName}", GetType().Name);
        }
    }
}
