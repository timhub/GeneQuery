using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;

namespace GeneQueryNGUI.Utilities.IniUtilities
{
    public class IniOperationUtilities
    {
        [DllImport("kernel32.dll")] 
        private static extern int GetPrivateProfileString(string section, string key, string defVal, StringBuilder retVal, int size, string filePath);          
        /// <summary> 
        /// 读操作读取整数         
        /// </summary> 
        /// <param name="lpAppName">指向包含Section 名称的字符串地址</param>         
        /// <param name="lpKeyName">指向包含Key 名称的字符串地址</param>         
        /// <param name="nDefault">如果Key 值没有找到，则返回缺省的值是多少</param> 
        /// <param name="lpFileName">INI文件的完整路径和文件名</param>         
        /// <returns>返回获得的整数值</returns>         
        [DllImport("kernel32")] 
        private static extern int GetPrivateProfileInt(string lpAppName, string lpKeyName, int nDefault, string lpFileName);  

        /// <summary>         
        /// 写操作         
        /// </summary> 
        /// <param name="section">要写入的段落名</param> 
        /// <param name="key">要写入的键，如果该key存在则覆盖写入</param>         
        /// <param name="val">key所对应的值</param> 
        /// <param name="filePath">INI文件的完整路径和文件名</param>         
        /// <returns></returns>         
        [DllImport("kernel32")] 
        private static extern bool WritePrivateProfileString(string section, string key, string val, string filePath);  

        /// <summary>         
        /// 获得整数值         
        /// </summary> 
        /// <param name="section">要读取的段落名</param>         
        /// <param name="key">要读取的键</param> 
        /// <param name="def">如果Key 值没有找到，则返回缺省的值是多少</param>         
        /// <param name="fileName">INI文件的完整路径和文件名</param>         
        /// <returns></returns> 
        public static int GetInt(string section, string key, int def, string fileName)          
        { 
            return GetPrivateProfileInt(section, key, def, fileName);         
        }  

        /// <summary> 
        /// 获得字符串值,默认返回长度为1024         
        /// </summary> 
        /// <param name="section">要读取的段落名</param>         
        /// <param name="key">要读取的键</param> 
        /// <param name="def">如果Key 值没有找到，返回的默认值</param>         
        /// <param name="fileName">INI文件的完整路径和文件名</param>         
        /// <returns></returns> 
        public static string GetString(string section, string key, string def, string fileName)         
        { 
            StringBuilder temp = new StringBuilder(1024); 
            int result = GetPrivateProfileString(section, key, def, temp, 1024, fileName);             
            return temp.ToString();         
        }  
        
        /// <summary> 
        /// 获得字符串值，返回长度用户自定义         
        /// </summary> 
        /// <param name="section">要读取的段落名</param>         
        /// <param name="key">要读取的键</param> 
        /// <param name="def">如果Key 值没有找到，返回的默认值</param>         
        /// <param name="fileName">INI文件的完整路径和文件名</param>         
        /// <param name="size">用户自定义返回的字符串长度</param>         
        /// <returns></returns> 
        public static string GetString(string section, string key, string def, string fileName, int size)         
        { 
            StringBuilder temp = new StringBuilder(); 
            int result = GetPrivateProfileString(section, key, def, temp, size, fileName);             
            return temp.ToString();         
        }  

        /// <summary>         
        /// 写整数值         
        /// </summary> 
        /// <param name="section">要写入的段落名</param> 
        /// <param name="key">要写入的键，如果该key存在则覆盖写入</param>         
        /// <param name="iVal">key所对应的值</param> 
        /// <param name="fileName">INI文件的完整路径和文件名</param> 
        public static void WriteInt(string section, string key, int iVal, string fileName)         
        { 
            WritePrivateProfileString(section, key, iVal.ToString(), fileName);         
        }  

        /// <summary>         
        /// 写字符串的值         
        /// </summary> 
        /// <param name="section">要写入的段落名</param> 
        /// <param name="key">要写入的键，如果该key存在则覆盖写入</param>         
        /// <param name="strVal">key所对应的值</param> 
        /// <param name="fileName">INI文件的完整路径和文件名</param> 
        public static void WriteString(string section, string key, string strVal, string fileName)         
        { 
            WritePrivateProfileString(section, key, strVal, fileName);         
        }  

        /// <summary>         
        /// 删除指定的key         
        /// </summary> 
        /// <param name="section">要写入的段落名</param>         
        /// <param name="key">要删除的键</param> 
        /// <param name="fileName">INI文件的完整路径和文件名</param> 
        public static void DelKey(string section, string key, string fileName)         
        { 
            WritePrivateProfileString(section, key, null, fileName);         
        }  
        /// <summary> 
        /// 删除指定的段落         
        /// </summary> 
        /// <param name="section">要删除的段落名</param> 
        /// <param name="fileName">INI文件的完整路径和文件名</param>         
        public static void DelSection(string section, string fileName) 
        { 
            WritePrivateProfileString(section, null, null, fileName);         
        }  
    }
}
