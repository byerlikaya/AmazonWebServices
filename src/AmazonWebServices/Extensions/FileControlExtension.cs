namespace AmazonWebServices.Extensions;

internal static class FileControlExtension
{
    //The signature of the files to be uploaded to the server is checked for security purposes.
    //If you want to add a signature later, reference can be taken from https://www.filesignatures.net/.
    private static readonly Dictionary<string, List<byte[]>> FileSignatures =
        new()
        {
            {
                ".jpeg",
                [
                    [0xFF, 0xD8, 0xFF, 0xE0],
                    [0xFF, 0xD8, 0xFF, 0xE2],
                    [0xFF, 0xD8, 0xFF, 0xE3]
                ]
            },
            {
                ".jpg",
                [
                    [0xFF, 0xD8, 0xFF, 0xE0],
                    [0xFF, 0xD8, 0xFF, 0xE1],
                    [0xFF, 0xD8, 0xFF, 0xE8]
                ]
            },
            {
                ".png",
                [
                    [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A],
                    [0x89, 0x50, 0x4E, 0x47]
                ]
            }
            ,
            {
                ".pdf",
                [
                    [0x25, 0x50, 0x44, 0x46]
                ]
            },
            {
                ".svg",
                [
                    [0x3C, 0x3F, 0x78, 0x6D, 0x6C, 0x20, 0x76, 0x65, 0x72, 0x73, 0x69, 0x6F, 0x6E, 0x3D, 0x22, 0x31, 0x2E, 0x30, 0x22, 0x3F, 0x3E]
                ]
            },
            {
                ".webp",
                [
                    [0x57, 0x45, 0x42, 0x50],
                    [0x52, 0x49, 0x46, 0x46]
                ]
            }
        };

    public static void Verify(this IFormFile file)
    {
        string[] permittedExtensions = [".jpeg", ".jpg", ".png", ".pdf", ".svg", ".webp"];

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            throw new FileLoadException();

        if (!file.SignatureControl())
            throw new FileLoadException();
    }

    private static bool SignatureControl(this IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (extension.Equals(".svg"))
            return file.IsSvgFile();

        using var binaryReader = new BinaryReader(file.OpenReadStream());

        var signatures = FileSignatures[extension];

        var headerBytes = binaryReader.ReadBytes(signatures.Max(m => m.Length));

        var result = signatures.Any(signature => headerBytes.Take(signature.Length).SequenceEqual(signature));

        if (result)
            return true;

        var signatureValues = FileSignatures.Values.SelectMany(signature => signature);

        foreach (var signatureValue in signatureValues)
        {
            result = signatureValue.SequenceEqual(headerBytes);
            if (result)
                return true;
        }

        return false;
    }

    private static bool IsSvgFile(this IFormFile svgFile)
    {
        try
        {
            using var file = svgFile.OpenReadStream();

            var reader = new StreamReader(file);

            var readCount = reader.Read();

            return readCount > 0;
        }
        catch
        {
            return false;
        }
    }
}