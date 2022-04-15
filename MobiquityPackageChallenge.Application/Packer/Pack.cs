using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using MobiquityPackageChallenge.Domain.Models;

namespace MobiquityPackageChallenge.Application.Packer
{
    public class PackCommand: IRequest<string>
    {
        public PackCommand(byte[] file)
        {
            File = file;
        }

        public byte[] File { get; private set; }
    }

    public class PackValidator : AbstractValidator<PackCommand>
    {
        public PackValidator()
        {
            RuleFor(c => c.File)
                .Must(BeInUTF8Format)
                .WithMessage("File is not in UTF-8 format.");
        }

        private bool BeInUTF8Format(byte[] bytes)
        {
            try
            {
                var text = Encoding.UTF8.GetString(bytes);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    public class PackHandler: IRequestHandler<PackCommand, string>
    {
        private readonly IExtractor _Extractor;

        public PackHandler(IExtractor extractor)
        {
            _Extractor = extractor;
        }

        public Task<string> Handle(PackCommand request, CancellationToken cancellationToken)
        {
            var dtos = _Extractor.Extract(request.File);

            var packages = dtos.Select(dto => Pack(dto, new List<ItemDto>())).ToList();

            var result = new StringBuilder();

            foreach (var package in packages)
            {
                if (!package.Items.Any())
                {
                    result.Append("-\n");
                }
                else
                {
                    var itemsIndices = package.Items.Select(i => i.Index).ToList();
                    result.Append(string.Join(',', itemsIndices));
                }

                result.Append("\n");
            }

            return Task.FromResult(result.ToString());
        }

        //TODO: implement Knapsack
        private Package Pack(PackageDto dto, List<ItemDto> packedItems)
        {
            var capacity = dto.WeightLimit - packedItems.Sum(x => x.Weight);
            
            var items = dto.Items.Except(packedItems)
                .Where(x => x.Weight <= capacity)
                .OrderByDescending(x => x.Cost.Amount)
                .ThenBy(x => x.Weight)
                .ToList();

            if (!items.Any())
                return new Package(
                    dto.WeightLimit,
                    packedItems.Select(x => new Item(x.Index, x.Weight, new Cost(x.Cost.CurrencySymbol, x.Cost.Amount)))
                );

            if (items.Sum(x => x.Weight) <= dto.WeightLimit)
                return new Package(
                    dto.WeightLimit,
                    items.Select(x => new Item(x.Index, x.Weight, new Cost(x.Cost.CurrencySymbol, x.Cost.Amount)))
                );
            
            packedItems.Add(items.First());

            return Pack(dto, packedItems);
        }
    } 
}
