using PhoneBook.Application.Commands.Contacts.Create;
using PhoneBook.Application.Commands.Contacts.Delete;
using PhoneBook.Application.Commands.Contacts.Update;
using PhoneBook.Application.Queries.Contacts.GetAll;
using PhoneBook.Application.Queries.Contacts.GetById;
using PhoneBook.Application.Queries.Contacts.GetByTag;
using PhoneBook.Core.Contracts.Contacts.Commands;
using PhoneBook.Core.Contracts.Contacts.Dtos;
using PhoneBook.Core.Contracts.Contacts.Queries;
using PhoneBook.Core.Contracts.Contacts.Requests;

namespace PhoneBook.Endpoints.Api.Contacts;

public static class ContactEndpoints
{
    public static IEndpointRouteBuilder MapContactEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/contacts")
            .WithTags("Contacts");

        group.MapPost(
                "/",
                CreateContact)
            .WithName("CreateContact")
            .WithSummary("Create a new contact.")
            .WithDescription(
                "Creates a new contact in the phone book.")
            .Produces<ContactDto>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        group.MapGet(
                "/",
                GetContacts)
            .WithName("GetContacts")
            .WithSummary("Get contacts.")
            .WithDescription("Returns all contacts or contacts filtered by an exact tag.")
            .Produces<IReadOnlyList<ContactDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        group.MapGet(
                "/{id:guid}",
                GetContactById)
            .WithName("GetContactById")
            .WithSummary("Get a contact by ID.")
            .WithDescription(
                "Returns a single contact identified by its unique ID.")
            .Produces<ContactDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        group.MapPut(
                "/{id:guid}",
                UpdateContact)
            .WithName("UpdateContact")
            .WithSummary("Update a contact.")
            .WithDescription(
                "Replaces the mutable data of an existing contact.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        group.MapDelete(
                "/{id:guid}",
                DeleteContact)
            .WithName("DeleteContact")
            .WithSummary("Delete a contact.")
            .WithDescription(
                "Deletes an existing contact from the phone book.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return endpoints;
    }

    private static IResult CreateContact(
        CreateContactRequest request,
        CreateContactCommandHandler handler)
    {
        var command = new CreateContactCommand(
            request.FirstName,
            request.LastName,
            request.PhoneNumber,
            request.Tag);

        var contact = handler.Handle(command);

        return TypedResults.CreatedAtRoute(
            contact,
            "GetContactById",
            new { id = contact.Id });
    }

    private static IResult GetContacts(
        string? tag,
        GetAllContactsQueryHandler getAllHandler,
        GetContactsByTagQueryHandler getByTagHandler)
    {
        if (tag is null)
        {
            var contacts = getAllHandler.Handle(
                new GetAllContactsQuery());

            return TypedResults.Ok(contacts);
        }

        var filteredContacts = getByTagHandler.Handle(
            new GetContactsByTagQuery(tag));

        return TypedResults.Ok(filteredContacts);
    }

    private static IResult GetContactById(
        Guid id,
        GetContactByIdQueryHandler handler)
    {
        var contact =
            handler.Handle(
            new GetContactByIdQuery(id));

        return contact is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(contact);
    }

    private static IResult UpdateContact(
        Guid id,
        UpdateContactRequest request,
        UpdateContactCommandHandler handler)
    {
        var command = new UpdateContactCommand(
            id,
            request.FirstName,
            request.LastName,
            request.PhoneNumber,
            request.Tag);

        handler.Handle(command);

        return TypedResults.NoContent();
    }

    private static IResult DeleteContact(
        Guid id,
        DeleteContactCommandHandler handler)
    {
        handler.Handle(
            new DeleteContactCommand(id));

        return TypedResults.NoContent();
    }
}