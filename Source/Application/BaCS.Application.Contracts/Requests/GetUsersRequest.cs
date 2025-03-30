namespace BaCS.Application.Contracts.Requests;

using System.ComponentModel;
using FluentValidation;

public record GetUsersRequest(
    [property: Description("Фильтр по ID пользователей.")]
    Guid[] Ids,
    int Skip = 0,
    int Take = 10
);

public class GetUsersRequestValidator : AbstractValidator<GetUsersRequest>
{
    public GetUsersRequestValidator()
    {
        RuleFor(x => x.Skip).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Take).GreaterThanOrEqualTo(0);
    }
}
