using Accenture.BusinessEntities;
using Accenture.BusinessRulesInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Accenture.BusinessEntities.LogInfo;

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
		/// <response code = "500"> Erro na consulta</response>
		[Route("ImportLogFile")]
		[HttpPost]
		public async Task<IActionResult> ImportLogFile(IFormFile file)
		{
			try
			{
				if (file.Length <= 0)
				return BadRequest("Empty file");

				//Strip out any path specifiers (ex: /../)
				var originalFileName = Path.GetFileName(file.FileName);

				//Create a unique file path
				var uniqueFileName = Path.GetRandomFileName();
				var uniqueFilePath = Path.Combine(@"C:\temp\", Path.GetFileNameWithoutExtension(uniqueFileName) + Path.GetExtension(originalFileName));

				//Save the file to disk
				using (var stream = System.IO.File.Create(uniqueFilePath))
				{
					await file.CopyToAsync(stream);
				}

				var quantity = this.LogBusinessRules.SaveLogInfoFromFile(uniqueFilePath);

				return Ok($"Foram importados {quantity} registros.");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// POST /Log/GetResumeLogInfo
		/// <summary>
		/// Retorna os tipos de logs que foram tipificados para realizar o filtro
		/// </summary>
		/// <param name="value"></param>
		/// <returns> Retorna os tipos de logs que foram tipificados para realizar o filtro </returns>
		/// <response code = "200"> Lista de tipos de logs </response>
		/// <response code = "500"> Erro na consulta</response>
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
				return BadRequest(ex.Message);
			}
		}

		// POST /Log/GetLogInfo
		/// <summary>
		/// Filtra os logs importados
		/// </summary>
		/// <param name="value"></param>
		/// <returns> Filtra os logs importados </returns>
		/// <response code = "200"> Lista de logs paginados</response>
		/// <response code = "500"> Erro na consulta</response>

		[Route("GetLogInfo")]
		[HttpGet]
		public ActionResult<LogInfosPaginationResponse> GetLogInfo(string LogIdentification = null, int? LogTypeId = null, string LogIp = null, string Description = null, int Page = 1, int RowsPerpage = 20)
		{
            try
            {
				if(RowsPerpage > 1000)
					return BadRequest("Maxmimo permitido de resposta são 1000 linhas.");
				else if(RowsPerpage < 1)
					return BadRequest("O minimo de linhas é 1.");

				if (Page <= 0)
					return BadRequest("A pagina deve ser válida.");


				var result = this.LogBusinessRules.GetLogInfosPagination(new LogInfosPaginationRequest() {
					LogIdentification = LogIdentification,
					LogTypeId = LogTypeId,
					Description = Description,
					LogIp = LogIp,

					Page = Page,
					RowsPerpage = RowsPerpage
				});

				return Ok(result);
			}
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
		}

		// POST /Log/GetResumeLogInfo
		/// <summary>
		/// Resumo dos logs com porcentagem de registros
		/// </summary>
		/// <param name="value"></param>
		/// <returns> Resumo dos logs com porcentagem de registros </returns>
		/// <response code = "200"> Agrupagmento de logs com porcentagem </response>
		/// <response code = "500"> Erro na consulta</response>
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
				return BadRequest(ex.Message);
			}
		}
	}
}
