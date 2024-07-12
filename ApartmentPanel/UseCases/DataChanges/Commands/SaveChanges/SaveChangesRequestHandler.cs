using ApartmentPanel.Core.Models;
using ApartmentPanel.Infrastructure.Interfaces.DataAccess;
using ApartmentPanel.UseCases.ApartmentElements.Commands.Circuits;
using ApartmentPanel.Utility;
using ApartmentPanel.Utility.AnnotationUtility;
using ApartmentPanel.Utility.AnnotationUtility.FileAnnotationService;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ApartmentPanel.UseCases.DataChanges.Commands.SaveChanges
{
    public class SaveChangesRequestHandler
        : IRequestHandler<SaveChangesRequest, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SaveChangesRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(SaveChangesRequest request, CancellationToken cancellationToken)
        {
            await Task.Delay(0);
            _unitOfWork.SaveChanges();
            return isDeleted;
        }
    }
}
