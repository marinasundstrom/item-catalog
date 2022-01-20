using System.Text.Json.Serialization;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json.Converters;

namespace Catalog.Application.Commands;

public enum UploadImageResult
{
    Successful,
    NotFound
}
