using System.Text.Json;
using System.Text.Json.Serialization;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Profiles.Infrastructure.Persistence.JSON;

/// <summary>
/// Conversor JSON que acepta tanto números como strings para deserializar ESpecialty.
/// 
/// Permite:
/// - "SolarInstallation" (string)
/// - 0 (número)
/// 
/// Ambos se convierten correctamente al enum ESpecialty.
/// </summary>
public class ESpecialtyJsonConverter : JsonConverter<ESpecialty>
{
    public override ESpecialty Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.Number when reader.TryGetInt32(out var intValue) =>
                Enum.IsDefined(typeof(ESpecialty), intValue)
                    ? (ESpecialty)intValue
                    : throw new JsonException($"Valor numérico inválido '{intValue}' para ESpecialty"),
            JsonTokenType.String when reader.GetString() is { } stringValue =>
                Enum.TryParse<ESpecialty>(stringValue, ignoreCase: true, out var enumValue)
                    ? enumValue
                    : throw new JsonException($"No se puede parsear '{stringValue}' como ESpecialty"),
            _ => throw new JsonException($"No se puede deserializar ESpecialty desde {reader.TokenType}")
        };
    }

    public override void Write(Utf8JsonWriter writer, ESpecialty value, JsonSerializerOptions options)
    {
        // Serializar como string en respuestas
        writer.WriteStringValue(value.ToString());
    }
}


