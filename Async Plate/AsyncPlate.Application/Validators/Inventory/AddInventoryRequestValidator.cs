using AsyncPlate.Application.DTOs.Inventory;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Validators.Inventory
{
    public class AddInventoryRequestValidator: AbstractValidator<AddInventoryRequestDTO>
    {
        public AddInventoryRequestValidator() {

            RuleFor(x => x.Name)
               .NotEmpty()
               .WithMessage("Inventory name is required.")
               .MaximumLength(200);

            RuleFor(x => x.CurrentStock)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Current stock cannot be negative.");

            RuleFor(x => x.MinStockLevel)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Minimum stock level cannot be negative.");

            RuleFor(x => x.Unit)
                .NotEmpty()
                .WithMessage("Unit is required.")
                .MaximumLength(50);

            RuleFor(x => x.PurchasedUnitPrice)
                .GreaterThan(0)
                .WithMessage("Purchased unit price must be greater than zero.");

            RuleFor(x => x.SupplierId)
                .NotEmpty()
                .WithMessage("Supplier id is required.");
        }
    }
}
