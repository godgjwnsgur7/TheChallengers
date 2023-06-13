using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class BadWordDiscriminator : MonoBehaviour
{
    static string[] badWordStrArray;

    static int badWordListCount = 2;

    public static bool Discriminator(string str)
    {
        // 조건 확인

        for(int i = 0; i < badWordListCount; i++)
        {
            if (File.Exists($"Resources/BadWordFile/BadWordList{i + 1}.txt"))
            {
                StreamReader word = new StreamReader($"Resources/BadWordFile/BadWordList{i + 1}.txt");
                string source = word.ReadToEnd();
                word.Close();

                badWordStrArray = Regex.Split(source, @"\r\n|\n\r|\n|\r");
            }


        }

        // 
        return true;
    }
}
