using DockableDialogs.Domain.Services.AnnotationService.AnnotationReaders;
using DockableDialogs.Domain.Services.AnnotationService.AnnotationWriters;
using System;
using System.IO;

namespace DockableDialogs.Domain.Services.AnnotationService
{
    public class FileAnnotationCommunicatorFactory : IAnnotationCommunicatorFactory
    {
        private readonly string _fullPath;

        public FileAnnotationCommunicatorFactory(string fileName)
        {
            _fullPath = Path.Combine(Environment.CurrentDirectory,
                "Resources", "Annotations", fileName + ".png");
        }

        public bool IsFileExists() => File.Exists(_fullPath);

        public IAnnotationReader CreateAnnotationReader() => new FileAnnotationReader(_fullPath);

        public IAnnotationWriter CreateAnnotationWriter() => new FileAnnotationWriter(_fullPath);
    }
}
