using DockableDialogs.Domain.Services.AnnotationService.AnnotationReaders;
using DockableDialogs.Domain.Services.AnnotationService.AnnotationWriters;
using DockableDialogs.Utility;
using System.IO;
using System.Text;

namespace DockableDialogs.Domain.Services.AnnotationService
{
    public class FileAnnotationCommunicatorFactory : IAnnotationCommunicatorFactory
    {
        private readonly string _fullPath;

        public FileAnnotationCommunicatorFactory(string fileName)
        {
            _fullPath = new StringBuilder(FileUtility.GetApplicationAnnotationsPath())
                .Append("\\")
                .Append(fileName)
                .Append(".png")
                .ToString();
        }

        public bool IsFileExists() => File.Exists(_fullPath);

        public IAnnotationReader CreateAnnotationReader() => new FileAnnotationReader(_fullPath);

        public IAnnotationWriter CreateAnnotationWriter() => new FileAnnotationWriter(_fullPath);
    }
}
