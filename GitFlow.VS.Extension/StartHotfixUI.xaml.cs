﻿using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TeamFoundation.Git.Extensibility;

namespace GitFlowVS.Extension
{
    /// </summary>
    public partial class StartHotfixUI : UserControl
    {
        private readonly GitFlowSection parent;
        private readonly StartHotfixModel model;
        private IGitRepositoryInfo ActiveRepo { get; set; }
        private IVsOutputWindowPane OutputWindow { get; set; }

        public StartHotfixUI(GitFlowSection parent, IGitRepositoryInfo activeRepo, IVsOutputWindowPane outputWindow)
        {
            this.parent = parent;
            ActiveRepo = activeRepo;
            OutputWindow = outputWindow;
            InitializeComponent();

            model = new StartHotfixModel();
            DataContext = model;
        }

        private void HotfixCancel_Click(object sender, RoutedEventArgs e)
        {
            parent.CancelAction();
        }

        private void HotfixOk_Click(object sender, RoutedEventArgs e)
        {
            if (ActiveRepo != null)
            {
                OutputWindow.Activate();
                using (new WaitCursor())
                {
                    var gf = new VsGitFlowWrapper(ActiveRepo.RepositoryPath, OutputWindow, parent);
                    gf.StartHotfix(model.HotfixName);
                }
                parent.FinishAction();
            }
        }
    }
}
