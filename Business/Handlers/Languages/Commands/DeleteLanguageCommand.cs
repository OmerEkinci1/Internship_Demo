﻿using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.Languages.Commands
{
	public class DeleteLanguageCommand : IRequest<IResult>
	{
		public int Id { get; set; }

		public class DeleteLanguageCommandHandler : IRequestHandler<DeleteLanguageCommand, IResult>
		{
			private readonly ILanguageRepository _languageRepository;
			private readonly IMediator _mediator;

			public DeleteLanguageCommandHandler(ILanguageRepository languageRepository, IMediator mediator)
			{
				_languageRepository = languageRepository;
				_mediator = mediator;
			}
			[CacheRemoveAspect("Get")]
			[LogAspect(typeof(FileLogger))]
			public async Task<IResult> Handle(DeleteLanguageCommand request, CancellationToken cancellationToken)
			{
				var languageToDelete = _languageRepository.Get(p => p.Id == request.Id);

				_languageRepository.Delete(languageToDelete);
				await _languageRepository.SaveChangesAsync();
				return new SuccessResult(Messages.Deleted);
			}
		}
	}
}
