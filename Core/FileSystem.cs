using System.IO;
using System.Collections.Generic;
using static UKit.Utils.Output;
namespace UKit.Core{

    public class FilterOption{
        public FilterOption(){
            this.ExcludeNames = new HashSet<string>();
            this.ExcludeExtensions = new HashSet<string>();
        }
        public HashSet<string> ExcludeNames{get; set;}
        public HashSet<string> ExcludeExtensions{get; set;}
    }

    public class FileSystem 
    {
        /// <summary>
        /// 拷贝文件夹
        /// </summary>
        /// <param name="strSource">源文件夹</param>
        /// <param name="strDestination">目的文件夹</param>
        /// <param name="option">过滤选项，支持按名称及后缀过滤</param>
        public static void CopyFolder(string strSource, string strDestination, FilterOption option = null){
            if (option == null){
                option = new FilterOption();
            }
            foreach(string from in Directory.GetFiles(strSource, "*.*", SearchOption.AllDirectories)){
                bool filter = false;
                foreach(string name in option.ExcludeNames){
                    if(from.Contains(name)){
                        filter = true;
                        break;
                    }
                }
                if(!filter){
                    foreach(string ex in option.ExcludeExtensions){
                        if(from.EndsWith("." + ex)){
                            filter = true;
                            break;
                        }
                    }
                }
                if(!filter){
                    CopyFile(from, from.Replace(strSource, strDestination));
                }
            }
        }

        public static void CopyFile(string raw, string copy){
            string extension = Path.GetExtension(raw);
            if(File.Exists(copy)){
                File.Delete(copy);
            }
            if(File.Exists(raw)){
                string path = Path.GetDirectoryName(copy);
                if(!Directory.Exists(path)){
                    Directory.CreateDirectory(path);
                }
                File.Copy(raw, copy);
            }
        }

        public static void ClearFolder(string path){
            if(Directory.Exists(path)){
                Directory.Delete(path);
            }
        }
    }

}

