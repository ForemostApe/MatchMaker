namespace MatchMaker.Core.Utilities;

public class FileValidationOptions
{
    public string[] ValidFileExtensions { get; set; } = Array.Empty<string>();
    public int MaxFileSize { get; set; }
}