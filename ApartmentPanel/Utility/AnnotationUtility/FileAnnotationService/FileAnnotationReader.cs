using System;
using System.Windows.Media.Imaging;
using System.IO;
using ApartmentPanel.Utility.AnnotationUtility.Interfaces;

namespace ApartmentPanel.Utility.AnnotationUtility.FileAnnotationService
{
    public class FileAnnotationReader : IDisposable, IAnnotationReader
    {
        private BitmapImage _annotation;
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

        public BitmapImage Get()
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

        private BitmapImage BitmapFromUri(Uri source)
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
