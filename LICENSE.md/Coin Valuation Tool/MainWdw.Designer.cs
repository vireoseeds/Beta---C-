namespace Coin_Valuation_Tool
{
    partial class MainWdw
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menu = new System.Windows.Forms.MenuStrip();
            this.projectsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newProjectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mixDebtEquityMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.equityOnlyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debtOnlyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.seeProjectsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openProjectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.thirdPartiesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.parametersMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fXRiskMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.degradationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.climateImpactMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.auditorFeeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.depositRatesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.coinSetupMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.depositRateDecreaseMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.riskTestsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.seedsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeDistributionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createGroupSeedsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.projectInScopeGrid = new System.Windows.Forms.DataGridView();
            this.ProjectID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProjectName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProjectCountry = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProjectSector = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProjectSubSector = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProjectiCur = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.contextMenuProjectInScope = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.updateStatusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.banckrupcyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.finishedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.validatedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateCashFlowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateConstructionCFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateDepositsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.equityOtherInvestorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loanWithdrawalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.coinsWithdrawalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.energyProductionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pPAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sGAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.royaltiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.taxesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.globalSeedsGrid = new System.Windows.Forms.DataGridView();
            this.GSProjectID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ValoGS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PortionGS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label3 = new System.Windows.Forms.Label();
            this.projectSeedsGrid = new System.Windows.Forms.DataGridView();
            this.ProjectSeedID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProjectSeedProjectID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProjectSeedValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProjectSeedPortion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label4 = new System.Windows.Forms.Label();
            this.groupSeedsGrid = new System.Windows.Forms.DataGridView();
            this.GroupSeedID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GroupSeedName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GroupSeedValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupSeedsMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.projectInScopeGrid)).BeginInit();
            this.contextMenuProjectInScope.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.globalSeedsGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.projectSeedsGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupSeedsGrid)).BeginInit();
            this.groupSeedsMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menu
            // 
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.projectsMenuItem,
            this.thirdPartiesMenuItem,
            this.parametersMenuItem,
            this.seedsMenuItem});
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(744, 24);
            this.menu.TabIndex = 0;
            this.menu.Text = "menu";
            // 
            // projectsMenuItem
            // 
            this.projectsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newProjectMenuItem,
            this.seeProjectsMenuItem,
            this.openProjectMenuItem});
            this.projectsMenuItem.Name = "projectsMenuItem";
            this.projectsMenuItem.Size = new System.Drawing.Size(61, 20);
            this.projectsMenuItem.Text = "&Projects";
            // 
            // newProjectMenuItem
            // 
            this.newProjectMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mixDebtEquityMenuItem,
            this.equityOnlyMenuItem,
            this.debtOnlyMenuItem});
            this.newProjectMenuItem.Name = "newProjectMenuItem";
            this.newProjectMenuItem.Size = new System.Drawing.Size(207, 22);
            this.newProjectMenuItem.Text = "&New Project";
            // 
            // mixDebtEquityMenuItem
            // 
            this.mixDebtEquityMenuItem.Name = "mixDebtEquityMenuItem";
            this.mixDebtEquityMenuItem.Size = new System.Drawing.Size(165, 22);
            this.mixDebtEquityMenuItem.Text = "&Mix Debt / Equity";
            this.mixDebtEquityMenuItem.Click += new System.EventHandler(this.mixDebtEquityMenuItem_Click);
            // 
            // equityOnlyMenuItem
            // 
            this.equityOnlyMenuItem.Name = "equityOnlyMenuItem";
            this.equityOnlyMenuItem.Size = new System.Drawing.Size(165, 22);
            this.equityOnlyMenuItem.Text = "&Equity only";
            // 
            // debtOnlyMenuItem
            // 
            this.debtOnlyMenuItem.Name = "debtOnlyMenuItem";
            this.debtOnlyMenuItem.Size = new System.Drawing.Size(165, 22);
            this.debtOnlyMenuItem.Text = "&Debt only";
            // 
            // seeProjectsMenuItem
            // 
            this.seeProjectsMenuItem.Name = "seeProjectsMenuItem";
            this.seeProjectsMenuItem.Size = new System.Drawing.Size(207, 22);
            this.seeProjectsMenuItem.Text = "&See Projects";
            // 
            // openProjectMenuItem
            // 
            this.openProjectMenuItem.Name = "openProjectMenuItem";
            this.openProjectMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.openProjectMenuItem.Size = new System.Drawing.Size(207, 22);
            this.openProjectMenuItem.Text = "&Manage Projects";
            this.openProjectMenuItem.Click += new System.EventHandler(this.openProjectMenuItem_Click);
            // 
            // thirdPartiesMenuItem
            // 
            this.thirdPartiesMenuItem.Name = "thirdPartiesMenuItem";
            this.thirdPartiesMenuItem.Size = new System.Drawing.Size(85, 20);
            this.thirdPartiesMenuItem.Text = "&Third Parties";
            this.thirdPartiesMenuItem.Click += new System.EventHandler(this.thirdPartiesMenuItem_Click);
            // 
            // parametersMenuItem
            // 
            this.parametersMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fXRiskMenuItem,
            this.degradationMenuItem,
            this.climateImpactMenuItem,
            this.auditorFeeMenuItem,
            this.depositRatesMenuItem,
            this.coinSetupMenuItem,
            this.depositRateDecreaseMenuItem,
            this.riskTestsMenuItem});
            this.parametersMenuItem.Name = "parametersMenuItem";
            this.parametersMenuItem.Size = new System.Drawing.Size(78, 20);
            this.parametersMenuItem.Text = "P&arameters";
            // 
            // fXRiskMenuItem
            // 
            this.fXRiskMenuItem.Name = "fXRiskMenuItem";
            this.fXRiskMenuItem.Size = new System.Drawing.Size(190, 22);
            this.fXRiskMenuItem.Text = "&FX Risk";
            this.fXRiskMenuItem.Click += new System.EventHandler(this.fXRiskMenuItem_Click);
            // 
            // degradationMenuItem
            // 
            this.degradationMenuItem.Name = "degradationMenuItem";
            this.degradationMenuItem.Size = new System.Drawing.Size(190, 22);
            this.degradationMenuItem.Text = "&Degradation";
            this.degradationMenuItem.Click += new System.EventHandler(this.degradationMenuItem_Click);
            // 
            // climateImpactMenuItem
            // 
            this.climateImpactMenuItem.Name = "climateImpactMenuItem";
            this.climateImpactMenuItem.Size = new System.Drawing.Size(190, 22);
            this.climateImpactMenuItem.Text = "&Climate Impact";
            this.climateImpactMenuItem.Click += new System.EventHandler(this.climateImpactMenuItem_Click);
            // 
            // auditorFeeMenuItem
            // 
            this.auditorFeeMenuItem.Name = "auditorFeeMenuItem";
            this.auditorFeeMenuItem.Size = new System.Drawing.Size(190, 22);
            this.auditorFeeMenuItem.Text = "&Auditor Fee";
            this.auditorFeeMenuItem.Click += new System.EventHandler(this.auditorFeeMenuItem_Click);
            // 
            // depositRatesMenuItem
            // 
            this.depositRatesMenuItem.Name = "depositRatesMenuItem";
            this.depositRatesMenuItem.Size = new System.Drawing.Size(190, 22);
            this.depositRatesMenuItem.Text = "De&posit Rates";
            this.depositRatesMenuItem.Click += new System.EventHandler(this.depositRatesMenuItem_Click);
            // 
            // coinSetupMenuItem
            // 
            this.coinSetupMenuItem.Name = "coinSetupMenuItem";
            this.coinSetupMenuItem.Size = new System.Drawing.Size(190, 22);
            this.coinSetupMenuItem.Text = "C&oin Setup";
            this.coinSetupMenuItem.Click += new System.EventHandler(this.coinSetupMenuItem_Click);
            // 
            // depositRateDecreaseMenuItem
            // 
            this.depositRateDecreaseMenuItem.Name = "depositRateDecreaseMenuItem";
            this.depositRateDecreaseMenuItem.Size = new System.Drawing.Size(190, 22);
            this.depositRateDecreaseMenuItem.Text = "Deposit &Rate Decrease";
            this.depositRateDecreaseMenuItem.Click += new System.EventHandler(this.depositRateDecreaseMenuItem_Click);
            // 
            // riskTestsMenuItem
            // 
            this.riskTestsMenuItem.Name = "riskTestsMenuItem";
            this.riskTestsMenuItem.Size = new System.Drawing.Size(190, 22);
            this.riskTestsMenuItem.Text = "Risk &Tests";
            this.riskTestsMenuItem.Click += new System.EventHandler(this.riskTestsMenuItem_Click);
            // 
            // seedsMenuItem
            // 
            this.seedsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeDistributionToolStripMenuItem,
            this.createGroupSeedsToolStripMenuItem});
            this.seedsMenuItem.Name = "seedsMenuItem";
            this.seedsMenuItem.Size = new System.Drawing.Size(49, 20);
            this.seedsMenuItem.Text = "&Seeds";
            // 
            // changeDistributionToolStripMenuItem
            // 
            this.changeDistributionToolStripMenuItem.Name = "changeDistributionToolStripMenuItem";
            this.changeDistributionToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.changeDistributionToolStripMenuItem.Text = "Change &Distribution";
            // 
            // createGroupSeedsToolStripMenuItem
            // 
            this.createGroupSeedsToolStripMenuItem.Name = "createGroupSeedsToolStripMenuItem";
            this.createGroupSeedsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.createGroupSeedsToolStripMenuItem.Text = "Create &Group Seeds";
            // 
            // projectInScopeGrid
            // 
            this.projectInScopeGrid.AllowUserToAddRows = false;
            this.projectInScopeGrid.AllowUserToDeleteRows = false;
            this.projectInScopeGrid.AllowUserToResizeRows = false;
            this.projectInScopeGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.projectInScopeGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ProjectID,
            this.ProjectName,
            this.ProjectCountry,
            this.ProjectSector,
            this.ProjectSubSector,
            this.ProjectiCur});
            this.projectInScopeGrid.Location = new System.Drawing.Point(12, 50);
            this.projectInScopeGrid.Name = "projectInScopeGrid";
            this.projectInScopeGrid.RowHeadersVisible = false;
            this.projectInScopeGrid.Size = new System.Drawing.Size(482, 263);
            this.projectInScopeGrid.TabIndex = 1;
            this.projectInScopeGrid.MouseEnter += new System.EventHandler(this.projectInScopeGrid_MouseEnter);
            this.projectInScopeGrid.MouseLeave += new System.EventHandler(this.projectInScopeGrid_MouseLeave);
            // 
            // ProjectID
            // 
            this.ProjectID.HeaderText = "Project ID";
            this.ProjectID.MaxInputLength = 6;
            this.ProjectID.Name = "ProjectID";
            this.ProjectID.ReadOnly = true;
            this.ProjectID.Width = 50;
            // 
            // ProjectName
            // 
            this.ProjectName.HeaderText = "Project Name";
            this.ProjectName.MaxInputLength = 30;
            this.ProjectName.Name = "ProjectName";
            this.ProjectName.ReadOnly = true;
            this.ProjectName.Width = 120;
            // 
            // ProjectCountry
            // 
            this.ProjectCountry.HeaderText = "Country";
            this.ProjectCountry.MaxInputLength = 25;
            this.ProjectCountry.Name = "ProjectCountry";
            this.ProjectCountry.ReadOnly = true;
            this.ProjectCountry.Width = 75;
            // 
            // ProjectSector
            // 
            this.ProjectSector.HeaderText = "Sector";
            this.ProjectSector.MaxInputLength = 20;
            this.ProjectSector.Name = "ProjectSector";
            this.ProjectSector.ReadOnly = true;
            this.ProjectSector.Width = 75;
            // 
            // ProjectSubSector
            // 
            this.ProjectSubSector.HeaderText = "SubSector";
            this.ProjectSubSector.MaxInputLength = 20;
            this.ProjectSubSector.Name = "ProjectSubSector";
            this.ProjectSubSector.ReadOnly = true;
            this.ProjectSubSector.Width = 75;
            // 
            // ProjectiCur
            // 
            this.ProjectiCur.HeaderText = "iCur";
            this.ProjectiCur.MaxInputLength = 3;
            this.ProjectiCur.Name = "ProjectiCur";
            this.ProjectiCur.ReadOnly = true;
            this.ProjectiCur.Width = 40;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Projects in scope:";
            // 
            // contextMenuProjectInScope
            // 
            this.contextMenuProjectInScope.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updateStatusToolStripMenuItem,
            this.updateCashFlowsToolStripMenuItem});
            this.contextMenuProjectInScope.Name = "contextMenuProjectInScope";
            this.contextMenuProjectInScope.Size = new System.Drawing.Size(175, 48);
            // 
            // updateStatusToolStripMenuItem
            // 
            this.updateStatusToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.banckrupcyToolStripMenuItem,
            this.defaultToolStripMenuItem,
            this.finishedToolStripMenuItem,
            this.validatedToolStripMenuItem});
            this.updateStatusToolStripMenuItem.Name = "updateStatusToolStripMenuItem";
            this.updateStatusToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.updateStatusToolStripMenuItem.Text = "Update &Status";
            // 
            // banckrupcyToolStripMenuItem
            // 
            this.banckrupcyToolStripMenuItem.Name = "banckrupcyToolStripMenuItem";
            this.banckrupcyToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.banckrupcyToolStripMenuItem.Text = "&Banckrupcy";
            // 
            // defaultToolStripMenuItem
            // 
            this.defaultToolStripMenuItem.Name = "defaultToolStripMenuItem";
            this.defaultToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.defaultToolStripMenuItem.Text = "&Default";
            // 
            // finishedToolStripMenuItem
            // 
            this.finishedToolStripMenuItem.Name = "finishedToolStripMenuItem";
            this.finishedToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.finishedToolStripMenuItem.Text = "&Finished";
            // 
            // validatedToolStripMenuItem
            // 
            this.validatedToolStripMenuItem.Name = "validatedToolStripMenuItem";
            this.validatedToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.validatedToolStripMenuItem.Text = "&Validated";
            // 
            // updateCashFlowsToolStripMenuItem
            // 
            this.updateCashFlowsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updateConstructionCFToolStripMenuItem,
            this.updateDepositsToolStripMenuItem,
            this.updateToolStripMenuItem,
            this.equityOtherInvestorsToolStripMenuItem,
            this.loanWithdrawalToolStripMenuItem,
            this.coinsWithdrawalToolStripMenuItem,
            this.energyProductionToolStripMenuItem,
            this.pPAToolStripMenuItem,
            this.oMToolStripMenuItem,
            this.sGAToolStripMenuItem,
            this.royaltiesToolStripMenuItem,
            this.taxesToolStripMenuItem});
            this.updateCashFlowsToolStripMenuItem.Name = "updateCashFlowsToolStripMenuItem";
            this.updateCashFlowsToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.updateCashFlowsToolStripMenuItem.Text = "Update &Cash Flows";
            // 
            // updateConstructionCFToolStripMenuItem
            // 
            this.updateConstructionCFToolStripMenuItem.Name = "updateConstructionCFToolStripMenuItem";
            this.updateConstructionCFToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.updateConstructionCFToolStripMenuItem.Text = "&Construction CF";
            // 
            // updateDepositsToolStripMenuItem
            // 
            this.updateDepositsToolStripMenuItem.Name = "updateDepositsToolStripMenuItem";
            this.updateDepositsToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.updateDepositsToolStripMenuItem.Text = "&Deposits";
            // 
            // updateToolStripMenuItem
            // 
            this.updateToolStripMenuItem.Name = "updateToolStripMenuItem";
            this.updateToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.updateToolStripMenuItem.Text = "&Equity PO";
            // 
            // equityOtherInvestorsToolStripMenuItem
            // 
            this.equityOtherInvestorsToolStripMenuItem.Name = "equityOtherInvestorsToolStripMenuItem";
            this.equityOtherInvestorsToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.equityOtherInvestorsToolStripMenuItem.Text = "Equity&OtherInvestors";
            // 
            // loanWithdrawalToolStripMenuItem
            // 
            this.loanWithdrawalToolStripMenuItem.Name = "loanWithdrawalToolStripMenuItem";
            this.loanWithdrawalToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.loanWithdrawalToolStripMenuItem.Text = "&Loan Withdrawal";
            // 
            // coinsWithdrawalToolStripMenuItem
            // 
            this.coinsWithdrawalToolStripMenuItem.Name = "coinsWithdrawalToolStripMenuItem";
            this.coinsWithdrawalToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.coinsWithdrawalToolStripMenuItem.Text = "Coins &Withdrawal";
            // 
            // energyProductionToolStripMenuItem
            // 
            this.energyProductionToolStripMenuItem.Name = "energyProductionToolStripMenuItem";
            this.energyProductionToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.energyProductionToolStripMenuItem.Text = "Energy &Production";
            // 
            // pPAToolStripMenuItem
            // 
            this.pPAToolStripMenuItem.Name = "pPAToolStripMenuItem";
            this.pPAToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.pPAToolStripMenuItem.Text = "PPA";
            // 
            // oMToolStripMenuItem
            // 
            this.oMToolStripMenuItem.Name = "oMToolStripMenuItem";
            this.oMToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.oMToolStripMenuItem.Text = "OM";
            // 
            // sGAToolStripMenuItem
            // 
            this.sGAToolStripMenuItem.Name = "sGAToolStripMenuItem";
            this.sGAToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.sGAToolStripMenuItem.Text = "SGA";
            // 
            // royaltiesToolStripMenuItem
            // 
            this.royaltiesToolStripMenuItem.Name = "royaltiesToolStripMenuItem";
            this.royaltiesToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.royaltiesToolStripMenuItem.Text = "Royalties";
            // 
            // taxesToolStripMenuItem
            // 
            this.taxesToolStripMenuItem.Name = "taxesToolStripMenuItem";
            this.taxesToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.taxesToolStripMenuItem.Text = "Taxes";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(525, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Global Seeds - Scope";
            // 
            // globalSeedsGrid
            // 
            this.globalSeedsGrid.AllowUserToAddRows = false;
            this.globalSeedsGrid.AllowUserToDeleteRows = false;
            this.globalSeedsGrid.AllowUserToOrderColumns = true;
            this.globalSeedsGrid.AllowUserToResizeRows = false;
            this.globalSeedsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.globalSeedsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.GSProjectID,
            this.ValoGS,
            this.PortionGS});
            this.globalSeedsGrid.Location = new System.Drawing.Point(528, 50);
            this.globalSeedsGrid.Name = "globalSeedsGrid";
            this.globalSeedsGrid.RowHeadersVisible = false;
            this.globalSeedsGrid.Size = new System.Drawing.Size(204, 263);
            this.globalSeedsGrid.TabIndex = 2;
            // 
            // GSProjectID
            // 
            this.GSProjectID.HeaderText = "Project ID";
            this.GSProjectID.MaxInputLength = 6;
            this.GSProjectID.Name = "GSProjectID";
            this.GSProjectID.ReadOnly = true;
            this.GSProjectID.Width = 50;
            // 
            // ValoGS
            // 
            this.ValoGS.HeaderText = "Value";
            this.ValoGS.MaxInputLength = 8;
            this.ValoGS.Name = "ValoGS";
            this.ValoGS.ReadOnly = true;
            this.ValoGS.Width = 50;
            // 
            // PortionGS
            // 
            this.PortionGS.HeaderText = "Portion";
            this.PortionGS.MaxInputLength = 5;
            this.PortionGS.Name = "PortionGS";
            this.PortionGS.ReadOnly = true;
            this.PortionGS.Width = 50;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 336);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Project Seeds - Scope";
            // 
            // projectSeedsGrid
            // 
            this.projectSeedsGrid.AllowUserToAddRows = false;
            this.projectSeedsGrid.AllowUserToDeleteRows = false;
            this.projectSeedsGrid.AllowUserToOrderColumns = true;
            this.projectSeedsGrid.AllowUserToResizeRows = false;
            this.projectSeedsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.projectSeedsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ProjectSeedID,
            this.ProjectSeedProjectID,
            this.ProjectSeedValue,
            this.ProjectSeedPortion});
            this.projectSeedsGrid.Location = new System.Drawing.Point(12, 352);
            this.projectSeedsGrid.Name = "projectSeedsGrid";
            this.projectSeedsGrid.RowHeadersVisible = false;
            this.projectSeedsGrid.Size = new System.Drawing.Size(226, 225);
            this.projectSeedsGrid.TabIndex = 3;
            // 
            // ProjectSeedID
            // 
            this.ProjectSeedID.HeaderText = "Seed ID";
            this.ProjectSeedID.MaxInputLength = 6;
            this.ProjectSeedID.Name = "ProjectSeedID";
            this.ProjectSeedID.ReadOnly = true;
            this.ProjectSeedID.Width = 50;
            // 
            // ProjectSeedProjectID
            // 
            this.ProjectSeedProjectID.HeaderText = "Project ID";
            this.ProjectSeedProjectID.MaxInputLength = 6;
            this.ProjectSeedProjectID.Name = "ProjectSeedProjectID";
            this.ProjectSeedProjectID.ReadOnly = true;
            this.ProjectSeedProjectID.Width = 50;
            // 
            // ProjectSeedValue
            // 
            this.ProjectSeedValue.HeaderText = "Value";
            this.ProjectSeedValue.MaxInputLength = 8;
            this.ProjectSeedValue.Name = "ProjectSeedValue";
            this.ProjectSeedValue.ReadOnly = true;
            this.ProjectSeedValue.Width = 50;
            // 
            // ProjectSeedPortion
            // 
            this.ProjectSeedPortion.HeaderText = "Portion";
            this.ProjectSeedPortion.MaxInputLength = 5;
            this.ProjectSeedPortion.Name = "ProjectSeedPortion";
            this.ProjectSeedPortion.ReadOnly = true;
            this.ProjectSeedPortion.Width = 50;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(272, 336);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Group Seeds - Scope";
            // 
            // groupSeedsGrid
            // 
            this.groupSeedsGrid.AllowUserToAddRows = false;
            this.groupSeedsGrid.AllowUserToDeleteRows = false;
            this.groupSeedsGrid.AllowUserToOrderColumns = true;
            this.groupSeedsGrid.AllowUserToResizeRows = false;
            this.groupSeedsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.groupSeedsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.GroupSeedID,
            this.GroupSeedName,
            this.GroupSeedValue});
            this.groupSeedsGrid.Location = new System.Drawing.Point(275, 352);
            this.groupSeedsGrid.Name = "groupSeedsGrid";
            this.groupSeedsGrid.RowHeadersVisible = false;
            this.groupSeedsGrid.Size = new System.Drawing.Size(253, 225);
            this.groupSeedsGrid.TabIndex = 5;
            this.groupSeedsGrid.MouseEnter += new System.EventHandler(this.groupSeedsGrid_MouseEnter);
            this.groupSeedsGrid.MouseLeave += new System.EventHandler(this.groupSeedsGrid_MouseLeave);
            // 
            // GroupSeedID
            // 
            this.GroupSeedID.HeaderText = "Seed ID";
            this.GroupSeedID.MaxInputLength = 6;
            this.GroupSeedID.Name = "GroupSeedID";
            this.GroupSeedID.ReadOnly = true;
            this.GroupSeedID.Width = 50;
            // 
            // GroupSeedName
            // 
            this.GroupSeedName.HeaderText = "Name";
            this.GroupSeedName.MaxInputLength = 20;
            this.GroupSeedName.Name = "GroupSeedName";
            this.GroupSeedName.ReadOnly = true;
            this.GroupSeedName.Width = 125;
            // 
            // GroupSeedValue
            // 
            this.GroupSeedValue.HeaderText = "Value";
            this.GroupSeedValue.MaxInputLength = 8;
            this.GroupSeedValue.Name = "GroupSeedValue";
            this.GroupSeedValue.ReadOnly = true;
            this.GroupSeedValue.Width = 50;
            // 
            // groupSeedsMenuStrip
            // 
            this.groupSeedsMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem});
            this.groupSeedsMenuStrip.Name = "groupSeedsMenuStrip";
            this.groupSeedsMenuStrip.Size = new System.Drawing.Size(104, 26);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "&Open";
            // 
            // MainWdw
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 587);
            this.Controls.Add(this.groupSeedsGrid);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.projectSeedsGrid);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.globalSeedsGrid);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.projectInScopeGrid);
            this.Controls.Add(this.menu);
            this.MainMenuStrip = this.menu;
            this.MaximumSize = new System.Drawing.Size(760, 625);
            this.MinimumSize = new System.Drawing.Size(760, 625);
            this.Name = "MainWdw";
            this.Text = "Coin Valuation Tool";
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.MainWdw_Layout);
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.projectInScopeGrid)).EndInit();
            this.contextMenuProjectInScope.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.globalSeedsGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.projectSeedsGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupSeedsGrid)).EndInit();
            this.groupSeedsMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem projectsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newProjectMenuItem;
        private System.Windows.Forms.ToolStripMenuItem seeProjectsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem thirdPartiesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem parametersMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fXRiskMenuItem;
        private System.Windows.Forms.ToolStripMenuItem degradationMenuItem;
        private System.Windows.Forms.ToolStripMenuItem climateImpactMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openProjectMenuItem;
        private System.Windows.Forms.ToolStripMenuItem auditorFeeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem depositRatesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem coinSetupMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mixDebtEquityMenuItem;
        private System.Windows.Forms.ToolStripMenuItem equityOnlyMenuItem;
        private System.Windows.Forms.ToolStripMenuItem debtOnlyMenuItem;
        private System.Windows.Forms.ToolStripMenuItem depositRateDecreaseMenuItem;
        private System.Windows.Forms.ToolStripMenuItem riskTestsMenuItem;
        private System.Windows.Forms.DataGridView projectInScopeGrid;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip contextMenuProjectInScope;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectiCur;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectSubSector;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectSector;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectCountry;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectID;
        private System.Windows.Forms.ToolStripMenuItem seedsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateStatusToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem banckrupcyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem defaultToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem finishedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem validatedToolStripMenuItem;
        private System.Windows.Forms.DataGridView globalSeedsGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn GSProjectID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ValoGS;
        private System.Windows.Forms.DataGridViewTextBoxColumn PortionGS;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripMenuItem changeDistributionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createGroupSeedsToolStripMenuItem;
        private System.Windows.Forms.DataGridView projectSeedsGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectSeedID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectSeedProjectID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectSeedValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectSeedPortion;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView groupSeedsGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn GroupSeedID;
        private System.Windows.Forms.DataGridViewTextBoxColumn GroupSeedName;
        private System.Windows.Forms.DataGridViewTextBoxColumn GroupSeedValue;
        private System.Windows.Forms.ContextMenuStrip groupSeedsMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateCashFlowsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateConstructionCFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateDepositsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem equityOtherInvestorsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loanWithdrawalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem coinsWithdrawalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem energyProductionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pPAToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sGAToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem royaltiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem taxesToolStripMenuItem;
    }
}

