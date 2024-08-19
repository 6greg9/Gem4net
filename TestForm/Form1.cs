using Gem4NetRepository;
using Gem4NetRepository.Model;
using Gem4Net;
using Secs4Net;
using Secs4Net.Sml;
using Secs4Net.Extensions;
using Secs4Net.Json;
using Microsoft.Extensions.Options;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Diagnostics;
using System.Text.Json;
using Dapper;
using System.Data.SQLite;
using System.Data;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Gem4Net.Control;
using System.Configuration;

namespace TestForm;

public partial class Form1 : Form
{
    GemRepository _gemRepo;
    GemEqpService GemEquipment;
    IConfiguration configuration;
    public Form1()
    {

        InitializeComponent();
        configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appSettings.json", false)
        .Build();
        _gemRepo = new GemRepository(configuration); //帶入的參數是ConnectionStr的Key



        UpdateVariables();

    }
    int cnt = 0;
    public void UpdateVariables()
    {
        //var cnStr = " Data Source= C:\\Users\\User\\Documents\\GemVariablesDb.sqlite";
        var cnStr = $"Host=127.0.0.1:5432; Database=GemEqpDb; Username=postgres; Password=greg3058;";
        Task.Run(async () =>
        {
            while (true)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                try
                {

                    //using (IDbConnection cn = new SQLiteConnection(cnStr))
                    using (IDbConnection cn = new NpgsqlConnection(cnStr))
                    {
                        //var tran = cn.BeginTransaction();
                        //string strSql = "UPDATE Variables SET Value=@value WHERE VID =@vid ;" ;//這樣會生成N筆SQL

                        ////刪除多筆參數
                        //var datas = new[]{
                        //  new { vid = "1001", value = Item.F4((float)(cnt * 0.1)).ToJson() }  //};
                        //, new { vid = "1002", value = Item.F4((float)(cnt * 0.2)).ToJson() }
                        //, new { vid = "1003", value = Item.F4((float)(cnt * 0.3)).ToJson() }//};
                        //, new { vid = "1004", value = Item.F4((float)(cnt * 0.4)).ToJson() }
                        //, new { vid = "1005", value = Item.F4((float)(cnt * 0.5)).ToJson() }
                        //, new { vid = "1006", value = Item.F4((float)(cnt * 0.6)).ToJson() }
                        //, new { vid = "1007", value = Item.F4((float)(cnt * 0.7)).ToJson() }
                        //, new { vid = "1008", value = Item.F4((float)(cnt * 0.8)).ToJson() }
                        //, new { vid = "1009", value = Item.F4((float)(cnt * 0.9)).ToJson() }
                        //, new { vid = "1010", value = Item.F4((float)(cnt * 1.0)).ToJson() }}; //似乎沒有顯著隨著row數目增加花費時間

                        //刪除多筆參數
                        //var datas = new[]{
                        //  new { vid = "1001", value = (cnt * 0.1).ToString("0.##") }  //};
                        //, new { vid = "1002", value = (cnt * 0.2).ToString("0.##") }
                        //, new { vid = "1003", value = (cnt * 0.3).ToString("0.##") }//};
                        //, new { vid = "1004", value = (cnt * 0.4).ToString("0.##") }
                        //, new { vid = "1005", value = (cnt * 0.5).ToString("0.##") }
                        //, new { vid = "1006", value = (cnt * 0.6).ToString("0.##") }
                        //, new { vid = "1007", value = (cnt * 0.7).ToString("0.##") }
                        //, new { vid = "1008", value = (cnt * 0.8).ToString("0.##") }
                        //, new { vid = "1009", value = (cnt * 0.9).ToString("0.##") }
                        //, new { vid = "1010", value = (cnt * 1.0).ToString("0.##") }}; //似乎沒有顯著隨著row數目增加花費時間
                        //var sql = "UPDATE \"Variables\" SET \"Value\" =  CASE \"VID\"";
                        //var inStr = "";
                        //foreach (var data in datas)
                        //{
                        //    var caseStr = " WHEN " + data.vid.ToString() + " THEN '" + data.value.ToString() + "'";
                        //    sql += caseStr;
                        //    inStr += " ," + data.vid.ToString();
                        //}
                        //sql += "ELSE \"Value\" END  WHERE \"VID\" IN ( " + inStr.Substring(2) + ")";//土炮

                        //cn.Execute(sql);
                        //tran.Commit();
                    }

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
                cnt += 1;
                //await Task.Delay(20);
                //Thread.Sleep(20);

                SpinWait.SpinUntil(() => false, 20);
                sw.Stop();
                //Debug.WriteLine(sw.ElapsedMilliseconds + " ms");
                Debug.WriteLine($" {sw.ElapsedTicks * 1000F / Stopwatch.Frequency:n3}ms");
            }
            //交易
            //using (var tranScope = new TransactionScope())
            //{


            //}

        });
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
        GemEquipment.Enable();
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
        var secs = GemEquipment.GetSecsWrapper;
        var S1F1 = new SecsMessage(1, 1);
        var S1F2 = await secs.SendAsync(S1F1);
        MessageBox.Show(S1F2.ToSml());
    }

    private void Btn_GoOffLine_Click(object sender, EventArgs e)
    {
        GemEquipment.GoOffline();
    }

    private void Btn_GoOnLine_Click(object sender, EventArgs e)
    {
        GemEquipment.RequestOnline();
    }

    private void Btn_GoLocal_Click(object sender, EventArgs e)
    {
        GemEquipment.GoOnlineLocal();
    }

    private void Btn_GoRemote_Click(object sender, EventArgs e)
    {
        GemEquipment.GoOnlineRemote();
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
        var test = _gemRepo.GetReportsByCeid(1);
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
        GemEquipment.SendEventReport(inputId, false);
    }

    private void Btn_S10F1TerminalRequest_Click(object sender, EventArgs e)
    {
        GemEquipment.SendTerminalMessageAsync((string)Tbx_TerminalInput.Text, 87, true);
    }

    private void Btn_InsertPP_Click(object sender, EventArgs e)
    {
        var pp = new FormattedProcessProgram();
        pp.LogId = Guid.NewGuid();
        pp.PPID = "test"; //+ DateTime.Now.ToString("YYYYMMddhhmmss");
        pp.UpdateTime = DateTime.UtcNow;

        pp.Editor = "87";
        pp.ApprovalLevel = "-1";
        pp.Description = "sss";
        pp.SoftwareRevision = "fff";
        pp.EquipmentModelType = "model";
        var ppBody = new List<ProcessCommand>();
        var temperatureCmd = new ProcessCommand();
        temperatureCmd.CommandCode = "temperCC";
        temperatureCmd.ProcessParameters.Add(
            new ProcessParameter
            {
                Name = "TempA",
                Value = "87.9",
                DataType = "FLOAT_4",
                Unit = "C",
                Length = 8,
                Definition = "test",
                Remark = "YOOOOOOO"
            });
        ppBody.Add(temperatureCmd);
        pp.PPBody = JsonSerializer.Serialize(ppBody);
        _gemRepo.CreateFormattedProcessProgram(pp);
        richTextBox2.Text = pp.PPID;
    }

    private void Btn_SelectAllPP_Click(object sender, EventArgs e)
    {
        var pps = _gemRepo.GetFormattedPPAll().Select(pp => pp).ToList();
        var ppBody = pps.FirstOrDefault().PPBody;
        var p = JsonSerializer.Deserialize<List<ProcessCommand>>(ppBody);
    }
    // 父 as 子 是沒用的...
    public class FormattedPP : FormattedProcessProgram
    {
        public List<ProcessCommand>? Commands { get; private set; }
        public FormattedPP()
        {
            try
            {
                Commands = JsonSerializer.Deserialize<List<ProcessCommand>>(this.PPBody);
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

        }
    }
    private void Btn_SendAlarm_Click(object sender, EventArgs e)
    {
        GemEquipment.SendAlarmReport(Cbx_SetAlarm.Checked ? 128 : 0, (int)Num_AlarmId.Value);
    }

    private void Btn_DeletePP_Click(object sender, EventArgs e)
    {
        _gemRepo.DeleteFormattedProcessProgram(new List<string> { richTextBox2.Text });
    }

    private void timer1_Tick(object sender, EventArgs e)
    {

    }

    private void Form1_Load(object sender, EventArgs e)
    {
        ISecsGemLogger logger = new SecsLogger(this);

        var secsGemOptions = Options.Create(new SecsGemOptions
        {
            IsActive = true,
            IpAddress = "127.0.0.1",
            Port = 5000,
            //SocketReceiveBufferSize = 8096,
            SocketReceiveBufferSize = 1024,
            DeviceId = 1,
            LinkTestInterval = 1000 * 60,
            T6 = 5000
        });
        var gemEqpAppOptions = Options.Create(new GemEqpAppOptions
        {
            ModelType = "MDLN",
            SoftwareVersion = "1.2.3.4",
            IsCommDefaultEnabled = 1,
            IsCommHostInit = 0,
            TimeFormatVID = 20,
            DefaultInitControlState = (int)ControlState.ATTEMPT_ON_LINE,
            DefaultAfterFailOnline = (int)ControlState.EQUIPMENT_OFF_LINE,
            DefaultLocalRemote = (int)ControlState.LOCAL,
            EstablishCommunicationsTimeout = 10
        });
        ;
        GemEquipment = new GemEqpService(logger, _gemRepo, secsGemOptions, gemEqpAppOptions); // 建構式就啟動惹..

        GemEquipment.OnConnectStatusChanged += (status) =>
        {
            this.Invoke(new Action(() => { rtbx_HSMS.AppendText($"{status}\n"); ; }));
        };
        GemEquipment.OnCommStateChanged += (cur, pre) =>
        {
            this.Invoke(new Action(() => { rtbx_Comm.AppendText($"{pre} --> {cur}\n"); ; }));
        };
        GemEquipment.OnControlStateChanged += (current, previous) =>
        {
            this.Invoke(new Action(() => { rtbx_Ctrl.AppendText($"{previous} --> {current}\n"); ; }));
        };

        GemEquipment.OnTerminalMessageReceived += (msg) =>
        {
            this.Invoke(new Action(() =>
            {
                Tbx_Terminal.AppendText(msg + "\n");
            }));
            return 0;
        };
        GemEquipment.OnRemoteCommandReceived += (remoteCmd) =>
        {
            var rtn = remoteCmd;
            rtn.HCACK = 0;
            rtn.Parameters.ForEach(p =>
            {
                p.CPACK = 0;
            });
            return rtn;
        };
        GemEquipment.OnEcRecieved += (ecLst) =>
        {
            return 0; // OK
        };

        GemEquipment.OnFormattedProcessProgramReceived += (fppSecs) =>
        {
            //return 0; // 要自行依照process program 結構來處理
            var pp = new FormattedProcessProgram();
            var result = _gemRepo.PharseSecsItemToFormattedPP(fppSecs, out pp);
            var ppCmds = JsonSerializer.Deserialize<List<ProcessCommand>>(pp.PPBody);
            var paraA = ppCmds.FirstOrDefault().ProcessParameters.FirstOrDefault();
            var rtn = _gemRepo.CreateFormattedProcessProgram(pp);
            return rtn.Result;
        };
        GemEquipment.OnProcessProgramDeleteReq += (ppLst) =>
        {

            if (ppLst.Count == 0)
            {
                _gemRepo.DeleteFormattedPPAll();
            }
            else
            {
                _gemRepo.DeleteFormattedProcessProgram(ppLst);
            }
            return 0;
        };
    }

    private void button4_Click(object sender, EventArgs e)
    {
        var ttt = Item.A("yooooooo").ToString();
    }
}
