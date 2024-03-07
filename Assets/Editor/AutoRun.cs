#region USING
using System.IO;

using UnityEditor;

using UnityEngine;
#endregion

[InitializeOnLoad]
public class AutoRun : MonoBehaviour
{
      #region AutoRun
      static AutoRun()
      {
            EditorApplication.delayCall += AutoStart;
      }
      static void AutoStart()
      {
#if UNITY_EDITOR
            EncoderUTF8();
#endif
      }
      #endregion 

      #region EncoderUTF8  c# 스크립트를 UTF-8 로 인코딩
      public static void EncoderUTF8()
      {
            //에디터 설정 파일 위치
            string path = Path.Combine(Application.dataPath, "..", ".editorconfig");

            //파일이 있는지 찾는다.
            if (!File.Exists(path))
            {
                  //없으면 만든다.
                  using (StreamWriter sw = File.CreateText(path))
                  {
                        sw.WriteLine("[*]");
                        sw.WriteLine("charset = utf-8");
                  }
            }
            else
            {
                  string[] lines = File.ReadAllLines(path);
                  bool fSection = false;
                  bool fCharset = false;

                  foreach (string line in lines)
                  {
                        //파일이 존재 하면 Section과 charset를 찾는다
                        if (!fSection && line.Contains("[*]"))
                        {
                              fSection = true;
                        }
                        else if (fSection && !fCharset && line.Trim() == "charset = utf-8")
                        {
                              fCharset = true;
                        }
                  }

                  //세션이 없으면 추가한다.
                  if (!fSection)
                  {
                        using (StreamWriter sw = File.AppendText(path))
                        {
                              sw.WriteLine("[*]");
                        }
                  }
                  //캐릭터셋이 없으면 추가한다.
                  if (!fCharset)
                  {
                        using (StreamWriter sw = File.AppendText(path))
                        {
                              sw.WriteLine("charset = utf-8");
                        }
                  }
            }
      }
      #endregion
}