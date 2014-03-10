using System;
using System.Windows.Forms;
using System.Text;

namespace NBi.UI.Genbi.View
{
    public partial class ExceptionManagerWindow : Form
    {
        public ExceptionManagerWindow(Exception exception)
        {
            this.InitializeComponent();
            this.exceptionLog.Text = Parse(exception, 0);
        }

        private static string Parse(Exception exception, int indentLevel)
        {
            var sb = new StringBuilder();

            var indent = string.Empty.PadLeft(indentLevel * 3, ' ');

            sb.AppendFormat("{1}Source: {0}", exception.Source, indent);
            sb.AppendLine();
            sb.AppendFormat("{1}Type: {0}", exception.GetType().FullName, indent);
            sb.AppendLine();
            sb.AppendFormat("{0}Description:", indent);
            sb.AppendLine();
            sb.AppendLine(string.Empty.PadLeft(40, '-'));
            sb.AppendLine(exception.ToString());

            if (exception.InnerException != null)
            {
                sb.AppendLine();
                sb.AppendFormat("{0}--- INNER EXCEPTION ".PadLeft(40, '-'), indent);
                sb.AppendLine();
                sb.AppendLine(Parse(exception.InnerException, indentLevel + 1));
            }

            return sb.ToString();
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
