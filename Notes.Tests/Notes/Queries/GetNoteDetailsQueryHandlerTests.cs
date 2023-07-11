using AutoMapper;
using Notes.Application.Notes.Queries.GetNoteDetails;
using Notes.Persistance;
using Notes.Tests.Common;
using Shouldly;

namespace Notes.Tests.Notes.Queries;

[Collection("QueryCollection")]
public class GetNoteDetailsQueryHandlerTests
{
    private readonly NotesDbContext Context;
    private readonly IMapper Mapper;

    public GetNoteDetailsQueryHandlerTests(QueryTestFixture fixture)
    {
        Context = fixture.Context;
        Mapper = fixture.Mapper;
    }

    [Fact]
    public async Task GetNoteDetailsQueryHandler_Success()
    {
        // Arrange
        var handler = new GetNoteDetailsQueryHandler(Context, Mapper);

        // Act
        var result = await handler.Handle(
            new GetNoteDetailsQuery
            {
                UserId = NotesContextFactory.UserBId,
                Id = Guid.Parse("AD560BED-85D6-4FD7-8EA8-6BA3C82F4CA6")
            },
            CancellationToken.None);

        // Assert
        result.ShouldBeOfType<NoteDetailsVm>();
        result.Title.ShouldBe("Title2");
        result.CreationDate.ShouldBe(DateTime.Today);
    }
}
