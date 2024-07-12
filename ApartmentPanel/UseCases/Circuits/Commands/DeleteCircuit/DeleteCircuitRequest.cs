using ApartmentPanel.Core.Models;
using MediatR;

namespace ApartmentPanel.UseCases.Circuits.Commands.DeleteCircuit
{
    public class DeleteCircuitRequest : IRequest<bool>
    {
        public string CircuitNumber { get; set; }
    }
}
