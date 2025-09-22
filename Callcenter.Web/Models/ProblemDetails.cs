﻿using System.Text.Json.Serialization;

namespace Callcenter.Web.Models;

public record ProblemDetails(
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("status")] int Status,
    [property: JsonPropertyName("instance")] string Instance,
    [property: JsonPropertyName("requestId")] string RequestId,
    [property: JsonPropertyName("traceId")] string TraceId
);