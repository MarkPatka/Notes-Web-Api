using Notes.Application.Common.Exceptions;
using Notes.Application.Notes.Commands.CreateNote;
using Notes.Application.Notes.Commands.DeleteCommand;
using Notes.Application.Notes.Commands.DeleteNote;
using Notes.Tests.Common;

namespace Notes.Tests.Notes.Commands;

public class DeleteNoteCommandHandlerContext : TestCommandBase
{
    [Fact]
    public async Task DeleteNoteCommandHandler_Success()
    {
        // Arrange
        var handler = new DeleteNoteCommandHandler(Context);

        // Act
        await handler.Handle(new DeleteNoteCommand
        {
            Id = NotesContextFactory.NoteIdForDelete,
            UserId = NotesContextFactory.UserAId,
        }, 
        CancellationToken.None);

        // Assert
        Assert.Null(Context.Notes.SingleOrDefault(note =>
            note.Id == NotesContextFactory.NoteIdForDelete));
    }

    [Fact]
    public async Task DeleteNoteCommandHandler_FailOnWrongId()
    {
        // Arrange
        var handler = new DeleteNoteCommandHandler(Context);

        // Act

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await handler.Handle(
                new DeleteNoteCommand
                {
                    Id = Guid.NewGuid(),
                    UserId = NotesContextFactory.UserAId,
                },
                CancellationToken.None));
    }

    [Fact]
    public async Task DeleteNoteCommandHandler_failOnWrongUserId()
    {
        // Arrange
        var deleteHandler = new DeleteNoteCommandHandler(Context);
        var createHandler = new CreateNoteCommandHandler(Context);

        // Act
        var noteId = await createHandler.Handle(
            new CreateNoteCommand
            {
                Title = "NoteTitle",
                UserId = NotesContextFactory.UserAId
            },
            CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await deleteHandler.Handle(
                new DeleteNoteCommand
                {
                    Id = noteId,
                    UserId = NotesContextFactory.UserBId,
                },
                CancellationToken.None));

    }
}
