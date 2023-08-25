﻿using System;
using System.IO;
using System.Text.Json;
using System.Windows.Annotations;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfPanel.Domain.Services.AnnotationService.AnnotationWriters
{
    public class FileAnnotationWriter : IDisposable, IAnnotationWriter
    {
        private bool _disposed;
        private readonly string _fileName;

        public FileAnnotationWriter(string fileName) => _fileName = fileName;

        public BitmapSource Save(BitmapSource annotation)
        {
            if (_disposed)
                throw new Exception("Object is disposed.");

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