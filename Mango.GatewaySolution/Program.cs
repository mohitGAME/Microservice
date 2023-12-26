
using GraphiQl;
using GraphQL;
using GraphQL.SystemTextJson;
using Mango.GatewaySolution;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using StarWars;
using Mango.GatewaySolution.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddAppAuthetication();

builder.Services.AddGraphQL(b => b
               .AddSchema<StarWarsSchema>()
               .AddSystemTextJson()
               .AddValidationRule<InputValidationRule>()
               .AddGraphTypes(typeof(StarWarsSchema).Assembly));
//.AddApolloTracing(options => options.RequestServices!.GetRequiredService<IOptions<GraphQLSettings>>().Value.EnableMetrics));

builder.Services.Configure<GraphQLSettings>(builder.Configuration.GetSection("GraphQLSettings"));
builder.Services.AddSingleton<StarWarsData>();
builder.Services.AddLogging(builder => builder.AddConsole());
builder.Services.AddHttpClient<StarWarsQuery>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews()
    .AddJsonOptions(opts => opts.JsonSerializerOptions.Converters.Add(new InputsJsonConverter()));
if (builder.Environment.EnvironmentName.ToString().ToLower().Equals("production"))
{
    builder.Configuration.AddJsonFile("ocelot.Production.json", optional: false, reloadOnChange: true);
}
else
{
    builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
}
builder.Services.AddOcelot();

//builder.Configuration
var app = builder.Build();
if (builder.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.UseRouting();
app.UseGraphiQl();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
        );
});
app.UseOcelot().GetAwaiter().GetResult();
app.UseDefaultFiles();
app.UseStaticFiles();
app.Run();
