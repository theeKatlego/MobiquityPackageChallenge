using System.ComponentModel;
using System.Globalization;
using System.Text;
using MobiquityPackageChallenge.Domain.Exceptions;

namespace MobiquityPackageChallenge.Application.Packer;

public interface IExtractor
{
    IEnumerable<PackageDto> Extract(byte[] bytes);
}

public class Extractor: IExtractor
{
    public IEnumerable<PackageDto> Extract(byte[] bytes)
    {
        try
        {
            var text = Encoding.UTF8
                .GetString(bytes)
                .Trim();
            var dtos = new List<PackageDto>();

            var packages = new List<PackageDto>();

            using (var reader = new StringReader(text))
            {
                string line; // 81 : (1,53.38,€45) (2,88.62,€98) (3,78.48,€3) (4,72.30,€76) (5,30.18,€9) (6,46.34,€48)

                while ((line = reader.ReadLine()) != null)
                {
                    var data = line.Split(" ");
                    
                    var weightLimit = TryExtractValue<double>(data.FirstOrDefault());

                    var itemsData = data.Skip(2).ToList(); // [(1,53.38,€45), (2,88.62,€98), (3,78.48,€3), (4,72.30,€76), (5,30.18,€9), (6,46.34,€48)]

                    var items = ExtractItems(itemsData);

                    var packageDto = new PackageDto
                    {
                        WeightLimit = weightLimit,
                        Items = items
                    };
                    packages.Add(packageDto);
                }

                return packages;
            }

            return dtos;
        }
        catch (Exception e)
        {
            throw new APIException("Failed to extract packages.", e);
        }
    }

    private IEnumerable<ItemDto> ExtractItems(List<string> itemsData)
    {
        var items = new List<ItemDto>();

        foreach (var itemData in itemsData)
        {
            var itemDataValues = itemData
                .Substring(1, itemData.Length - 2) // 1,53.38,€45
                .Split(','); // [1, 53.38, €45]

            var index = TryExtractValue<int>(itemDataValues[0]);
            var weight = TryExtractValue<double>(itemDataValues[1]);

            var costData = itemDataValues[2]; // €45
            var costCurrencySymbol = costData[0]; // €
            var costAmountData = costData.Substring(1, costData.Length - 1); // 45
            var costAmount = TryExtractValue<double>(costAmountData);

            var costDto = new CostDto
            {
                CurrencySymbol = costCurrencySymbol,
                Amount = costAmount
            };

            var itemDto = new ItemDto
            {
                Cost = costDto,
                Index = index,
                Weight = weight
            };

            items.Add(itemDto);
        }

        return items;
    }

    private T TryExtractValue<T>(string stringValue)
    {
        var converter = TypeDescriptor.GetConverter(typeof(T));

        var value = converter.ConvertFromString(null, CultureInfo.InvariantCulture, stringValue);
        
        if (value == null)
            throw new APIException($"Failed to parse '{stringValue}' to {typeof(T).FullName}");

        return (T)value;
    }
}

public struct PackageDto
{
    public double WeightLimit { get; set; }
    public IEnumerable<ItemDto> Items { get; set; }
}

public struct ItemDto
{
    public int Index { get; set; }
    public double Weight { get; set; }
    public CostDto Cost { get; set; }
}

public struct CostDto
{
    public char CurrencySymbol { get; set; }
    public double Amount { get; set; }
}