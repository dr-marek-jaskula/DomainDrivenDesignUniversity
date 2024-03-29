﻿using FluentValidation;

namespace Shopway.Application.Features.Products.Commands.UpdateProduct;

internal sealed class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id).NotNull();
        RuleFor(x => x.Body).NotNull();
        RuleFor(x => x.Body.Price).NotNull();
    }
}