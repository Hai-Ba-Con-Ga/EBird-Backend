using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Response;

namespace EBird.Api.Controllers
{

  [ApiController]
  [Route("generate")]
  public class GenerateDataController : ControllerBase
  {
    private readonly IWebHostEnvironment _env;

    public GenerateDataController(IWebHostEnvironment env)
    {
      _env = env;
    }
    [HttpPost("room")]
    public ActionResult<Response<string>> GenerateRoomData()
    {
      try
      {
        //action to do  here
        return Ok();
      }
      catch (Exception ex)
      {
        return BadRequest(new Response<string> { Data = ex.Message });
      }
    }

    [HttpGet("account")]
    public ActionResult<Response<string>> GenerateAccountData()
    {
      try
      {
        //         private  readonly ProcessStartInfo cmdStartInfo = new()
        // {
        //     FileName = "cmd.exe",
        //     RedirectStandardInput = true,
        //     RedirectStandardOutput = true,
        //     UseShellExecute = false
        // };

        string[] commands = {
                    @"cd C:\Path\To\Sql\Scripts", // Change to the directory containing the SQL script
                    @"sqlcmd -S wyvernp.database.windows.net -d globird -U server_admin -P WyvernP2506 -i ./account.sql" };

        System.Console.WriteLine($"{_env.ContentRootPath}data/script/account_gen.py");
        //action to do  here
        string pythonScriptPath = $"{_env.ContentRootPath}data/script/account_gen.py";
        string pythonExecutablePath = "python3";

        var process = new Process
        {
          StartInfo = new ProcessStartInfo
          {
            FileName = pythonExecutablePath,
            Arguments = pythonScriptPath,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
          }
        };

        process.Start();
        string result = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        return Ok();
      }
      catch (Exception ex)
      {
        return BadRequest(new Response<string> { Data = ex.Message });
      }
    }

    [HttpPost("bird")]
    public ActionResult<Response<string>> GenerateBirdData([FromBody] BirdGenDTO body)
    {
      try
      {
        //action to do  here
        System.Console.WriteLine(body.UserId);
        return Ok();
      }
      catch (Exception ex)
      {
        return BadRequest(new Response<string> { Data = ex.Message });
      }
    }

    [HttpPost("request")]
    public ActionResult<Response<string>> GenerateRequestData()
    {
      try
      {
        //action to do  here
        return Ok();
      }
      catch (Exception ex)
      {
        return BadRequest(new Response<string> { Data = ex.Message });
      }
    }

    [HttpPost("match")]
    public ActionResult<Response<string>> GenerateMatchData()
    {
      try
      {
        //action to do here 
        return Ok();
      }
      catch (Exception ex)
      {
        return BadRequest(new Response<string> { Data = ex.Message });
      }
    }

    [HttpPost("payment")]
    public ActionResult<Response<string>> GeneratePaymentData()
    {
      try
      {
        //action to do  here
        return Ok();
      }
      catch (Exception ex)
      {
        return BadRequest(new Response<string> { Data = ex.Message });
      }
    }
  }
  public class BirdGenDTO
  {
    public string UserId { get; set; }
    public int NumberRecords { get; set; }
  }
}