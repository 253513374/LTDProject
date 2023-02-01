using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weitedianlan.Model.Entity;

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