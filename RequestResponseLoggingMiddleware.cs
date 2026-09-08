using System.Diagnostics;
using System.Text;
using CompanyApplicationApi.Data;
using CompanyApplicationApi.Entities;

namespace CompanyApplicationApi.Middlewares;

public class RequestResponseLoggingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, AppDbContext dbContext)
    {
        var watch = Stopwatch.StartNew();

        // 1. Correlation ID kontrolü (Header'da varsa al, yoksa yeni üret)
        if (!context.Request.Headers.TryGetValue("X-Correlation-ID", out var correlationId))
        {
            correlationId = Guid.NewGuid().ToString();
            context.Request.Headers["X-Correlation-ID"] = correlationId;
        }
        context.Response.Headers["X-Correlation-ID"] = correlationId;

        // 2. Request Body Okuma
        context.Request.EnableBuffering();
        var requestBody = await new StreamReader(context.Request.Body, Encoding.UTF8).ReadToEndAsync();
        context.Request.Body.Position = 0;

        // URL içindeki processId'yi yakalama (varsa)
        var path = context.Request.Path.Value ?? "";
        string? processId = ExtractProcessIdFromPath(path);

        // 3. Response Body'yi yakalamak için geçici akış (stream)
        var originalBodyStream = context.Response.Body;
        using var responseBodyStream = new MemoryStream();
        context.Response.Body = responseBodyStream;

        // İsteği sonraki aşamaya geçir (Controller çalışır)
        await next(context);

        // 4. Response Body Okuma
        responseBodyStream.Position = 0;
        var responseBody = await new StreamReader(responseBodyStream, Encoding.UTF8).ReadToEndAsync();
        responseBodyStream.Position = 0;
        await responseBodyStream.CopyToAsync(originalBodyStream);

        watch.Stop();

        // 5. Veritabanına Log Kaydı
        var log = new ApiLog
        {
            CorrelationId = correlationId.ToString(),
            ProcessId = processId,
            Path = path,
            Method = context.Request.Method,
            RequestBody = requestBody,
            ResponseBody = responseBody,
            StatusCode = context.Response.StatusCode,
            ExecutionTimeMs = watch.ElapsedMilliseconds,
            CreatedAt = DateTime.UtcNow
        };

        dbContext.ApiLogs.Add(log);
        await dbContext.SaveChangesAsync();
    }

    private static string? ExtractProcessIdFromPath(string path)
    {
        var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
        foreach (var segment in segments)
        {
            if (Guid.TryParse(segment, out _))
            {
                return segment;
            }
        }
        return null;
    }
}