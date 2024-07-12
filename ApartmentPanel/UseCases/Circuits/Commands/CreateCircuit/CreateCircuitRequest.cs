using ApartmentPanel.Core.Models;
using MediatR;

namespace ApartmentPanel.UseCases.Circuits.Commands.CreateCircuit
{
    public class CreateCircuitRequest : IRequest<Circuit>
    {
        public string CircuitNumber { get; set; }
    }
}
