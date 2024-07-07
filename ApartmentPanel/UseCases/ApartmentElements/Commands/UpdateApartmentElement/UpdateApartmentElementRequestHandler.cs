using ApartmentPanel.Core.Models;
using ApartmentPanel.Infrastructure.Interfaces.DataAccess;
using ApartmentPanel.UseCases.ApartmentElements.Dto;
using MediatR;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApartmentPanel.UseCases.ApartmentElements.Commands.UpdateApartmentElement
{
    public class UpdateApartmentElementRequestHandler
        : IRequestHandler<UpdateApartmentElementRequest, ApartmentElement>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateApartmentElementRequestHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<ApartmentElement> Handle(UpdateApartmentElementRequest request, CancellationToken cancellationToken)
        {
            await Task.Delay(0);
            ApartmentElementUpdateDto updateDto = request.Dto;
            var foundElement = _unitOfWork.ApartmentElementRepository
                .FindBy(ae => string.Equals(ae.Name, updateDto.Name))
                .FirstOrDefault();

            if (foundElement == null) { return null; }

            foundElement.Name = updateDto.Name;
            foundElement.Category = updateDto.Category;
            foundElement.Family = updateDto.Family;
            foundElement.Annotation = updateDto.Annotation;
            UpdateHeight(foundElement.MountingHeight, updateDto.MountingHeight);
            UpdateParameters(foundElement.Parameters, updateDto.Parameters);

            return foundElement;
        }

        private void UpdateHeight(Height heightForUpdate, Height heightSource)
        {
            heightForUpdate.TypeOf = heightSource.TypeOf;
            heightForUpdate.FromFloor = heightSource.FromFloor;
            heightForUpdate.FromLevel = heightSource.FromLevel;
            heightForUpdate.ResponsibleForHeightParameter = heightSource.ResponsibleForHeightParameter;
        }
        private void UpdateParameters(ObservableCollection<Parameter> parametersForUpdate,
            ICollection<Parameter> parametersSource)
        {
            for (int i = 0; i < parametersForUpdate.Count; i++)
            {
                var parameterForUpdate = parametersForUpdate[i];
                var parameterSource = parametersSource
                    .Where(p => Equals(p.Id, parameterForUpdate.Id))
                    .FirstOrDefault();
                if (parameterSource == null) continue;
                parameterForUpdate.Name = parameterSource.Name;
                parameterForUpdate.Value = parameterSource.Value;
            }
        }
    }
}
