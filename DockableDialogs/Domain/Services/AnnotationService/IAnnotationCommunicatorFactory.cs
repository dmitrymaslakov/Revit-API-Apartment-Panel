using DockableDialogs.Domain.Services.AnnotationService.AnnotationReaders;
using DockableDialogs.Domain.Services.AnnotationService.AnnotationWriters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockableDialogs.Domain.Services.AnnotationService
{
    public interface IAnnotationCommunicatorFactory
    {
        IAnnotationReader CreateAnnotationReader();
        IAnnotationWriter CreateAnnotationWriter();
        bool IsFileExists();
    }
}
