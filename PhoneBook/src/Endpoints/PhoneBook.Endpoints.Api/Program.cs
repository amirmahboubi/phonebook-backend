using PhoneBook.Endpoints.Api.Contacts;
using PhoneBook.Endpoints.Api.Infrastructure;
using PhoneBook.Application.Queries.Contacts.GetAll;
using PhoneBook.Application.Queries.Contacts.GetById;
using PhoneBook.Application.Queries.Contacts.GetByTag;
using PhoneBook.Application.Commands.Contacts.Create;
using PhoneBook.Application.Commands.Contacts.Update;
using PhoneBook.Application.Commands.Contacts.Delete;
using PhoneBook.Infrastructure.Data.InMemory.Context;
using PhoneBook.Infrastructure.Data.InMemory.Contacts.Repositories;
using PhoneBook.Core.Contracts.Contacts.Abstractions.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddSingleton<InMemoryDataContext>(_ =>
{
    var context = new InMemoryDataContext();

    context.Seed();

    return context;
});

builder.Services.AddSingleton<
    IContactCommandRepository,
    ContactCommandRepository>();

builder.Services.AddSingleton<
    IContactQueryRepository,
    ContactQueryRepository>();

builder.Services.AddSingleton<CreateContactCommandValidator>();
builder.Services.AddSingleton<UpdateContactCommandValidator>();
builder.Services.AddSingleton<DeleteContactCommandValidator>();

builder.Services.AddScoped<CreateContactCommandHandler>();
builder.Services.AddScoped<UpdateContactCommandHandler>();
builder.Services.AddScoped<DeleteContactCommandHandler>();

builder.Services.AddSingleton<GetContactByIdQueryValidator>();
builder.Services.AddSingleton<GetContactsByTagQueryValidator>();

builder.Services.AddSingleton<GetAllContactsQueryHandler>();
builder.Services.AddScoped<GetContactByIdQueryHandler>();
builder.Services.AddScoped<GetContactsByTagQueryHandler>();

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/openapi/v1.json",
            "PhoneBook API v1");
    });
}

app.MapContactEndpoints();

app.Run();

public partial class Program;