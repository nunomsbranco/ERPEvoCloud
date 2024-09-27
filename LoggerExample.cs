using Primavera.Extensibility.BusinessEntities;
using Primavera.Extensibility.CustomCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensibilityBestPracticesSaaS
{
    /// <summary>
    /// Classe que permite efetuar log no Event Viewer
    /// </summary>
    public class LoggerExample : CustomCode
    {
        /// <summary>
        /// Efetua log de uma mensagem no Event Viewer
        /// </summary>
        public void LogToEventViewer()
        {
            string message = "";

            PSO.MensagensDialogos.MostraDialogoInput(ref message, "Insira a mensagem a efetuar log", "Mensagem para log no Event Viewer", 0, true);
            BSO.Extensibility.Logger.LogMessage(message);           
        }
    }
}
