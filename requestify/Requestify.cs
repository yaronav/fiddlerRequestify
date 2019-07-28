using System.Windows.Forms;
using Fiddler;
using requestify;

[assembly: Fiddler.RequiredVersion("2.3.5.0")]

public class Requestify : IFiddlerExtension
{
    public Requestify()
    { }

    public void OnLoad()
    {
        var _codeTab = new TabPage("Requstify");
        _codeTab.ImageIndex = (int)Fiddler.SessionIcons.RPC;
        _codeTab.AllowDrop = true;
        
        var _codeView = new CodeGen2();
        _codeView.AllowDrop = true;
        _codeTab.Controls.Add(_codeView);
        _codeView.Dock = DockStyle.Fill;
        FiddlerApplication.UI.tabsViews.TabPages.Add(_codeTab);        
        if (!FiddlerApplication.UI.tabsViews.ShowToolTips)
        {
            FiddlerApplication.UI.tabsViews.ShowToolTips = true;
        }
        _codeTab.ToolTipText = "Drag sessions into this tab to generate Python code";        
    }

    public void OnBeforeUnload() { }
}