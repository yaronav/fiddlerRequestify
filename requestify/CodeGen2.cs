using System.Linq;
using System.Windows.Forms;
using Fiddler;

namespace requestify
{    
    public partial class CodeGen2 : UserControl
    {
        public CodeGen2()
        {
            InitializeComponent();

            txtCode.DragEnter += RequestToCodeView_DragEnter;
            txtCode.DragDrop += RequestToCodeView_DragDrop;
            txtCode.DragOver += txtCode_DragOver;
            
            txtCode.AllowDrop = true;
            txtCode.Language = FastColoredTextBoxNS.Language.Lua;
            this.AllowDrop = true;
            
            this.DragEnter += new System.Windows.Forms.DragEventHandler(RequestToCodeView_DragEnter);
            this.DragOver += new DragEventHandler(RequestToCodeView_DragDrop);
            this.DragDrop += new DragEventHandler(RequestToCodeView_DragDrop);
        }

        public bool SetSessions(Session[] sessions)
        {                                    
            txtCode.Text = CodeGenerator.GeneratePythonCode(sessions.First());
            return true;
        }

        private void txtCode_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = (e.Data.GetDataPresent("Fiddler.Session[]") ? DragDropEffects.Copy : DragDropEffects.None);
        }

        
        private void RequestToCodeView_DragDrop(object sender, DragEventArgs e)
        {
            Session[] array = (Session[])e.Data.GetData("Fiddler.Session[]");
            if (array != null && array.Length >= 1)
            {
                SetSessions(array);
            }
        }

        private void RequestToCodeView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = (e.Data.GetDataPresent("Fiddler.Session[]") ? DragDropEffects.Copy : DragDropEffects.None);
        }
    }
}
