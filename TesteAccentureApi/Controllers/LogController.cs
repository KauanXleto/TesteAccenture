using Accenture.BusinessEntities;
using Accenture.BusinessRulesInterface;
using Accenture.Commun.TextHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Accenture.BusinessEntities.LogInfo;
using static Accenture.BusinessEntities.LogType;

namespace TesteAccentureApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LogController : ControllerBase
    {
        ILogBusinessRules LogBusinessRules { get; set; }

        private readonly ILogger<LogController> _logger;

        public LogController(ILogger<LogController> logger, ILogBusinessRules _LogBusinessRules)
        {
            _logger = logger;
            this.LogBusinessRules = _LogBusinessRules;
        }

		// POST /Log/ImportLogFile
		/// <summary>
		/// Sava os logs presentes do arquivo enviado
		/// </summary>
		/// <param name="value"></param>
		/// <returns> Informação de quantos registros foram inseridos </returns>
		/// <response code = "200"> Quantidade de registros inseridos</response>
		/// <response code = "400"> Erro no cadastro</response>
		/// <response code = "500"> Erro interno</response>
		/// <response code = "429"> Limite de requisições atingido</response>
		[Route("ImportLogFile")]
		[HttpPost]
		public async Task<IActionResult> ImportLogFile(IFormFile file)
		{
			try
			{
				if (file == null || (file != null && file.Length <= 0))
					return BadRequest("Deve ser informado enviado um arquivo.");

				if (!StringCompare.CompareString(Path.GetExtension(file.FileName), ".log"))
					return BadRequest("Erro!! Extensão do arquivo deve ser do tipo 'log'");

				var originalFileName = Path.GetFileName(file.FileName);

				var uniqueFileName = Path.GetRandomFileName();
				var uniqueFilePath = Path.Combine(@"C:\Windows\Temp\", Path.GetFileNameWithoutExtension(uniqueFileName) + Path.GetExtension(originalFileName));

				using (var stream = System.IO.File.Create(uniqueFilePath))
				{
					await file.CopyToAsync(stream);
				}

				if (string.IsNullOrWhiteSpace(uniqueFilePath) || uniqueFilePath.Length < 3)
					return BadRequest("Erro!! É necessário um caminho válido do arquivo");

				var quantity = this.LogBusinessRules.SaveLogInfoFromFile(uniqueFilePath);

				return Ok($"Foram importados {quantity} registros.");
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		// POST /Log/GetResumeLogInfo
		/// <summary>
		/// Retorna os tipos de logs que foram tipificados para realizar o filtro
		/// </summary>
		/// <param name="value"></param>
		/// <returns> Retorna os tipos de logs que foram tipificados para realizar o filtro </returns>
		/// <response code = "200"> Lista de tipos de logs </response>
		/// <response code = "400"> Erro na consulta</response>
		/// <response code = "500"> Erro interno</response>
		/// <response code = "429"> Limite de requisições atingido</response>
		/// 
		[Route("GetLogTypes")]
		[HttpGet]
		public ActionResult<List<LogType>> GetLogTypes()
		{
			try
			{
				var result = this.LogBusinessRules.GetLogTypes();

				return Ok(result);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		// POST /Log/GetLogInfo
		/// <summary>
		/// Filtra os logs importados
		/// </summary>
		/// <param name="value"></param>
		/// <returns> Filtra os logs importados </returns>
		/// <response code = "200"> Lista de logs paginados</response>
		/// <response code = "400"> Erro na consulta</response>
		/// <response code = "500"> Erro interno</response>
		/// <response code = "429"> Limite de requisições atingido</response>

		[Route("GetLogInfo")]
		[HttpGet]
		public ActionResult<LogInfosPaginationResponse> GetLogInfo(ELogType? LogTypeId = null, string LogDate = null, string LogIdentification = null, string LogIp = null, string Description = null, int Page = 1, int RowsPerpage = 20)
		{
            try
            {
				if(RowsPerpage > 1000)
					return BadRequest("Maxmimo permitido de resposta são 1000 linhas.");
				else if(RowsPerpage < 1)
					return BadRequest("O minimo de linhas é 1.");

				if (Page <= 0)
					return BadRequest("A pagina deve ser válida.");


				var result = this.LogBusinessRules.GetLogInfosPagination(new LogInfosPaginationRequest()
				{
					LogDate = LogDate,
					LogIdentification = LogIdentification,
					LogTypeId = (int?)LogTypeId,
					Description = Description,
					LogIp = LogIp,

					Page = Page,
					RowsPerpage = RowsPerpage
				});

				return Ok(result);
			}
            catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		// POST /Log/GetResumeLogInfo
		/// <summary>
		/// Resumo dos logs com porcentagem de registros
		/// </summary>
		/// <param name="value"></param>
		/// <returns> Resumo dos logs com porcentagem de registros </returns>
		/// <response code = "200"> Agrupagmento de logs com porcentagem </response>
		/// <response code = "400"> Erro na consulta</response>
		/// <response code = "500"> Erro interno</response>
		/// <response code = "429"> Limite de requisições atingido</response>
		/// 
		[Route("GetResumeLogInfo")]
		[HttpGet]
		public ActionResult<ResumeLogInfo> GetResumeLogInfo()
		{
			try
			{
				var result = this.LogBusinessRules.GetResumeLogInfo();

				return Ok(result);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
	}
}
