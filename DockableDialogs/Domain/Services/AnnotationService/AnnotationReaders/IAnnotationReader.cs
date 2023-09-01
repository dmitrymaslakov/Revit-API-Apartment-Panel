using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DockableDialogs.Domain.Services.AnnotationService.AnnotationReaders
{
    public interface IAnnotationReader : IDisposable
    {
        ImageSource Get();
    }
}
