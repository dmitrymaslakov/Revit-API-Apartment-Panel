using ApartmentPanel.Utility;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ApartmentPanel.Presentation.View.Components
{
    /// <summary>
    /// Interaction logic for AnnotationPreview.xaml
    /// </summary>
    public partial class AnnotationPreview : UserControl
    {
        public AnnotationPreview() => InitializeComponent();  

        public static readonly DependencyProperty AnnotationProperty =
            DependencyProperty.Register(nameof(Annotation), typeof(BitmapSource),
                typeof(AnnotationPreview), new PropertyMetadata(null));

        public BitmapSource Annotation
        {
            get { return (BitmapSource)GetValue(AnnotationProperty); }
            set { SetValue(AnnotationProperty, value); }
        }

        public static readonly DependencyProperty SetAnnotationPreviewCommandProperty =
            DependencyProperty.Register(nameof(SetAnnotationPreviewCommand), typeof(ICommand),
                typeof(AnnotationPreview), new PropertyMetadata(null));

        public ICommand SetAnnotationPreviewCommand
        {
            get { return (ICommand)GetValue(SetAnnotationPreviewCommandProperty); }
            set { SetValue(SetAnnotationPreviewCommandProperty, value); }
        }

        public static readonly DependencyProperty SetAnnotationToElementCommandProperty =
            DependencyProperty.Register(nameof(SetAnnotationToElementCommand), typeof(ICommand),
                typeof(AnnotationPreview), new PropertyMetadata(null));

        public ICommand SetAnnotationToElementCommand
        {
            get { return (ICommand)GetValue(SetAnnotationToElementCommandProperty); }
            set { SetValue(SetAnnotationToElementCommandProperty, value); }
        }

    }
}
