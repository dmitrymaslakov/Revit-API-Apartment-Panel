﻿using ApartmentPanel.Utility.AnnotationUtility.Interfaces;
using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace ApartmentPanel.Utility.AnnotationUtility.FileAnnotationService
{
    public class FileAnnotationWriter : IDisposable, IAnnotationWriter
    {
        private bool _disposed;
        private readonly string _fileName;

        public FileAnnotationWriter(string fileName) => _fileName = fileName;

        //public BitmapSource Save(BitmapSource annotation)
        public BitmapImage Save(BitmapImage annotation)
        {
            if (_disposed)
                throw new Exception("Object is disposed.");

            var folderPath = FileUtility.GetApplicationAnnotationsPath();

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            using (var fileStream = new FileStream(_fileName, FileMode.Create))
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(annotation));
                encoder.Save(fileStream);
            }
            return annotation;
        }

        public void Dispose() => _disposed = true;
    }
}
