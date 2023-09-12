using Microsoft.AspNetCore.Http;

namespace AmazonWebServices.Extensions
{
    internal static class FileControlExtension
    {
        //The signature of the files to be uploaded to the server is checked for security purposes.
        //If you want to add a signature later, reference can be taken from https://www.filesignatures.net/.
        private static readonly Dictionary<string, List<byte[]>> FileSignatures =
            new()
            {
                {
                    ".jpeg",
                    new List<byte[]>
                    {
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 }
                    }
                },
                {
                    ".jpg",
                    new List<byte[]>
                    {
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 }
                    }
                },
                {
                    ".png",
                    new List<byte[]>
                    {
                        new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A },
                        new byte[] { 0x89, 0x50, 0x4E, 0x47 }
                    }
                }
                ,
                {
                    ".pdf",
                    new List<byte[]>
                    {
                        new byte[] { 0x25, 0x50, 0x44, 0x46 }
                    }
                },
                {
                    ".svg",
                    new List<byte[]>
                    {
                        new byte[]{ 0x3C ,0x3F, 0x78, 0x6D, 0x6C, 0x20, 0x76, 0x65, 0x72, 0x73, 0x69, 0x6F, 0x6E, 0x3D, 0x22, 0x31, 0x2E, 0x30, 0x22, 0x3F, 0x3E }
                    }
                }
            };

        public static void Verify(this IFormFile file)
        {
            string[] permittedExtensions = { ".jpeg", ".jpg", ".png", ".pdf", ".svg" };

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                throw new FileLoadException();

            if (!file.SignatureControl())
                throw new FileLoadException();
        }

        private static bool SignatureControl(this IFormFile file)
        {
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (ext.Equals(".svg"))
                return file.IsSvgFile();

            using var reader = new BinaryReader(file.OpenReadStream());

            var signatures = FileSignatures[ext];

            var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

            var result = signatures.Any(signature => headerBytes.Take(signature.Length).SequenceEqual(signature));

            if (result) return true;
            {
                var signatureValues = FileSignatures.Values.SelectMany(signature => signature);

                foreach (var signatureValue in signatureValues)
                {
                    result = signatureValue.SequenceEqual(headerBytes);
                    if (result)
                        return true;
                }
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
}
