using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace ImageProcessing.Utility
{
    public static class TextureUtility
    {
        [CanBeNull]
        public static Texture2D WriteTextureToNewPNG(Texture2D texture, string path, string fileName)
        {
            try
            {
                byte[] bytes = texture.EncodeToPNG();
                var pathToDesktopDir = Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) +
                                       "\\ImageProcessing\\" + path + "\\";
                if (!Directory.Exists(pathToDesktopDir))
                {
                    Directory.CreateDirectory(pathToDesktopDir);
                }

                File.WriteAllBytes(pathToDesktopDir + fileName + ".png", bytes);

                //File.WriteAllBytes(Application.dataPath + fileName + ".png" , bytes);

                //System.IO.File.WriteAllBytes(path, bytes);
                Debug.Log(bytes.Length / 1024 + "Kb was saved as: " + pathToDesktopDir + fileName);
                AssetDatabase.CreateAsset(texture, "Assets/Result/" + fileName + ".asset");
                Texture2D savedTexture =
                    (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Result/" + fileName + ".asset", typeof(Texture2D));
                return savedTexture;
            }
            catch (Exception e)
            {
                Debug.Log(e);
                return null;
            }
        }
    }
}