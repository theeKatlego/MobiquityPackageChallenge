namespace MobiquityPackageChallenge.Domain.Exceptions; //Yes, this is not com.mobiquity.packer.APIException I hope that's not a bummer

public class APIException : Exception
{
    public APIException(string? message) : base() { }
    public APIException(string? message, Exception? innerException) : base() { }
}