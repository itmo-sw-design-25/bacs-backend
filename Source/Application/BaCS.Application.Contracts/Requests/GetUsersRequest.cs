namespace BaCS.Application.Contracts.Requests;

using System.ComponentModel;
using FluentValidation;

public record GetUsersRequest(
    [property: Description("Фильтр по ID пользователей.")]
    Guid[] Ids,
    int Offset = 0,
    int Limit = 10
);

public class GetUsersRequestValidator : AbstractValidator<GetUsersRequest>
{
    public GetUsersRequestValidator()
    {
        RuleFor(x => x.Offset).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Limit).InclusiveBetween(0, 100);
    }
}
