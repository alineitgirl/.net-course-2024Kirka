using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.App.Validators;
using BankSystem.Data.DbContext;
using BankSystem.Data.Storages;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateSlimBuilder(args);


builder.Services.ConfigureHttpJsonOptions(options => { });


builder.Services.AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ClientDtoValidator>())
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<EmployeeDtoValidator>());
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<ClientService>();
builder.Services.AddScoped<EmployeeService>();
builder.Services.AddScoped<IClientStorage, ClientStorage>();
builder.Services.AddScoped<IEmployeeStorage, EmployeeStorage>();
builder.Services.AddSingleton<BankSystemDbContext>();
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddFluentValidation(
    options => options.RegisterValidatorsFromAssemblyContaining<Program>()
    );
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "Bank API", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors("AllowSpecificOrigin");

app.Run();