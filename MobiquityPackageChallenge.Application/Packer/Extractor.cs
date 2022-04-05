namespace MobiquityPackageChallenge.Application.Packer;

public interface IExtractor
{
    IEnumerable<PackageDto> Extract(byte[] bytes);
}

public class Extractor: IExtractor
{
    public IEnumerable<PackageDto> Extract(byte[] bytes)
    {
        throw new NotImplementedException();
    }
}

public class PackageDto
{
    public double WeightLimit { get; set; }
    public IEnumerable<(int index, double weight, double cost)> Items { get; set; }
}