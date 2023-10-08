namespace TestForm;
using GemVarRepository;
using GemVarRepository.Model;
using GemDeviceService;
using Secs4Net;
using Secs4Net.Sml;
using Secs4Net.Extensions;
using Secs4Net.Json;
using Microsoft.Extensions.Options;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

public partial class Form1 : Form
{
    GemRepository _gemRepo;

    public Form1()
    {
        InitializeComponent();
        _gemRepo = new GemRepository(new GemVarContext());

        var logger = new SecsLogger(this);
        //var options = Options.Create(new SecsGemOptions
        //{
        //    IsActive = true,
        //    IpAddress = "127.0.0.1",
        //    Port = 5000,
        //    SocketReceiveBufferSize = 8096,
        //    DeviceId= 0,
        //    T6= 5000
        //});
        service = new GemDeviceService(logger, _gemRepo, new SecsGemOptions
        {
            IsActive = true,
            IpAddress = "127.0.0.1",
            Port = 5000,
            //SocketReceiveBufferSize = 8096,
            SocketReceiveBufferSize = 16384,
            //SocketReceiveBufferSize = 32768,
            DeviceId = 0,
            T6 = 5000
        });
        service.OnConnectStatusChange += (status) =>
        {
            this.Invoke(new Action(() => { rtbx_HSMS.AppendText($"{status}\n"); ; }));
        };
        service.OnCommStateChange += (current, previous) =>
        {
            this.Invoke(new Action(() => { rtbx_Comm.AppendText($"{current},{previous}\n"); ; }));
        };



    }
    GemDeviceService service;
    private void button1_Click(object sender, EventArgs e)
    {
        var test = _gemRepo.GetSvByVID(9);
        MessageBox.Show(test.ToJson());

        var nameList = _gemRepo.GetSvNameListAll();
        MessageBox.Show(nameList.ToJson());
        //using (var db = new GemVarContext())
        //{
        //    //var test = db.Variables.Where(v=> v.VID== 7).FirstOrDefault();
        //    //MessageBox.Show(test.Name);



        //    var testListSV = db.Variables.Where(v=>v.VID==9).FirstOrDefault();
        //    if (testListSV != null && testListSV.DataType == "LIST")
        //    {

        //        var children = db.Variables.Where(v=>v.ListSVID==testListSV.VID).ToList();
        //        // var temp = Item.L(1);
        //        Item temp;

        //        if (children.Where(v => v.DataType == "LIST").Count() > 0)
        //        {
        //            temp = Item.L(children.Select(v => Item.I2(69)).ToArray());
        //        }
        //        else
        //        {
        //            temp = Item.L(children.Select(v => Item.I2(69)).ToArray());
        //        }

        //        MessageBox.Show(temp.ToJson());
        //    }

        //}
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
