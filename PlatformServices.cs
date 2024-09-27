using Primavera.Extensibility.Platform.Services;
using Primavera.Extensibility.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Primavera.Extensibility.BusinessEntities.ExtensibilityService.EventArgs;
using System.Drawing;
using StdPlatBS100;
using StdBE100;

namespace ExtensibilityBestPracticesSaaS
{
    public class PlatformServices: Plataforma
    {
        private const string TABID = "TAB1";
        private const string GRPID = "GRPID1";
        private const string BTNID = "BTNID";

        #region Private Variables

        private StdBSPRibbon RibbonEvents;

        #endregion

        #region Public Events
        /// <summary>
        /// Registo de evento para criação de menus custom
        /// </summary>
        /// <param name="e">Args</param>
        public override void DepoisDeCriarMenus(ExtensibilityEventArgs e)
        {
            string antes = $"Mem. Hard: {Math.Round(PSO.SaaSPerformaceMeasurement.InUseCPUHardLimit, 2)} MB (lim: {PSO.SaaSPerformaceMeasurement.MemoryHardLimit}) | Proc.: {PSO.SaaSPerformaceMeasurement.InUseCPUHardLimit} ms (lim: {PSO.SaaSPerformaceMeasurement.CPUHardLimit})";
            // PSO.MensagensDialogos.MostraMensagem(StdBSTipos.TipoMsg.PRI_SimplesOk, "Antes de criar menus.", StdBE100.StdBETipos.IconId.PRI_Informativo, antes, 1, true);
            
            RegistaMenusCustom();

            string depois = $"Mem. Hard: {Math.Round(PSO.SaaSPerformaceMeasurement.InUseCPUHardLimit, 2)} MB (lim: {PSO.SaaSPerformaceMeasurement.MemoryHardLimit}) | Proc.: {PSO.SaaSPerformaceMeasurement.InUseCPUHardLimit} ms (lim: {PSO.SaaSPerformaceMeasurement.CPUHardLimit})";
            // PSO.MensagensDialogos.MostraMensagem(StdBSTipos.TipoMsg.PRI_SimplesOk, "Depois de criar menus.", StdBE100.StdBETipos.IconId.PRI_Informativo, depois, 1, true);
        }

        public override void DepoisDeSelecionarMenu(string Id, string Command, ExtensibilityEventArgs e)
        {
            RibbonEvents_Executa(Id, Command);
        }

        #endregion Public Events

        #region Private Methods

        /// <summary>
        /// Evento de clique por parte do utilizador
        /// </summary>
        /// <param name="Id">Id do botão clicado na Ribbon</param>
        /// <param name="Comando">Comando a executar</param>
        private void RibbonEvents_Executa(string Id, string Comando)
        {
            try
            {
                string mensagem = $"O utilizador clicou no botão com id {Id}";

                // Trace para o ficheiro de Log
                PSO.Diagnosticos.Trace(mensagem);

                // Trace para o Event Viewer
                BSO.Extensibility.Logger.LogMessage(mensagem);

                switch (Id)
                {
                    case BTNID:
                        // NOTA: em SaaS a instânciação de um custom Form tem que ser chamada com o método abaixo, que recebe dois parâmetros
                        // o segundo parâmetro indica se o Form estará dentro da shell
                        using (var inst = BSO.Extensibility.CreateCustomFormInstance(typeof(CustomerForm).ToString(), false))
                        {
                            (inst.Result as CustomerForm).ShowDialog();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                PSO.MensagensDialogos.MostraAviso("Erro ao executar operação", StdBETipos.IconId.PRI_Informativo, ex.Message);
            }
        }

        private void RegistaMenusCustom()
        {
            // Subscrição de evento para clique no butão da Ribbon
            RibbonEvents = this.PSO.Ribbon;

            // NOTA: Esta subscrição de um evento não funciona em SaaS
            // RibbonEvents.Executa += RibbonEvents_Executa;

            CreateTab("SaaS Extensibility");
            CreateGroup("Teste");
            CreateGroupButton32(TABID, GRPID, BTNID, "Create Show Dialog", Properties.Resources.home);
        }

        private void CreateTab(string Descricao)
        {
            this.PSO.Ribbon.CriaRibbonTab(Descricao, TABID, 10);
        }

        private void CreateGroup(string Descricao)
        {
            this.PSO.Ribbon.CriaRibbonGroup(TABID, Descricao, GRPID);
        }

        private void CreateGroupButton32(string tabId, string groupId, string buttonId, string buttonDescription, Image buttonImage)
        {
            this.PSO.Ribbon.CriaRibbonButton(tabId, groupId, buttonId, buttonDescription, true, buttonImage);
        }
        #endregion Private Methods
    }
}
