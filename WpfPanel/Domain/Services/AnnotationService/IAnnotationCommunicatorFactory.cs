using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfPanel.Domain.Services.AnnotationService.AnnotationReaders;
using WpfPanel.Domain.Services.AnnotationService.AnnotationWriters;

namespace WpfPanel.Domain.Services.AnnotationService
{
    public interface IAnnotationCommunicatorFactory
    {
        IAnnotationReader CreateAnnotationReader();
        IAnnotationWriter CreateAnnotationWriter();
        bool IsFileExists();
    }
}
