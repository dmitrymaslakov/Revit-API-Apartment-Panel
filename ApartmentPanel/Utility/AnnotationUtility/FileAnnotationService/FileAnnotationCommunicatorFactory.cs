using System.IO;
using System.Text;
using ApartmentPanel.Utility.AnnotationUtility.Interfaces;

namespace ApartmentPanel.Utility.AnnotationUtility.FileAnnotationService
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

        public bool IsAnnotationExists() => File.Exists(_fullPath);

        public IAnnotationReader CreateAnnotationReader() => new FileAnnotationReader(_fullPath);

        public IAnnotationWriter CreateAnnotationWriter() => new FileAnnotationWriter(_fullPath);
    }
}
