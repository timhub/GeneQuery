using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CommonMysql;

namespace BackingFunctions
{
    public class CsvFileOperator
    {
        public CsvFileOperator()
        {
        }

        //Only do the save
        public void CsvFileSave(ObservableCollection<ItemDataBean> list)
        {
            var basePath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var dateTime = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
            dateTime = dateTime.Replace("/", "-");
            dateTime = dateTime.Replace(":", ".");
            basePath = Path.Combine(basePath, @"data_csv\");
            var filePath = CreateFile(basePath, dateTime + "backup", "csv");
            var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);

            var sw = new StreamWriter(fileStream, UTF8Encoding.UTF8);
            WriteHeader(sw);
            foreach (var itemDataBean in list)
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.Append(itemDataBean.Id)
                             .Append(",")
                             .Append(itemDataBean.FId)
                             .Append(",")
                             .Append(itemDataBean.MId)
                             .Append(",")
                             .Append(itemDataBean.Nation)
                             .Append(",")
                             .Append(itemDataBean.Gender)
                             .Append(",")
                             .Append(itemDataBean.Condition)
                             .Append(",")
                             .Append(itemDataBean.EBVFC)
                             .Append(",")
                             .Append(itemDataBean.TPI)
                             .Append(",")
                             .Append(itemDataBean.D)
                             .Append(",")
                             .Append(itemDataBean.H)
                             .Append(",")
                             .Append(itemDataBean.R)
                             .Append(",")
                             .Append(itemDataBean.EBVM)
                             .Append(",")
                             .Append(itemDataBean.T)
                             .Append(",")
                             .Append(itemDataBean.EBVP)
                             .Append(",")
                             .Append(itemDataBean.EBCMS)
                             .Append(",")
                             .Append(itemDataBean.FL)
                             .Append(",")
                             .Append(itemDataBean.SCS)
                             .Append(",")
                             .Append(itemDataBean.Others);

                sw.WriteLine(stringBuilder.ToString());
            }
            sw.Close();
            sw.Dispose();
        }

        //write the header of the chart
        private void WriteHeader(StreamWriter streamWriter)
        {
            streamWriter.WriteLine("个体编号,父亲编号,母亲编号,国籍,性别,现有个体,外貌等级,总性能指数," +
                                   "女儿数,牛群数,可靠性,产奶量育种值,体型,乳蛋白量育种值,泌乳系统,肢蹄,体细胞,其他");
        }

        private string CreateFile(string folder, string fileName, string fileExtension)
        {
            FileStream fs = null;
            string filePath = folder + fileName + "." + fileExtension;
            try
            {
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                fs = File.Create(filePath);
            }
            catch (Exception ex)
            { }
            finally
            {
                if (fs != null)
                {
                    fs.Dispose();
                }
            }
            return filePath;
        }
    }
}
