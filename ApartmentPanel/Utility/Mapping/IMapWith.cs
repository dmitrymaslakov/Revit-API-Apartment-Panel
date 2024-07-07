using AutoMapper;

namespace ApartmentPanel.Utility.Mapping
{
    public interface IMapWith<T>
    {
        void Mapping(Profile profile);
    }
}
