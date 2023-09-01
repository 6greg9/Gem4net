namespace TestForm;
using GemVarRepository;
using GemVarRepository.Model;
using GemDeviceService;
using Secs4Net;
using Secs4Net.Sml;
using Secs4Net.Extensions;


public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
        var logger = new SecsLogger(this);
        service = new GemDeviceService(default, default
            , default, default, default, logger);
        service.OnConnectStatusChange += (msg) =>
        {
            this.Invoke(new Action(() => { richTextBox2.AppendText($"{msg}\n"); ; }));
        };

    }
    GemDeviceService service;
    private void button1_Click(object sender, EventArgs e)
    {
        using (var db = new GemVarContext())
        {
            var test = db.Variables.Where(v=> v.VID== 7).FirstOrDefault();
            MessageBox.Show(test.Name);
        }
    }

    private void button2_Click(object sender, EventArgs e)
    {
        service.Enable();
    }

    private void listView1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    private sealed class SecsLogger : ISecsGemLogger
    {
        private readonly Form1 _form;
        internal SecsLogger(Form1 form)
        {
            _form = form;
        }

        public void MessageIn(SecsMessage msg, int id)
        {
            _form.Invoke((MethodInvoker)delegate
            {
                _form.richTextBox1.SelectionColor = Color.Black;
                _form.richTextBox1.AppendText($"{DateTime.Now.ToString()}<-- [0x{id:X8}] {msg.ToSml()}\n");
            });
        }

        public void MessageOut(SecsMessage msg, int id)
        {
            _form.Invoke((MethodInvoker)delegate
            {
                _form.richTextBox1.SelectionColor = Color.Black;
                _form.richTextBox1.AppendText($"{DateTime.Now.ToString()}--> [0x{id:X8}] {msg.ToSml()}\n");
            });
        }

        public void Info(string msg)
        {
            _form.Invoke((MethodInvoker)delegate
            {
                _form.richTextBox1.SelectionColor = Color.Blue;
                _form.richTextBox1.AppendText($"{DateTime.Now.ToString()}{msg}\n");
            });
        }

        public void Warning(string msg)
        {
            _form.Invoke((MethodInvoker)delegate
            {
                _form.richTextBox1.SelectionColor = Color.Green;
                _form.richTextBox1.AppendText($"{DateTime.Now.ToString()}{msg}\n");
            });
        }

        public void Error(string msg, SecsMessage? message, Exception? ex)
        {
            _form.Invoke((MethodInvoker)delegate
            {
                _form.richTextBox1.SelectionColor = Color.Red;
                _form.richTextBox1.AppendText($"{DateTime.Now.ToString()}{msg}\n");
                _form.richTextBox1.AppendText($"{message?.ToSml()}\n");
                _form.richTextBox1.SelectionColor = Color.Gray;
                _form.richTextBox1.AppendText($"{ex}\n");
            });
        }

        public void Debug(string msg)
        {
            _form.Invoke((MethodInvoker)delegate
            {
                _form.richTextBox1.SelectionColor = Color.Yellow;
                _form.richTextBox1.AppendText($"{DateTime.Now.ToString()}{msg}\n");
            });
        }

#if NET472
        public void Error(string msg)
        {
            Error(msg, null, null);
        }

        public void Error(string msg, Exception ex)
        {
            Error(msg, null, ex);
        }
#endif
    }

    private async void button3_Click(object sender, EventArgs e)
    {
        var secs = service.GetSecsWrapper;
        var S1F1 = new SecsMessage(1, 1);
        var S1F2 = await secs.SendAsync(S1F1);
        MessageBox.Show(S1F2.ToSml());
    }
}
