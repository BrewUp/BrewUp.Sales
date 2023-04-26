using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace BrewUp.Sales.Modules;

public sealed class SwaggerModule : IModule
{
	/// <summary>
	/// 
	/// </summary>
	public bool IsEnabled => true;
	/// <summary>
	/// 
	/// </summary>
	public int Order => 0;

	/// <summary>
	/// 
	/// </summary>
	public const string BearerId = "Bearer";

	/// <summary>
	/// 
	/// </summary>
	public IServiceCollection RegisterModule(WebApplicationBuilder builder)
	{
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen(SetSwaggerGenOptions);

		return builder.Services;
	}

	/// <summary>
	/// 
	/// </summary>
	public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints) => endpoints;

	private void SetSwaggerGenOptions(SwaggerGenOptions options)
	{
		options.OperationFilter<SecurityRequirementsOperationFilter>();
		options.SwaggerDoc("v1", new OpenApiInfo
		{
			Title = "BrewUp Sales Api",
			Version = "v1"
		});
		options.AddSecurityDefinition(BearerId, new OpenApiSecurityScheme
		{
			Type = SecuritySchemeType.Http,
			In = ParameterLocation.Header,
			Name = "Authorization",
			Scheme = JwtBearerDefaults.AuthenticationScheme,
			BearerFormat = "JWT",
			Description = "Please enter a valid token"
		});

		ConfigureXmlComments(options);
	}

	private void ConfigureXmlComments(SwaggerGenOptions options)
	{
		var xmlFile = Path.Combine(
			Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,
			$"{GetType().Assembly.GetName().Name}.xml");

		// Tells swagger to pick up the output XML document file
		if (!File.Exists(xmlFile))
			return;

		var currentAssembly = Assembly.GetExecutingAssembly();
		options.IncludeXmlComments(xmlFile);

		// Collect all referenced projects output XML document file paths
		var xmlDocs = currentAssembly.GetReferencedAssemblies()
			.Union(new[] { currentAssembly.GetName() })
			.Select(a => Path.Combine(Path.GetDirectoryName(currentAssembly.Location)!, $"{a.Name}.xml"))
			.Where(File.Exists).ToArray();

		Array.ForEach(xmlDocs, (d) => { options.IncludeXmlComments(d); });
	}
}

// ReSharper disable once ClassNeverInstantiated.Global
internal class SecurityRequirementsOperationFilter : IOperationFilter
{
	public void Apply(OpenApiOperation operation, OperationFilterContext context)
	{
		var requiredScopes = context.MethodInfo
			.GetCustomAttributes(true)
			.OfType<AuthorizeAttribute>()
			.Select(attr => attr.AuthenticationSchemes)
			.Distinct()
			.ToArray();

		var requireAuth = false;
		var id = string.Empty;

		if (requiredScopes.Contains(JwtBearerDefaults.AuthenticationScheme))
		{
			requireAuth = true;
			id = SwaggerModule.BearerId;
		}

		if (!requireAuth || string.IsNullOrEmpty(id)) return;

		operation.Responses.Add("401", new OpenApiResponse
		{
			Description = "Unauthorized",
		});
		operation.Responses.Add("403", new OpenApiResponse
		{
			Description = "Forbidden",
		});

		operation.Security = new List<OpenApiSecurityRequirement>
		{
			new()
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = id
						}
					},
					Array.Empty<string>()
				}
			}
		};
	}
}