using MediatR;

namespace Notes.Application.Notes.Commands.CreateNote
{
    public class CreateNoteCommand : IRequest<Guid>
    {
        public Guid USerId { get; set; }
        public string Title { get; set;} = string.Empty;
        public string Details { get; set; } = string.Empty;
    }
}
