﻿using Core.Aspects.Autofac.Caching;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Business.Constants;
using Core.Entities.Concrete;

namespace Business.Fakes.Handlers.Translates
{
    public class CreateTranslateInternalCommand : IRequest<IResult>
    {
        public int LangId { get; set; }
        public string Value { get; set; }
        public string Code { get; set; }

        public class CreateTranslateInternalCommandHandler : IRequestHandler<CreateTranslateInternalCommand, IResult>
        {
            private readonly ITranslateRepository _translateRepository;
            private readonly IMediator _mediator;

            public CreateTranslateInternalCommandHandler(ITranslateRepository translateRepository, IMediator mediator)
            {
                _translateRepository = translateRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            public async Task<IResult> Handle(CreateTranslateInternalCommand request, CancellationToken cancellationToken)
            {
                var isThereTranslateRecord = _translateRepository.Query().Any(u => u.LangId == request.LangId && u.Code == request.Code);

                if (isThereTranslateRecord == true)
                {
                    return new ErrorResult(Messages.NameAlreadyExist);
                }

                var addedTranslate = new Translate
                {
                    LangId = request.LangId,
                    Value = request.Value,
                    Code = request.Code,
                };

                _translateRepository.Add(addedTranslate);
                await _translateRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}
