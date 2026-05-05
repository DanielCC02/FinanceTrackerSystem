using FluentValidation;
using MediatR;

namespace FinanceTracker.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            // Si no hay validators para este command/query, deja pasar
            if (!_validators.Any())
                return await next();

            // Ejecutar todos los validators
            var context = new ValidationContext<TRequest>(request);

            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(error => error != null)
                .ToList();

            // Si hay errores, lanza excepción antes de llegar al handler
            if (failures.Any())
                throw new ValidationException(failures);

            // Todo OK, continúa al handler
            return await next();
        }
    }
}