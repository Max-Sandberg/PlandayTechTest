using Planday.Schedule.Api.DTOs.ProblemDetails;
using Planday.Schedule.Api.DTOs.Requests.Examples;
using Planday.Schedule.Api.DTOs.Responses;
using Planday.Schedule.Infrastructure.Providers;
using Planday.Schedule.Infrastructure.Providers.Interfaces;
using Planday.Schedule.Infrastructure.Queries;
using Planday.Schedule.Queries;
using Planday.Schedule.Services;
using Planday.Schedule.Services.ExternalAPIs;
using Planday.Schedule.Services.Interfaces;
using Swashbuckle.AspNetCore.Filters;
using System.Net.Http.Headers;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Strip "Response" from the class names in the schema, e.g. ShiftResponse just appears as Shift.
    options.CustomSchemaIds(type => type.Name.Replace("Response", String.Empty));

    // Locate the XML documentation file generated when the project is built, and use this in the API documentation.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

    // Use the custom example classes in the API documentation.
    options.ExampleFilters();
});

// Register the external employee API.
builder.Services.AddHttpClient<IExternalEmployeeApi, ExternalEmployeeApi>(client =>
{
    client.BaseAddress = new Uri("http://planday-employee-api-techtest.westeurope.azurecontainer.io:5000/");
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("8e0ac353-5ef1-4128-9687-fb9eb8647288");
});

// Register all our services.
builder.Services.AddSingleton<IConnectionStringProvider>(new ConnectionStringProvider(builder.Configuration.GetConnectionString("Database")));
builder.Services.AddScoped<IAddShiftQuery, AddShiftQuery>();
builder.Services.AddScoped<IGetAllShiftsQuery, GetAllShiftsQuery>();
builder.Services.AddScoped<IGetShiftByIdQuery, GetShiftByIdQuery>();
builder.Services.AddScoped<IGetShiftsByEmployeeQuery, GetShiftsByEmployeeQuery>();
builder.Services.AddScoped<IUpdateShiftQuery, UpdateShiftQuery>();
builder.Services.AddScoped<IGetEmployeeByIdQuery, GetEmployeeByIdQuery>();
builder.Services.AddScoped<IShiftService, ShiftService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

// Register all the swagger examples.
builder.Services.AddSwaggerExamplesFromAssemblyOf<ShiftConflictProblemExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<ShiftAndEmployeeResponseExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<ShiftResponseExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<ShiftWithEmailResponse>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<MultipleShiftResponsesExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<CreateOpenShiftRequestExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<OpenShiftResponseExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<ShiftNotFoundProblemExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<ValidationProblemExample>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Would be good to add ExceptionHandlingMiddleware here to catch exceptions, log them and sanitise before sending response to client.

app.Run();
