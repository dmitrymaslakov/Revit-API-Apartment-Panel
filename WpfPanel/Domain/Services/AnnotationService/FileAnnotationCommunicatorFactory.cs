using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using WpfPanel.Domain.Services.AnnotationService.AnnotationReaders;
using WpfPanel.Domain.Services.AnnotationService.AnnotationWriters;

namespace WpfPanel.Domain.Services.AnnotationService
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
