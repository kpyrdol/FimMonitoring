using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FIMMonitoring.Common;
using FIMMonitoring.Common.Helpers;
using FIMMonitoring.Domain;
using FIMMonitoring.Domain.Enum;
using FIMMonitoring.Service.Properties;
using Quartz;

namespace FIMMonitoring.Service.Jobs
{
    [DisallowConcurrentExecution]
    public class LogFilesCheckerJob : IJob
    {
        private readonly FIMContext context = new FIMContext();
        private readonly SoftLogsContext softLogsContext = new SoftLogsContext();
        private readonly string patternError = "ERROR(.*?)";
        private readonly string patternSourceId = "\\[SID(.*?)\\]";

        public void Execute(IJobExecutionContext jobContext)
        {
            Logger.Log.Info("Starting LogFilesCheckeJob");
            Logger.Log.Info($"Getting list of files in {Settings.Default.LogDirectory}");
            var files = GetLogFiles().Where(e => !context.FileChecks.Select(f => f.FileName).Contains(e)).ToList();
            if (files.Any())
            {
                Logger.Log.Info($"Got {files.Count} files to check");

                foreach (var item in files)
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        var fileCheck = new FileCheck()
                        {
                            FileName = item,
                            ParseDate = DateTime.Now,
                            Parsed = false
                        };
                        context.FileChecks.Add(fileCheck);
                        try
                        {
                            ProcessFile(item, ref fileCheck);
                            context.SaveChanges();
                            transaction.Commit();
                            fileCheck.Parsed = true;
                        }
                        catch (Exception e)
                        {
                            Logger.Log.Error($"An error accured while processing file {item} {e}");
                            transaction.Rollback();
                        }
                        finally
                        {
                            context.SaveChanges();
                        }
                    }
                }
            }
            else
            {
                Logger.Log.Info("No new files to check. Finishing work");
            }


        }

        public void ProcessFile(string filename, ref FileCheck fileCheck)
        {
            var sender = new MailSender()
            {
                MailFrom = Settings.Default.MailFrom,
                MailFromDisplayName = Settings.Default.MailFromDisplayName,
                MailFromPassword = Settings.Default.MailFromPassword,
                MailHost = Settings.Default.MailHost,
                MailPort = Settings.Default.MailPort,
                MailSsl = Settings.Default.MailSsl
            };
            try
            {
                Logger.Log.Info($"Start processing file {filename}");
                var fileContent = File.ReadAllLines(filename);
                var fileContentAll = File.ReadAllText(filename);

                Regex rg = new Regex(patternError, RegexOptions.None);

                var errorLines = fileContent.Where(e => rg.IsMatch(e)).ToList();

                if (errorLines.Count > 0)
                {
                    Logger.Log.Info($"Found {errorLines.Count} errors");
                    foreach (var error in errorLines)
                    {
                        var importError = new ServiceImportError()
                        {
                            ErrorLevel = ErrorLevel.Critical,
                            CreatedAt = DateTime.Now,
                            ErrorDate = DateTime.Parse(error.Substring(0, 19)),
                            ErrorSource = ErrorSource.LogFile,
                            ErrorType = ErrorType.DownloadFileError,
                            FileCheck = fileCheck,
                            Guid = Guid.NewGuid()
                        };

                        Logger.Log.Info("Trying to get error text");

                        try
                        {
                            if (importError.ErrorDate.HasValue)
                            {
                                var desc = Regex.Matches(fileContentAll,
                                   $"{(error.Replace("[", "\\[").Replace("]", "\\]"))}(.*?){(importError.ErrorDate.Value.ToString("yyyy-MM-dd HH:mm:ss"))}",
                                   RegexOptions.Singleline);
                                if (desc.Count > 0)
                                {
                                    importError.Description = desc[0].ToString();
                                    Logger.Log.Info("Error text found");
                                }
                                else
                                {
                                    Logger.Log.Info("Error text not found");
                                }
                            }
                            
                        }
                        catch (Exception e)
                        {
                            Logger.Log.Error("Error while getting error text");
                        }


                        var ids = Regex.Matches(error, patternSourceId);
                        if (ids.Count > 0)
                        {
                            Logger.Log.Info($"Detailed error found");
                            var splited = ids[0].ToString().Replace("[", "").Replace("]", "").Split(';');
                            if (splited.Any())
                            {
                                int sourceId = Int32.Parse(splited[0].Replace("SID:", ""));
                                var importSource = softLogsContext.ImportSources.FirstOrDefault(e => e.Id == sourceId);

                                Logger.Log.Info($"Found specific Import Source ID {sourceId}");

                                if (importSource != null)
                                {
                                    Logger.Log.Info($"Import source found");
                                    importError.CarrierId = importSource.CarrierMapping.CarrierId;
                                    importError.SourceId = sourceId;
                                    importError.CustomerId = importSource.CustomerId;
                                    importError.SourceName = importSource.Name;
                                    importError.CarrierName = importSource.CarrierMapping.Carrier.Name;
                                    importError.CustomerName= importSource.Customer.Name;
                                }
                                else
                                {
                                    Logger.Log.Info($"Cannot find importSource with id: {sourceId}");
                                }


                                if (splited.Length == 2)
                                {
                                    int importId = Int32.Parse(splited[1].Replace("IID:", ""));
                                    var import = softLogsContext.Imports.FirstOrDefault(e => e.Id == importId);

                                    Logger.Log.Info($"Found specific Import Id {importId}");

                                    if (import != null)
                                    {
                                        importError.ImportId = importId;
                                        Logger.Log.Info($"Import found");
                                    }
                                    else
                                    {
                                        Logger.Log.Warn($"Cannot find import with id: {importId}");
                                    }
                                }
                            }
                        }
                        else
                        {
                            Logger.Log.Info("No detail error found in line");
                        }

                        context.ServiceImportErrors.Add(importError);
                    }
                }
                else
                {
                    Logger.Log.Info("No errors found");
                }

                Logger.Log.Info($"Finished processing file {filename}");
            }
            catch (Exception ex)
            {
                Logger.Log.Error("LogFilesCheckeJob has encountered an error.", ex);
                var mail = new MailViewModel
                {
                    To = Settings.Default.MailAdmin,
                    Subject = "FIM Import error service",
                    CC = Settings.Default.MailToCC,
                    Body = $"An error occured in LogFilesCheckeJob {ex.Message}"
                };
                sender.Send(mail);
                throw;
            }

        }

        private List<string> GetLogFiles()
        {

            if (Directory.Exists(Settings.Default.LogDirectory))
            {
                return Directory.GetFiles(Settings.Default.LogDirectory).Where(e => !e.EndsWith(".log")).ToList();
            }

            return new List<string>();
        }
    }
}
