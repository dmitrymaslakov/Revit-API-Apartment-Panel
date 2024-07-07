using System.IO;
using ApartmentPanel.Utility.AnnotationUtility.Interfaces;

namespace ApartmentPanel.Utility.AnnotationUtility.FileAnnotationService
{
    public class FileAnnotationCommunicatorFactory : IAnnotationCommunicatorFactory
    {
        private readonly string _fullPath;

        public FileAnnotationCommunicatorFactory(string fullPath)
        {
            /*_fullPath = new StringBuilder(FileUtility.GetApplicationAnnotationsPath())
                .Append("\\")
                .Append(fileName)
                .Append(".png")
                .ToString();*/
            _fullPath = fullPath;
        }

        public bool IsAnnotationExists() => File.Exists(_fullPath);
        public IAnnotationReader CreateAnnotationReader() => new FileAnnotationReader(_fullPath);
        public IAnnotationWriter CreateAnnotationWriter() => new FileAnnotationWriter(_fullPath);
    }
}
