using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DockableDialogs.Domain.Services.AnnotationService.AnnotationWriters
{
    public interface IAnnotationWriter : IDisposable
    {
        BitmapSource Save(BitmapSource annotation);
    }
}
