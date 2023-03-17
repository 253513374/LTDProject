using MediatR;
using Wtdl.Model.Entity;

namespace Wtdl.Repository.Tools
{
    public class FileRecordIRquest : IRequestHandler<FileUploadRecord, string>
    {
        public Task<string> Handle(FileUploadRecord request, CancellationToken cancellationToken)
        {
            // Twiddle thumbs
            return Task.FromResult("Pong");
        }
    }
}