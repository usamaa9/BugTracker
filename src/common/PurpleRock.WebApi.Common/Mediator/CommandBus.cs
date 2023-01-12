using MediatR;
using Microsoft.AspNetCore.Http;
using PurpleRock.Application.Common.Commands;
using PurpleRock.Application.Common.Models;

namespace PurpleRock.WebApi.Common.Mediator;

/// <inheritdoc cref="ICommandBus"/>
public class CommandBus : ICommandBus
{
  private readonly IMediator _mediator;

  /// <summary>
  /// Initializes a new instance of the <see cref="CommandBus"/> class.
  /// </summary>
  /// <param name="mediator"></param>
  public CommandBus(IMediator mediator)
  {
    _mediator = mediator;
  }

  /// <inheritdoc/>
  public async Task<dynamic> SendAsync<TCommand, T>(
      TCommand command, CommandMetadata metadata)
      where TCommand : IRequest<Result<T>>
  {
    if (metadata.Context is not HttpContext)
    {
      throw new InvalidOperationException($"CommandMetadata Context is not of type {nameof(HttpContext)}");
    }

    var mediatorResponse = await _mediator.Send(command);

    return mediatorResponse;
  }
}