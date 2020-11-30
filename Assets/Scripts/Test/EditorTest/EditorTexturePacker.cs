using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Sprites;

namespace flash.display.EditorTest {
    using UnityEngine;
    public class EditorTexturePacker : Editor {
        [MenuItem("Test/FindAssetsUsingSearchFilter")]
        static void FindAssetsUsingSearchFilter () 
        {
            // Find all assets labelled with 'concrete' : 
            var guids = AssetDatabase.FindAssets ("l:concrete", null);
            foreach (var guid in guids)
                Debug.Log (AssetDatabase.GUIDToAssetPath(guid));
            
            // Find all Texture2Ds that have 'co' in their filename, that are labelled with 'concrete' and 'architecture' and are placed in 'MyAwesomeProps' folder
            var guids2 = AssetDatabase.FindAssets ("co l:concrete l:architecture t:texture2D", new string[]{"Assets/MyAwesomeProps"});
            foreach (var guid in guids2)
                Debug.Log (AssetDatabase.GUIDToAssetPath(guid));

            var guids3 = AssetDatabase.FindAssets ("t:Sprite", new string[]{"Assets/Resources/flash"});
            foreach (var guid in guids3)
                Debug.Log (AssetDatabase.GUIDToAssetPath(guid));
        }

        [MenuItem ("GameObject/SearchAtlas", false, 0)]
        static void StartInitializeOnLoadMethod1 () {
            //需要Sprite Packer界面定位的图集名称
            string spriteName = "atlas_name2";
            //设置使用采取图集的方式
            EditorSettings.spritePackerMode = SpritePackerMode.AlwaysOn;
            //打包图集
            Packer.RebuildAtlasCacheIfNeeded (EditorUserBuildSettings.activeBuildTarget, true);
            //打开SpritePack窗口
            EditorApplication.ExecuteMenuItem ("Window/Sprite Packer");

            //反射遍历所有图集
            var type = typeof (EditorWindow).Assembly.GetType ("UnityEditor.Sprites.PackerWindow");
            var window = EditorWindow.GetWindow (type);
            FieldInfo infoNames = type.GetField ("m_AtlasNames", BindingFlags.NonPublic | BindingFlags.Instance);
            string[] infoNamesArray = (string[]) infoNames.GetValue (window);

            if (infoNamesArray != null) {
                for (int i = 0; i < infoNamesArray.Length; i++) {
                    if (infoNamesArray[i] == spriteName) {
                        //找到后设置索引
                        FieldInfo info = type.GetField ("m_SelectedAtlas", BindingFlags.NonPublic | BindingFlags.Instance);
                        info.SetValue (window, i);
                        break;
                    }
                }
            }
        }

        private static bool CombineSpritesHelper (string path, string dpath, string name, int padding) {
            string[] paths = AssetDatabase.FindAssets ("t:sprite", new string[] { path });
            List<Sprite> spriteList = new List<Sprite> ();
            List<Texture2D> texList = new List<Texture2D> ();

            foreach (var o in paths) {
                Sprite s = AssetDatabase.LoadAssetAtPath (AssetDatabase.GUIDToAssetPath (o), typeof (Sprite)) as Sprite;
                if (null != s) {
                    spriteList.Add (s);
                    texList.Add (s.texture);
                }
            }

            if (texList.Count > 0) {
                Texture2D tex = new Texture2D (1024, 1024, TextureFormat.ARGB32, true);
                Rect[] uvs = UITexturePacker.PackTextures (tex, texList.ToArray (), 4, 4, padding, 1024);
                if (null == uvs) {
                    EditorUtility.DisplayDialog (path, "图集超过1024，需要分组成多张图集", "点击退出");
                    Object.DestroyImmediate (tex);
                    tex = null;
                    return false;
                } else {
                    List<SpriteMetaData> metaList = new List<SpriteMetaData> ();
                    for (int i = 0; i < uvs.Length; ++i) {
                        SpriteMetaData data = new SpriteMetaData ();
                        data.alignment = (int) SpriteAlignment.Center;
                        data.border = spriteList[i].border;
                        data.name = spriteList[i].name;
                        data.pivot = spriteList[i].pivot;
                        data.rect = new Rect (uvs[i].x * tex.width, uvs[i].y * tex.height, uvs[i].width * tex.width, uvs[i].height * tex.height);
                        metaList.Add (data);
                    }

                    //string dpath = path.Substring(0, path.Length - obj.name.Length) + "SpriteSet";
                    if (!System.IO.Directory.Exists (dpath)) {
                        System.IO.Directory.CreateDirectory (dpath);
                    }

                    string file = dpath + "/" + name + ".png";
                    if (System.IO.File.Exists (file)) {
                        System.IO.File.Delete (file);
                    }
                    System.IO.File.WriteAllBytes (file, tex.EncodeToPNG ());

                    AssetDatabase.ImportAsset (file, ImportAssetOptions.ForceUpdate);
                    TextureImporter importer = AssetImporter.GetAtPath (file) as TextureImporter;
                    importer.spritesheet = metaList.ToArray ();
                    importer.spriteImportMode = SpriteImportMode.Multiple;
                    importer.textureType = TextureImporterType.Sprite;
                    importer.textureFormat = TextureImporterFormat.ARGB32;
                    importer.mipmapEnabled = true;
                    importer.mipmapFilter = TextureImporterMipFilter.BoxFilter;
                    importer.assetBundleName = "ui_image/" + name.ToLower ();
                    AssetDatabase.ImportAsset (file);
                    AssetDatabase.Refresh ();
                }
            }
            return true;
        }

        //[MenuItem ("Tool/Combine Sprites")]
        [MenuItem ("Assets/Tool/Combine Sprites")]
        public static void CombineSprites () {
            EditorUtility.DisplayProgressBar ("Combine Sprites", "Initializing ", 0);
            try {
                Object obj = Selection.activeObject;
                string path = AssetDatabase.GetAssetPath (obj.GetInstanceID ());
                string dpath = path.Substring (0, path.Length - obj.name.Length) + "SpriteSet";

                if (System.IO.Directory.Exists (path)) {
                    string[] directories = System.IO.Directory.GetDirectories (path);
                    int count = 0;
                    if (directories.Length > 0) {
                        foreach (var directory in directories) {
                            count++;
                            EditorUtility.DisplayProgressBar ("Combine Sprites", string.Format ("combing {0}", count), (float) (count) / (directories.Length));
                            if (!CombineSpritesHelper (directory, dpath, string.Concat (obj.name, "_", count.ToString ()), 1)) {
                                break;
                            }
                        }
                    } else {
                        EditorUtility.DisplayProgressBar ("Combine Sprites", "combing 0", 1);
                        CombineSpritesHelper (path, dpath, obj.name, 1);
                    }
                }
            } catch (System.Exception e) {
                Debug.LogError (e);
            }
            EditorUtility.ClearProgressBar ();
        }
    }

}