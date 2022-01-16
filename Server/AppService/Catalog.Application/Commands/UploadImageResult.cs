using System.Text.Json.Serialization;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json.Converters;

using Catalog.Infrastructure.Persistence;

namespace Catalog.Application.Commands;

public enum UploadImageResult
{
    Successful,
    NotFound
}
