namespace ToolForm;

using CsvHelper;
using Gem4NetRepository.Model;
using static Dapper.SqlMapper;

using System.Globalization;
using System.Data.SQLite;
using System.Data;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }
    static string dbPath = @".\GemVariablesDb.sqlite";
    static string cnStr = "data source=" + dbPath;
    private void Btn_InsertVariableTable_Click(object sender, EventArgs e)
    {
        OpenFileDialog dialog = new ();
        var result = dialog.ShowDialog();
        if (result == DialogResult.OK)
        {
            using var reader = new StreamReader(dialog.FileName);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<ItriVarCsv>().ToList();
            var variables = records.Select(row =>
                new GemVariable { 
                                VID = row.ID ,
                                DataType = row.Type, 
                                Definition= row.Define,
                                Name= row.Name ,
                                Remark= row.Remark,
                                System = true,
                                VarType = row.VarType,
                })
            ;
            using (IDbConnection cn = new SQLiteConnection(cnStr))
            {

                //參數是用@paramName
                //var insertScript =
                //    "INSERT INTO Variables VALUES (@Id, @Name, @RegDate, @Score, @BinData)";
                 cn.Execute("INSERT INTO Variables (VID, DataType, Definition, Name, Remark, System, VarType)" +
                    " VALUES (@VID, @DataType, @Definition, @Name, @Remark, @System, @VarType)", variables);

            }

        }
    }
}
