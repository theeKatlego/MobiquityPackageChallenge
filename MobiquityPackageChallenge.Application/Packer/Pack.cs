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
        public byte[] File { get; set; }
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

            var packages = dtos.Select(dto => Pack(dto)).ToList();

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
            }

            return Task.FromResult(result.ToString());
        }

        private Package Pack(PackageDto dto)
        {
            throw new NotImplementedException();
        }
    } 
}
