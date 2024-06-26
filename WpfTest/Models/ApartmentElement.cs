﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace WpfTest.Models
{
    public class ApartmentElement
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Name { get; set; }
        public string Category { get; set; }
        public string Family { get; set; }
        private ImageSource _annotation;
        public ImageSource Annotation
        {
            get => _annotation;
            set => Set(ref _annotation, value);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public ApartmentElement Clone()
        {
            return new ApartmentElement
            {
                Name = Name,
                Category = Category,
                Annotation = Annotation?.Clone()
            };
        }
    }
}
