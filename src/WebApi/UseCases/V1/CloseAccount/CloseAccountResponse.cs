using System.ComponentModel.DataAnnotations;
using AmaniRobot.Application.Boundaries.CloseAccount;

namespace AmaniRobot.WebApi.UseCases.V1.CloseAccount;
/// <summary>
/// Close Account Response
/// </summary>
public sealed class CloseAccountResponse
{
    /// <summary>
    /// Account ID
    /// </summary>
    [Required]
    public Guid AccountId { get; }

    public CloseAccountResponse(CloseAccountOutput output)
    {
        AccountId = output.AccountId;
    }
}