using MediatR;
using ScanCode.Model.Entity;

namespace ScanCode.Repository.Tools
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