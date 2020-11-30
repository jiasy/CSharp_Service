using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils {
    public class FileUtils {

        public static bool IsFileExists (string path) {
            return new FileInfo (path).Exists;
        }

        public static bool Write (string path_, string data_, bool isAppend_ = false) {
            path_ = Application.dataPath + "/" + path_;
            StreamWriter sw;
            FileInfo fi = new FileInfo (path_);
            if (!IsFileExists (path_)) {
                Directory.CreateDirectory (Path.GetDirectoryName (path_));
            }
            if (!isAppend_)
                sw = fi.CreateText ();
            else
                sw = fi.AppendText ();
            sw.WriteLine (data_);
            sw.Close ();
            sw.Dispose ();
            return false;
        }

        public static ArrayList ReadFileToArray (string path_) {
            path_ = Application.dataPath + "/" + path_;
            if (!IsFileExists (path_)) {
                return null;
            }

            StreamReader Reader = File.OpenText (path_);
            string t_sLine;
            ArrayList t_aArrayList = new ArrayList ();
            while ((t_sLine = Reader.ReadLine ()) != null) {
                t_aArrayList.Add (t_sLine);
            }
            Reader.Close ();
            Reader.Dispose ();

            return t_aArrayList;
        }

        public static string ReadFileToString (string path_) {
            path_ = Application.dataPath + "/" + path_;
            if (!IsFileExists (path_)) return null;

            StreamReader Reader = File.OpenText (path_);
            string all = Reader.ReadToEnd ();
            Reader.Close ();
            Reader.Dispose ();

            return all;
        }
    }
}