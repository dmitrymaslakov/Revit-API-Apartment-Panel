using ApartmentPanel.Domain.Services.AnnotationService.AnnotationReaders;
using ApartmentPanel.Domain.Services.AnnotationService.AnnotationWriters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentPanel.Domain.Services.AnnotationService
{
    public interface IAnnotationCommunicatorFactory
    {
        IAnnotationReader CreateAnnotationReader();
        IAnnotationWriter CreateAnnotationWriter();
        bool IsFileExists();
    }
}
