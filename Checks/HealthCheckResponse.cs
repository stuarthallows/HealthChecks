using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;

namespace HealthChecks.Checks;

/// <summary>
/// A health check response.
/// </summary>
public class HealthCheckResponse
{
    /// <summary>
    /// Gets the default JSON settings.
    /// </summary>
    public static JsonSerializerSettings JsonSettings { get; } = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };

    // /// <summary>
    // /// Gets or sets the health check name.
    // /// </summary>
    // public string HealthCheckName { get; set; } = string.Empty;

    // /// <summary>
    // /// Gets or sets the health check description.
    // /// </summary>
    // public string HealthCheckDescription { get; set; } = string.Empty;

    /// <summary>
    /// Write response to /healthz endpoint.
    /// </summary>
    /// <param name="context">The http context.</param>
    /// <param name="healthReport">The health report.</param>
    /// <returns>An awaitable task.</returns>
    public static Task WriteHealthZResponse(HttpContext context, HealthReport healthReport)
    {
        context.Response.ContentType = "application/json; charset=utf-8";

        var options = new JsonWriterOptions { Indented = true };

        using var memoryStream = new MemoryStream();
        using (var jsonWriter = new Utf8JsonWriter(memoryStream, options))
        {
            jsonWriter.WriteStartObject();
            jsonWriter.WriteString("status", healthReport.Status.ToString());
            jsonWriter.WriteStartObject("results");

            foreach (var healthReportEntry in healthReport.Entries)
            {
                jsonWriter.WriteStartObject(healthReportEntry.Key);
                jsonWriter.WriteString("status",
                    healthReportEntry.Value.Status.ToString());
                jsonWriter.WriteString("description",
                    healthReportEntry.Value.Description);
                jsonWriter.WriteStartObject("data");

                foreach (var item in healthReportEntry.Value.Data)
                {
                    jsonWriter.WritePropertyName(item.Key);

                    System.Text.Json.JsonSerializer.Serialize(jsonWriter, item.Value, item.Value?.GetType() ?? typeof(object));
                }

                jsonWriter.WriteEndObject();
                jsonWriter.WriteEndObject();
            }

            jsonWriter.WriteEndObject();
            jsonWriter.WriteEndObject();
        }

        return context.Response.WriteAsync(Encoding.UTF8.GetString(memoryStream.ToArray()));
    }
}
