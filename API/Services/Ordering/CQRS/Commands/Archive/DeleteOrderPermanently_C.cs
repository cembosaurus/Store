using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using FluentValidation;
using MediatR;
using Ordering.Services.Interfaces;

namespace Ordering.CQRS.Commands.Archive
{
    public class DeleteOrderPermanently_C : IRequest<IServiceResult<IEnumerable<Guid>>>
    {
        public int UserId { get; set; }


        public class Validator : AbstractValidator<DeleteOrderPermanently_C>
        {
            public Validator()
            {
                RuleFor(x => x)
                    .NotNull()
                    .WithMessage("- Delete Order Permanently command model must NOT be NULL !");
                When(x => x != null, () => {
                    RuleFor(x => x.UserId)
                    .GreaterThan(0)
                    .WithMessage("- User Id must be greater than 0 !");
                });
            }
        }


        public class DeleteOrderPermanently_H : IRequestHandler<DeleteOrderPermanently_C, IServiceResult<IEnumerable<Guid>>>
        {
            private readonly IArchiveService _archiveService;

            public DeleteOrderPermanently_H(IArchiveService archiveService)
            {
                _archiveService = archiveService;
            }


            public async Task<IServiceResult<IEnumerable<Guid>>> Handle(DeleteOrderPermanently_C request, CancellationToken cancellationToken)
            {
                var result = await _archiveService.DeleteOrdersPermanently(request.UserId);

                return result;
            }
        }
    }
}
