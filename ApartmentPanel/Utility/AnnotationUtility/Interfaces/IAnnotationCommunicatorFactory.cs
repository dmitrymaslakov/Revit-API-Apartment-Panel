namespace ApartmentPanel.Utility.AnnotationUtility.Interfaces
{
    public interface IAnnotationCommunicatorFactory
    {
        IAnnotationReader CreateAnnotationReader();
        IAnnotationWriter CreateAnnotationWriter();
        bool IsAnnotationExists();
    }
}
