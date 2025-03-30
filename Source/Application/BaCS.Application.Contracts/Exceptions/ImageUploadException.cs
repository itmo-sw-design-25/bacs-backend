namespace BaCS.Application.Contracts.Exceptions;

public class ImageUploadException : ApplicationException
{
    public ImageUploadException(string message)
        : base(message) { }

    public ImageUploadException(string message, Exception innerException)
        : base(message, innerException) { }
}
