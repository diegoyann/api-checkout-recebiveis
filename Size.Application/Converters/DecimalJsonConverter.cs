using System.Text.Json;
using System.Text.Json.Serialization;

namespace Size.Application.Converters;

/// <summary>
/// Converter JSON para formatar valores decimais com 2 casas decimais
/// </summary>
public class DecimalJsonConverter : JsonConverter<decimal>
{
    /// <summary>
    /// Lê um valor decimal do JSON
    /// </summary>
    /// <param name="reader">Reader JSON</param>
    /// <param name="typeToConvert">Tipo a converter</param>
    /// <param name="options">Opções de serialização</param>
    /// <returns>Valor decimal</returns>
    public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetDecimal();
    }

    /// <summary>
    /// Escreve um valor decimal no JSON com 2 casas decimais
    /// </summary>
    /// <param name="writer">Writer JSON</param>
    /// <param name="value">Valor decimal</param>
    /// <param name="options">Opções de serialização</param>
    public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(Math.Round(value, 2));
    }
}