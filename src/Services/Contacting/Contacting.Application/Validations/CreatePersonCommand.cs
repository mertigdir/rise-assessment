using Contacting.Application.Commands;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Contacting.Application.Validations
{
    public class CreateAuctionCommandValidator : AbstractValidator<CreatePersonCommand>
    {
        public CreateAuctionCommandValidator(ILogger<CreateAuctionCommandValidator> logger)
        {
            RuleFor(command => command.Name)
                .NotEmpty()
                .WithMessage("Field is required");

            RuleFor(command => command.Surname)
                .NotEmpty()
                .WithMessage("Field is required");

            RuleFor(command => command.Company)
                .NotEmpty()
                .WithMessage("Field is required");

            logger.LogTrace("----- INSTANCE CREATED - {ClassName}", GetType().Name);
        }
    }
}
