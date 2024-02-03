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
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text.Json;
using Secs4Net;
using static Secs4Net.Item;
public partial class Form1 : Form
{
    GemRepository _gemRepo;
    GemEqpService service;
    public Form1()
    {
        InitializeComponent();
        _gemRepo = new GemRepository("GemVariablesDb.sqlite");

        ISecsGemLogger logger = new SecsLogger(this);

        service = new GemEqpService(logger, _gemRepo, new SecsGemOptions
        {
            IsActive = true,
            IpAddress = "127.0.0.1",
            Port = 5000,
            //SocketReceiveBufferSize = 8096,
            SocketReceiveBufferSize = 1024,
            DeviceId = 0,
            LinkTestInterval = 1000 * 60,
            T6 = 5000
        });
        service.OnConnectStatusChanged += (status) =>
        {
            this.Invoke(new Action(() => { rtbx_HSMS.AppendText($"{status}\n"); ; }));
        };
        service.OnCommStateChanged += (current, previous) =>
        {
            this.Invoke(new Action(() => { rtbx_Comm.AppendText($"{previous} --> {current}\n"); ; }));
        };
        service.OnControlStateChanged += (current, previous) =>
        {
            this.Invoke(new Action(() => { rtbx_Ctrl.AppendText($"{previous} --> {current}\n"); ; }));
        };
        service.OnTerminalMessageReceived += (msg) =>
        {
            this.Invoke(new Action(() =>
            {
                Tbx_Terminal.AppendText(msg + "\n");
            }));
            return 0;
        };
        service.OnRemoteCommandReceived += (remoteCmd) =>
        {
            var rtn = remoteCmd;
            rtn.HCACK = 0;
            rtn.Parameters.ForEach(p =>
            {
                p.CPACK = 0;
            });
            return rtn;
        };
        service.OnEcRecieved += (ecLst) =>
        {
            return 0; // OK
        };
        service.OnFormattedProcessProgramReceived += (formatedPP) =>
        {
            return 0; // 要自行依照process program 結構來處理
            var pp = new FormattedProcessProgram();
            pp.PPID = formatedPP.Items[0].GetString();
            pp.EquipmentModelType = formatedPP.Items[1].GetString();
            pp.SoftwareRevision = formatedPP.Items[2].GetString();
            foreach (var processCmd in formatedPP.Items[3].Items)
            {
                var pCmd = new ProcessCommand { CommandCode = processCmd.Items[0].GetString() };
                var paras = processCmd.Items[1];
                foreach (var para in paras.Items) // 這個要很注意客製
                {
                    var p = new ProcessParameter();
                    p.Value = para.GetString();
                }
            }
            var rtn = _gemRepo.CreateProcessProgram(pp);
        };
        service.OnFormattedProcessProgramReq += (ppid) => // 好像可以自動處理?
        {
            var fpp = _gemRepo.GetProcessProgramFormatted(ppid).ToList();
            return _gemRepo.FormattedProcessProgramToSecsItem(fpp.First());
        };
        service.OnProcessProgramDeleteAllReq += () =>
        {
            return _gemRepo.DeleteProcessProgramAll();
        };
        service.OnProcessProgramDeleteReq += (ppLst) =>
        {
            return _gemRepo.DeleteProcessProgram(ppLst);
        };
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
    
    private void button1_Click(object sender, EventArgs e)
    {

        var test = _gemRepo.GetSv(9);
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
    

    /// <summary>
    /// 粗暴用法
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void button3_Click(object sender, EventArgs e)
    {
        var secs = service.GetSecsWrapper;
        var S1F1 = new SecsMessage(1, 1);
        var S1F2 = await secs.SendAsync(S1F1);
        MessageBox.Show(S1F2.ToSml());
    }

    private void Btn_GoOffLine_Click(object sender, EventArgs e)
    {
        service.GoOffline();
    }

    private void Btn_GoOnLine_Click(object sender, EventArgs e)
    {
        service.RequestOnline();
    }

    private void Btn_GoLocal_Click(object sender, EventArgs e)
    {
        service.GoOnlineLocal();
    }

    private void Btn_GoRemote_Click(object sender, EventArgs e)
    {
        service.GoOnlineRemote();
    }


    void Temp()
    {
        ///首次會1000~3000ms
        ///之後約20ms
        //Stopwatch sw = new Stopwatch();
        //sw.Start();
        //var test = _gemRepo.SetVarValueById(1, 44);
        //sw.Stop();
        //MessageBox.Show($" {sw.ElapsedTicks * 1000F / Stopwatch.Frequency:n3}ms");
        //MessageBox.Show(test.ToString());
        //sw.Restart();
        //var testEC = _gemRepo.SetVarValueById(7, 2);
        //sw.Stop();
        //MessageBox.Show($" {sw.ElapsedTicks * 1000F / Stopwatch.Frequency:n3}ms");
        //MessageBox.Show(testEC.ToString());
        //var testNull = _gemRepo.SetECByIdLst(new List<(int, object)> { (7, 3) });
        //sw.Stop();
        //MessageBox.Show($" {sw.ElapsedTicks * 1000F / Stopwatch.Frequency:n3}ms");
        //MessageBox.Show(testNull.ToString());
    }

    private void Btn_TestGetEvents_Click(object sender, EventArgs e)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        var test = _gemRepo.GetReport(1);
        sw.Stop();
        MessageBox.Show(test.ToJson());
        MessageBox.Show($" {sw.ElapsedTicks * 1000F / Stopwatch.Frequency:n3}ms");
    }

    private void Btn_GetSvById_Click(object sender, EventArgs e)
    {
        var inputId = Convert.ToInt32(Tbx_InputVid.Text.Trim());
        Stopwatch sw = new Stopwatch();
        sw.Start();
        var test = _gemRepo.GetSv(inputId);
        sw.Stop();
        MessageBox.Show($" {sw.ElapsedTicks * 1000F / Stopwatch.Frequency:n3}ms");
        MessageBox.Show(test.ToJson());
    }

    private void Btn_UpdateSV_Click(object sender, EventArgs e)
    {
        //var inputId = Convert.ToInt32(Tbx_InputECID.Text);
        //var inputValue = Tbx_InputVid.Text; //先這樣...
        //Stopwatch sw = new Stopwatch();
        //sw.Start();
        //var test = _gemRepo.SetVarValueById(1, 44);
        //sw.Stop();
        //MessageBox.Show($" {sw.ElapsedTicks * 1000F / Stopwatch.Frequency:n3}ms");
        //MessageBox.Show(test.ToString());
    }

    private void Btn_TestSendS6F11_Click(object sender, EventArgs e)
    {
        var inputId = Convert.ToInt32(Tbx_InputECID.Text.Trim());
        service.SendEventReport(inputId);
    }

    private void Btn_S10F1TerminalRequest_Click(object sender, EventArgs e)
    {
        service.SendTerminalMessageAsync((string)Tbx_TerminalInput.Text, 87);
    }

    private void Btn_InsertPP_Click(object sender, EventArgs e)
    {
        var pp = new FormattedProcessProgram();
        pp.ID = Guid.NewGuid();
        pp.PPID = "test" + DateTime.Now.ToString("YYYYMMddhhmmss");
        pp.UpdateTime = DateTime.Now;
        pp.Status = 1;
        pp.Editor = "87";
        pp.ApprovalLevel = "-1";
        pp.Description = "sss";
        pp.SoftwareRevision = "fff";
        pp.EquipmentModelType = "model";
        var ppBody = new List<ProcessCommand>();
        var temperatureCmd = new ProcessCommand();
        temperatureCmd.CommandCode = "temperCC";
        temperatureCmd.ProcessParameters.Add(
            new ProcessParameter { Name = "TempA", Value = "87.9", 
                DataType = "FT_4", Unit="C",Length=8, Definition="test", Remark="YOOOOOOO" });
        ppBody.Add(temperatureCmd);
        pp.PPBody = JsonSerializer.Serialize(ppBody);
        _gemRepo.CreateProcessProgram(pp);
    }

    private void Btn_SelectAllPP_Click(object sender, EventArgs e)
    {

    }
}
