﻿using System.Threading;
using Fractural.Tasks;

namespace TanitakaTech.GodotProcessManager
{
    public class RequestHandler<TRequest> :
        IRequestPusher<TRequest>,
        IRequestConsumer<TRequest> where TRequest : struct
    {
        private TRequest? _requestValue = null;
        private bool _isWaitingRequest = false;

        void IRequestPusher<TRequest>.PushRequest(TRequest requestValue)
        {
            if (!_isWaitingRequest) return;
            
            _requestValue = requestValue;
        }

        async GDTask<TRequest> IRequestConsumer<TRequest>.WaitRequestAndConsumeAsync(
            CancellationToken cancellationToken)
        {
            _isWaitingRequest = true;
            cancellationToken.Register(() => _isWaitingRequest = false);
            await GDTask.WaitUntil(() => _requestValue != null, cancellationToken: cancellationToken);
            TRequest requestedValue = _requestValue.Value;
            _requestValue = null;
            _isWaitingRequest = false;
            return requestedValue;
        }
    }
}