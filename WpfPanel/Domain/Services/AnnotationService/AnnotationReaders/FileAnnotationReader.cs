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
                _annotation = BitmapFromUri(new Uri(fullPath));
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

        private ImageSource BitmapFromUri(Uri source)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = source;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            return bitmap;
        }
    }
}