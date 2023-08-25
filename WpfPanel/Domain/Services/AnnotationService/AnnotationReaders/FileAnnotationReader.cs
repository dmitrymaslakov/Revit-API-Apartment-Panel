using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Media.Imaging;

namespace WpfPanel.Domain.Services.AnnotationService.AnnotationReaders
{
    public class FileAnnotationReader : IDisposable, IAnnotationReader
    {
        private ImageSource _annotation;
        private bool _disposed;

        public FileAnnotationReader(string fullPath)
        {
            try
            {
                var uriSource = new Uri(fullPath);
                _annotation = new BitmapImage(uriSource); 
            }
            catch (FileNotFoundException)
            {
                _annotation = null;
            }
        }

        public ImageSource Get()
        {
            if (_disposed)
                throw new Exception("Object is disposed.");

            return _annotation;
        }

        public void Dispose()
        {
            _disposed = true;
            _annotation = null;
        }
    }
}