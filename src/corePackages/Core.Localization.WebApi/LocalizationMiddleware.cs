using System.Collections.Immutable;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Core.Localization.Abstraction;
using Microsoft.Extensions.Primitives;

namespace Core.Localization.WebApi;

public class LocalizationMiddleware
{
    private readonly RequestDelegate _next;

    public LocalizationMiddleware(RequestDelegate next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
    }

    public async Task Invoke(HttpContext context, ILocalizationService localizationService)
    {
        IList<StringWithQualityHeaderValue> acceptLanguages = context.Request.GetTypedHeaders().AcceptLanguage;

        if (acceptLanguages == null || acceptLanguages.Count == 0)
        {

            acceptLanguages = new List<StringWithQualityHeaderValue>();
            var x = new StringWithQualityHeaderValue("en");
            acceptLanguages.Add(x);


        }
        localizationService.AcceptLocales = acceptLanguages
            .OrderByDescending(x => x.Quality ?? 1)
            .Select(x => x.Value.ToString())
            .ToImmutableArray();
        await _next(context);
    }
}
